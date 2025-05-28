using System.Text;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var header = context.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(header) || !header.StartsWith("Basic "))
        {
            context.Response.StatusCode = 401;
            context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"MyApp\"";
            await context.Response.WriteAsync("Authorization required");
            return;
        }

        var encoded = header.Substring("Basic ".Length).Trim();
        string decoded;
        try
        {
            decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));
        }
        catch
        {
            context.Response.StatusCode = 400; // Bad Request, niepoprawny Base64
            await context.Response.WriteAsync("Invalid Authorization header");
            return;
        }

        var parts = decoded.Split(':', 2); // rozdziel tylko na dwie części

        if (parts.Length != 2 || parts[0] != "admin" || parts[1] != "password")
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Invalid credentials");
            return;
        }

        // Jeśli wszystko OK - przekazujemy dalej
        await _next(context);
    }
}
