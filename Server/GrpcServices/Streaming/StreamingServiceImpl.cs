using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Grpc.Core;
using GrpcSandbox.Api.Streaming;

namespace GrpcSandbox.Server.GrpcServices.Streaming
{
    public class StreamingServiceImpl : StreamingService.StreamingServiceBase, IGrpcService
    {
        protected IGrpcChannelOptions GrpcChannelOptions { get; }

        public StreamingServiceImpl(IGrpcChannelOptions grpcChannelOptions)
        {
            GrpcChannelOptions = grpcChannelOptions ?? throw new ArgumentNullException(nameof(grpcChannelOptions));
        }

        public override async Task StreamBytes(StreamBytesRequest request, IServerStreamWriter<StreamBytesResponse> responseStream, ServerCallContext context)
        {
            if (request.ChunkSize < 1)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Chunk size must be greater or equal than 1"));
            }

            if (request.ChunkSize > GrpcChannelOptions.MaxReceiveMessageLength)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Chunk size must not exceed {GrpcChannelOptions.MaxReceiveMessageLength}"));
            }

            byte[] buffer = new byte[request.ChunkSize];
            var random = new Random();
            random.NextBytes(buffer);
            for (long i = 0; i < request.TotalChunks; i++)
            {
                await responseStream.WriteAsync(new StreamBytesResponse()
                {
                    Chunk = ByteString.CopyFrom(buffer),
                }).ConfigureAwait(false);
            }
        }

        public ServerServiceDefinition GetServerServiceDefinition()
        {
            return StreamingService.BindService(this);
        }
    }
}
