using Grpc.Core;
using GrpcSandbox.Server.GrpcServices;
using GrpcSandbox.Server.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Extensions.Threading;

namespace GrpcSandbox.Server.ConsoleCommands.RunCommand
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
            GrpcHostingService hostingService = new ServiceCollection()
                .AddLogging()
                .AddSingleton(serviceProvider => new GrpcHostingOptions()
                {
                    ServerPorts = Options
                            .Listen
                            .Values
                            .Select(listen =>
                            {
                                string[] listenSplit = listen.Split(':');
                                string host = listenSplit[0];
                                int port = Convert.ToInt32(listenSplit[1]);
                                return new ServerPort(host, port, ServerCredentials.Insecure);
                            })
                            .ToList(),
                    MaxReceiveMessageLength = 1024 * 1024 * 4,
                })
                .AddTransient<IGrpcChannelOptions>(serviceProvider => serviceProvider.GetRequiredService<GrpcHostingOptions>())
                .AddAllImplementationsOfInterfaceInAssemblyAsSingleton<IGrpcService>(typeof(CommandHandler).Assembly)
                .AddTransient<GrpcHostingService>()
                .BuildServiceProvider()
                .GetRequiredService<GrpcHostingService>();

            // start
            await hostingService.StartAsync(default)
                .ConfigureAwait(false);

            // await shutdown
            await token
                .AwaitCancellationAsync()
                .ConfigureAwait(false);

            // stop
            await hostingService.StopAsync(default)
                .ConfigureAwait(false);
        }
    }
}
