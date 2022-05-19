using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);


// builder.WebHost.ConfigureKestrel(options =>
// {
//     // Setup a HTTP/2 endpoint without TLS.
//     options.ListenLocalhost(7273, o => o.Protocols = HttpProtocols.Http2);
// });

// Add the reverse proxy to capability to the server
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Enable endpoint routing, required for the reverse proxy
app.UseRouting();

// Register the reverse proxy routes
app.MapReverseProxy();

app.Run();