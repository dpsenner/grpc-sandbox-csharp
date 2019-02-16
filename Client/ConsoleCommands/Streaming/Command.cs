using Microsoft.Extensions.CommandLineUtils;
using Extensions.CommandLineUtils;

namespace GrpcSandbox.Client.ConsoleCommands.Streaming
{
    public class Command : ICommandConfigurator
    {
        public void Configure(CommandLineApplication command)
        {
            var options = new CommandOptions()
            {
                GrpcHost = command.ConfigureGrpcHostOptions(),
                ChunkSize = command.Option("--chunk-size", "An integral value between 1 and max int; defaults to 1 MByte", CommandOptionType.SingleValue),
                TotalChunks = command.Option("--total-chunks", "An integral value between 1 and inf; defaults to 10000", CommandOptionType.SingleValue),
            };
            command.OnExecuteWithCancellation(token => new CommandHandler(options).RunAsync(token));
        }
    }
}
