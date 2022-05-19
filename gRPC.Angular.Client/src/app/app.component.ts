import { Component } from '@angular/core';
import { grpc } from '@improbable-eng/grpc-web';
import { Request } from '@improbable-eng/grpc-web/dist/typings/invoke';
import { Request as RpcRequest, Response as RpcResponse } from './generated/stream_pb';
import { StreamService } from './generated/stream_pb_service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'gRPC.Angular.Client';

  grpcClient!: Request;

  messages: string[] = [];


  startStream() {
    const request = new RpcRequest();
    request.setId(1);

    this.grpcClient = grpc.invoke(StreamService.FetchResponse, {
      request: request,
      host: `https://localhost:7172/grpc/grpc-web-service`,
      onMessage: (message: RpcResponse) => {
        // This section works when server writes something to stream.

        const data = message.toObject();
        this.messages.push(data.result);
      },
      onEnd: (code: grpc.Code, msg: string | undefined, trailers: grpc.Metadata) => {
        // This section works when server close connection.

        if (code == grpc.Code.OK) {
          console.log('request finished wihtout any error');
        } else {
          console.log('an error occured', code, msg, trailers);
        }

      },
    });
  }

  stopStream() {
    this.grpcClient.close();
  }
}
