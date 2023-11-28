using System.Text.Json;

namespace srcds_server_query;

public static class Extensions
{
    public static async Task WriteResponse(this HttpResponse res, int statusCode, object body)
    {
        res.StatusCode = statusCode;
        await res.WriteAsync(JsonSerializer.Serialize(body));
    }
}