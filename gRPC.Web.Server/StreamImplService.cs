
using Grpc.Core;

public class StreamImplService: StreamService.StreamServiceBase {
    
    private readonly List<string> _messages = new List<string>()
    {
      "Hello",
      "World",
      "!"
    };
    
    public override async Task FetchResponse(
        Request request, 
        IServerStreamWriter<Response> responseStream, 
        ServerCallContext context)
    {
        while (!context.CancellationToken.IsCancellationRequested)
        {
            foreach (var message in _messages)
            {
                await responseStream.WriteAsync(new Response()
                {
                    Result = message
                });

                Thread.Sleep(750);
            }
        }
    }
}