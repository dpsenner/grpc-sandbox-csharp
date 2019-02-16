using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GrpcSandbox.Api.Streaming;
using Extensions.Threading;

namespace GrpcSandbox.Client.ConsoleCommands.StreamBytes
{
    public class CommandHandler : IAsyncCommandHandler
    {
        protected CommandOptions Options { get; }

        public CommandHandler(CommandOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task RunAsync(CancellationToken token)
        {
            int chunkSize = 1024 * 1024;
            if (Options.ChunkSize.HasValue())
            {
                chunkSize = Convert.ToInt32(Options.ChunkSize.Value());
            }

            long totalChunks = 1000;
            if (Options.TotalChunks.HasValue())
            {
                totalChunks = Convert.ToInt64(Options.TotalChunks.Value());
            }

            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var connectTask = Options
                    .GrpcHost
                    .ConnectAsync(cancellationTokenSource.Token);
                var cancellationTask = token.AwaitCancellationAsync();
                var completedTask = await Task.WhenAny(connectTask, cancellationTask).ConfigureAwait(false);
                if (completedTask == cancellationTask)
                {
                    // operation cancelled
                    return;
                }

                var channel = await connectTask.ConfigureAwait(false);
                var streamingService = new StreamingService.StreamingServiceClient(channel);
                var streamBytesCall = streamingService.StreamBytes(new StreamBytesRequest()
                {
                    ChunkSize = chunkSize,
                    TotalChunks = totalChunks,
                }, default);

                var stopwatch = Stopwatch.StartNew();
                long totalBytes = 0;
                try
                {
                    await streamBytesCall
                         .SelectAsync(streamBytesResponse =>
                         {
                             totalBytes += streamBytesResponse.Chunk.Length;
                         }, token)
                         .ConfigureAwait(false);
                }
                catch (Exception)
                {
                    if (token.IsCancellationRequested)
                    {
                        // cancelled by user
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    Console.WriteLine($"Transferred {totalBytes} bytes in {stopwatch.Elapsed.TotalMilliseconds}ms ({(totalBytes / (1024 * 1024) / stopwatch.Elapsed.TotalSeconds)} MBytes/second)");
                }
            }
        }
    }
}
