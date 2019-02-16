using Grpc.Core;
using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Extensions.Threading;

namespace GrpcSandbox.Client.ConsoleCommands
{
    public static class GrpcHostOptionsExtensions
    {
        public static GrpcHostOptions ConfigureGrpcHostOptions(this CommandLineApplication application)
        {
            return new GrpcHostOptions()
            {
                Host = application.Option("--grpc-host", "The grpc host to connect to; defaults to localhost.", CommandOptionType.SingleValue),
                Port = application.Option("--grpc-port", "The grpc port to connect to; defaults to 50051.", CommandOptionType.SingleValue),
            };
        }

        public static async Task<Channel> ConnectAsync(this GrpcHostOptions options, CancellationToken token)
        {
            string host = "localhost";
            if (options.Host.HasValue())
            {
                host = options.Host.Value();
            }

            int port = 50051;
            if (options.Port.HasValue())
            {
                // try to parse the value
                if (!int.TryParse(options.Port.Value(), out port))
                {
                    throw new ArgumentException("Port not in range of [1..65535]");
                }
                else if (port < 1 || port > ushort.MaxValue)
                {
                    throw new ArgumentException("Port not in range of [1..65535]");
                }
            }

            var channel = new Channel(host, port, ChannelCredentials.Insecure, new[]
            {
                new ChannelOption(ChannelOptions.MaxReceiveMessageLength, int.MaxValue),
            });

            Console.WriteLine($"Connecting to grpc host {channel.Target} ..");
            var connectTask = channel
                .ConnectAsync();
            var cancellationTask = token
                .AwaitCancellationAsync();
            var completedTask = await Task
                .WhenAny(connectTask, cancellationTask)
                .ConfigureAwait(false);
            if (completedTask == cancellationTask)
            {
                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                else
                {
                    // this is a bug
                    throw new InvalidOperationException();
                }
            }

            // await the connect task
            try
            {
                await connectTask;

                Console.WriteLine($"Connection established to grpc host {channel.Target}");
                return channel;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not connect to grpc host {channel.Target}", ex);
            }
        }

    }
}
