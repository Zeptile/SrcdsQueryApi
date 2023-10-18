using System.Net;
using System.Net.Sockets;
using srcds_server_query_timeout;

namespace srcds_server_query;

public static class QueryResponseHeader
{
    public static byte CHALLENGE_BYTE = 0x41;
    public static byte VALID_RESPONSE_BYTE = 0x49;
}


public static class TimeoutSettings
{
    public const int ReceiveTimeout = 10000000;
}


public class ServerQuery
{
    private readonly ILogger _logger;
    public ServerQuery(IPEndPoint endPoint, ILogger logger)
    {
        EndPoint = endPoint;
        _logger = logger;
    }

    private IPEndPoint EndPoint { get; }

    public async Task<ServerInfoResult> GetServerInfo()
    {
        var requestPacket = BuildSourceEngineServerQuery(null);
        var responseBuffer = await SendServerRequestPacketAsync(requestPacket);
        
        // if challenge header, handle challenge
        if (responseBuffer[4] == QueryResponseHeader.CHALLENGE_BYTE)
        {
            var challenge = responseBuffer[5..];
            _logger.LogDebug("received challenge [{}], sending challenge request", challenge);
            
            var challengeRequestPacket = BuildSourceEngineServerQuery(challenge);
            responseBuffer = await SendServerRequestPacketAsync(challengeRequestPacket);
        }
        
        _logger.LogDebug("received response buffer from server [{}]", responseBuffer);
        
        // if response header still not valid, throw
        if (responseBuffer[4] != QueryResponseHeader.VALID_RESPONSE_BYTE)
        {
            throw new Exception("did not get valid response after challenge");
        }
        
        _logger.LogDebug("valid header type returned, parsing payload...");

        return ServerInfoResult.Parse(responseBuffer);
    }

    private static List<byte> BuildSourceEngineServerQuery(byte[]? challenge)
    {
        var sourceEngineStrByteArr = "Source Engine Query"u8.ToArray();
        var query = new List<byte>(); 

        query.AddRange(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF});
        query.Add(0x54); // Header "T" field
        query.AddRange(sourceEngineStrByteArr); // Payload
        
        query.Add(0x00); // Footer

        if (challenge is not null)
        {
            query.AddRange(challenge);
        }

        return query;
    }

    private async Task<byte[]> SendServerRequestPacketAsync(List<byte> packet)
    {
        using var client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
        client.Connect(EndPoint);

        await client.SendAsync(packet.ToArray(), packet.ToArray().Length);
        var response = Task.Run(() =>
        {
            var task = client.ReceiveAsync();

            task.Wait(TimeoutSettings.ReceiveTimeout);

            if (task.IsCompleted)
                return task.Result;

            throw new QueryTimeoutException($"Query using IP {EndPoint} timed out after {TimeoutSettings.ReceiveTimeout / 1000} seconds.");
        });

        if (response.Result.Buffer is null)
        {
            throw new Exception("invalid response from UDP packet, received zero bytes");
        }

        return response.Result.Buffer;
    }

}
