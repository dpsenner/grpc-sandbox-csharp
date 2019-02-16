using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;
using Extensions.CommandLineUtils;

namespace GrpcSandbox.Server.ConsoleCommands.RunCommand
{
    public class Command : ICommandConfigurator
    {
        public void Configure(CommandLineApplication command)
        {
            var options = new CommandOptions()
            {
                Listen = command.Option("--listen", "Example: 0.0.0.0:50051", CommandOptionType.MultipleValue),
            };
            command.OnExecuteWithCancellation(token => new CommandHandler(options).RunAsync(token));
        }
    }
}
