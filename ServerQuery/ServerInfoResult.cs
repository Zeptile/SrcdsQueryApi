using System;

namespace srcds_server_query;

public class ServerInfoResult
{
    public int ID;
    public byte VAC;
    public int Protocol { get; set; }
    public string Name { get; set; }
    public string Map { get; set; }
    public string Folder { get; set; }
    public string Game { get; set; }
    public byte Players { get; set; }
    public byte MaxPlayers { get; set; }
    public byte Bots { get; set; }
    public string ServerType { get; set; }
    public string Environment { get; set; }
    public byte Visibility { get; set; }
    public string Version { get; set; }
    public int Port { get; set; }

    public static ServerInfoResult Parse(byte[] data)
    {
        var parser = new ResponseParser(data);
        parser.CurrentPosition += 5; //Header
        
        var result = new ServerInfoResult
        {
            Protocol = parser.GetByte(),
            Name = parser.GetStringToTermination(),
            Map = parser.GetStringToTermination(),
            Folder = parser.GetStringToTermination(),
            Game = parser.GetStringToTermination(),
            ID = parser.GetShort(),
            Players = parser.GetByte(),
            MaxPlayers = parser.GetByte(),
            Bots = parser.GetByte(),
            ServerType = parser.GetStringOfByte(),
            Environment = parser.GetStringOfByte(),
            Visibility = parser.GetByte(),
            VAC = parser.GetByte(),
            Version = parser.GetStringToTermination()
        };

        uint edf = parser.GetByte();

        if ((edf & 0x80) != 0)
        {
            result.Port = parser.GetShort();
        }

        return result;
    }
}