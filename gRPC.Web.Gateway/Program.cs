var builder = WebApplication.CreateBuilder(args);


// Add the reverse proxy to capability to the server
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// Enable endpoint routing, required for the reverse proxy
app.UseRouting();

// Register the reverse proxy routes
app.MapReverseProxy();

app.Run();