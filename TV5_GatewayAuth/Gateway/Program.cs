using Yarp.ReverseProxy.Model;
using Yarp.ReverseProxy.Configuration;
using DotNetEnv;
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

//.env
DotNetEnv.Env.Load();

//YARP
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .ConfigureHttpClient((context, handler) =>
    {
        handler.EnableMultipleHttp2Connections = true;
    });

//JWT Authentication
builder.Services.AddAuthentication("Bearer")
   .AddJwtBearer("Bearer", options =>
   {
       var jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");

       if (string.IsNullOrEmpty(jwtKey))
       {
           throw new Exception("JWT_KEY is missing in environment variables");
       }

       options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
               System.Text.Encoding.UTF8.GetBytes(jwtKey))
       };
   });

//Authorization gateway route
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DefaultGatewayPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

//Seperate REST and gRPC to avoid protocol negotiation error
builder.WebHost.ConfigureKestrel(options =>
{
    //REST (Http/1.1)
    options.ListenAnyIP(7000, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1;
    });

    //gRPC (Http/2)
    options.ListenAnyIP(7001, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
    });
});

var app = builder.Build();

app.MapGet("/debug/routes", (Yarp.ReverseProxy.Configuration.IProxyConfigProvider configProvider) =>
{
    var config = configProvider.GetConfig();
    return Results.Ok(new
    {
        Routes = config.Routes,
        Clusters = config.Clusters
    });
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    Console.WriteLine("\n================ [GATEWAY REQUEST INBOUND] ================");
    Console.WriteLine($"Time: {DateTime.Now:HH:mm:ss.fff}");
    Console.WriteLine($"Protocol: {context.Request.Protocol}");
    Console.WriteLine($"Method: {context.Request.Method}");
    Console.WriteLine($"Path: {context.Request.Path}");

    Console.WriteLine("--- Headers Gui Len ---");
    foreach (var header in context.Request.Headers)
    {
        Console.WriteLine($"  {header.Key}: {header.Value}");
    }

    await next();

    Console.WriteLine("================ [GATEWAY RESPONSE OUTBOUND] ================");
    Console.WriteLine($"Time: {DateTime.Now:HH:mm:ss.fff}");
    Console.WriteLine($"Status Code Tra Ve Client: {context.Response.StatusCode}");
    Console.WriteLine("--- Headers Tra Ve ---");
    foreach (var header in context.Response.Headers)
    {
        Console.WriteLine($"  {header.Key}: {header.Value}");
    }
    Console.WriteLine("============================================================\n");
});

app.MapReverseProxy();

app.Run();