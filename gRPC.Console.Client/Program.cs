
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using gRPC.Console.Client.Handlers;
using static StreamService;


// Define service URL, it may come from configuration.
string baseUrl = "https://localhost:7172";
string gatewayPrefix = "/grpc/grpc-web-service";


AsyncServerStreamingCall<Response> _call;

StartGrpcStream();
Thread.Sleep(TimeSpan.FromSeconds(15));


async void StartGrpcStream() {

    // Creating a SubDirectoryHandler instance to handle gateway.
    var handler = new SubdirectoryHandler(new HttpClientHandler(), gatewayPrefix);


    /* This switch must be set before creating the GrpcChannel/HttpClient.
     * This switch is only required when you want to use http instead of https (may happen on MAC devices)
     * The switch is only required for .NET Core 3.x. It does nothing in .NET 5 and isn't required.
     */
    // AppContext.SetSwitch(
    //              "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);


    // Creating channel and setting GrpcWebHandler as HttpHandler
    var channel = GrpcChannel.ForAddress(baseUrl, new GrpcChannelOptions
    {
        HttpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, handler),
    });

    Metadata metadata = new()
    {
        { "authorization", "Bearer " + "tokenString" },
    };


    var client = new StreamServiceClient(channel);

    // You can set a deadline for your call by deadLine parameter.
    _call = client.FetchResponse(new Request() { Id= 1 },
                                                 metadata,
                                                 deadline: DateTime.UtcNow.AddSeconds(10));

    _ = Task.Run(async () =>
    {
        while (await _call.ResponseStream.MoveNext())
        {
            Console.WriteLine(_call.ResponseStream.Current);
        }
    });
}
