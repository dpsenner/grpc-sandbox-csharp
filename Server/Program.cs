using GrpcSandbox.Server.Hosting;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using GrpcSandbox.Server.ConsoleCommands;
using Extensions.CommandLineUtils;

namespace GrpcSandbox.Server
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                return new CommandLineApplication
                {
                    Name = nameof(Server),
                }
                .AddCommand<ConsoleCommands.RunCommand.Command>("run")
                .OnExecuteShowHelp()
                .Execute(args);
            }
            catch (CommandParsingException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine();
                ex.Command.ShowHelp();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }
    }
}
