using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcSandbox.Client.ConsoleCommands
{
    public static class AsyncServerStreamingCallExtensions
    {
        public static async Task SelectAsync<T>(this AsyncServerStreamingCall<T> streamingCall, Action<T> callback, CancellationToken token)
        {
            using (IAsyncStreamReader<T> responseStream = streamingCall.ResponseStream)
            {
                while (await responseStream
                    .MoveNext(token)
                    .ConfigureAwait(false))
                {
                    callback(responseStream.Current);
                }
            }
        }
    }
}
