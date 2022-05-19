// package: 
// file: stream.proto

var stream_pb = require("./stream_pb");
var grpc = require("@improbable-eng/grpc-web").grpc;

var StreamService = (function () {
  function StreamService() {}
  StreamService.serviceName = "StreamService";
  return StreamService;
}());

StreamService.FetchResponse = {
  methodName: "FetchResponse",
  service: StreamService,
  requestStream: false,
  responseStream: true,
  requestType: stream_pb.Request,
  responseType: stream_pb.Response
};

exports.StreamService = StreamService;

function StreamServiceClient(serviceHost, options) {
  this.serviceHost = serviceHost;
  this.options = options || {};
}

StreamServiceClient.prototype.fetchResponse = function fetchResponse(requestMessage, metadata) {
  var listeners = {
    data: [],
    end: [],
    status: []
  };
  var client = grpc.invoke(StreamService.FetchResponse, {
    request: requestMessage,
    host: this.serviceHost,
    metadata: metadata,
    transport: this.options.transport,
    debug: this.options.debug,
    onMessage: function (responseMessage) {
      listeners.data.forEach(function (handler) {
        handler(responseMessage);
      });
    },
    onEnd: function (status, statusMessage, trailers) {
      listeners.status.forEach(function (handler) {
        handler({ code: status, details: statusMessage, metadata: trailers });
      });
      listeners.end.forEach(function (handler) {
        handler({ code: status, details: statusMessage, metadata: trailers });
      });
      listeners = null;
    }
  });
  return {
    on: function (type, handler) {
      listeners[type].push(handler);
      return this;
    },
    cancel: function () {
      listeners = null;
      client.close();
    }
  };
};

exports.StreamServiceClient = StreamServiceClient;

