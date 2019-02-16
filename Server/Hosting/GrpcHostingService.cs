using Grpc.Core;
using GrpcSandbox.Server.GrpcServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcSandbox.Server.Hosting
{
    public class GrpcHostingService : IHostedService
    {
        protected GrpcHostingOptions Options { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected ILogger Logger { get; }

        protected Grpc.Core.Server Server { get; }

        public GrpcHostingService(GrpcHostingOptions options, IServiceProvider serviceProvider, ILogger<GrpcHostingService> logger)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Server = new Grpc.Core.Server(new List<ChannelOption>()
            {
                new ChannelOption(ChannelOptions.MaxReceiveMessageLength, options.MaxReceiveMessageLength),
            });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // add endpoints to listen on
            foreach (var serverPort in Options.ServerPorts)
            {
                Server.Ports.Add(serverPort);
            }

            // add service implementations
            foreach (var grpcService in ServiceProvider.GetServices<IGrpcService>())
            {
                ServerServiceDefinition serviceDefinition = grpcService.GetServerServiceDefinition();
                Logger.LogInformation($"Adding service implementation {grpcService.GetType()}");
                Server.Services.Add(serviceDefinition);
            }

            // start server
            Logger.LogInformation("Starting grpc server ..");
            Server.Start();

            // dump the server ports
            foreach (var serverPort in Server.Ports)
            {
                if (serverPort.Credentials == ServerCredentials.Insecure)
                {
                    Logger.LogInformation($"Listening on {serverPort.Host}:{serverPort.BoundPort} (TLS disabled)");
                }
                else
                {
                    Logger.LogInformation($"Listening on {serverPort.Host}:{serverPort.BoundPort} (TLS enabled)");
                }
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation("Server is shutting down ..");

            // await the shutdown to happen
            await Server
                .ShutdownAsync()
                .ConfigureAwait(false);

            Logger.LogInformation("Server shutdown complete, bye!");
        }
    }
}
