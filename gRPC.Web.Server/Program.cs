using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

// builder.WebHost.ConfigureKestrel(options =>
// {
//     // Setup a HTTP/2 endpoint without TLS.
//     options.ListenLocalhost(7264, o => o.Protocols = HttpProtocols.Http2);
// });

var app = builder.Build();

app.UseCors("AllowOrigin");
app.UseGrpcWeb();

app.MapGrpcService<StreamImplService>().EnableGrpcWeb();

app.Run();
