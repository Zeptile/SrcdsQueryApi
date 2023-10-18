using System.Net;
using srcds_server_query;
using srcds_server_query_timeout;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/query", async (HttpRequest req) => {
    try 
    {
        var host = req.Query["host"].ToString();
        var port = req.Query["port"].ToString();

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
        var serverQuery = new ServerQuery(endpoint, app.Logger);
        
        return await serverQuery.GetServerInfo();
    } 
    catch (QueryTimeoutException e) 
    {
        Console.WriteLine(e.Message);
        throw e;
    } 
    catch (Exception e) 
    {
        Console.WriteLine(e.Message);
        throw e;
    }
});

app.Run();
