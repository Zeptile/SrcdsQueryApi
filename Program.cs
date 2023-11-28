using System.Net;
using srcds_server_query;
using srcds_server_query_timeout;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var logger = app.Logger;

app.MapGet("/query", async (HttpRequest req, HttpResponse res) => {
    try 
    {
        var host = req.Query["host"].ToString();
        var port = req.Query["port"].ToString();

        if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port))
        {
            await res.WriteResponse(400, new { error = "host and port are required" });
            return;
        }

        string ip;

        if (IPAddress.TryParse(host, out var ipResult))
        {
            ip = ipResult.ToString();
        }
        else
        {
            var addresses = await Dns.GetHostAddressesAsync(host);
            ip = addresses.First().ToString();
        }

        var endpoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
        var serverQuery = new ServerQuery(endpoint, logger);
        
        await res.WriteResponse(200, await serverQuery.GetServerInfo());
    } 
    catch (QueryTimeoutException e)
    {
        logger.LogError("Timeout while contacting Server - {}", e.Message);
        await res.WriteResponse(500, new { error = "Timeout while contacting Server" });
    } 
    catch (Exception e) 
    {
        logger.LogError("Unexpected error while trying to query server - {}", e.Message);
        await res.WriteResponse(500, new { error = "Unexpected error while trying to query server" });
    }
});

app.Run();
