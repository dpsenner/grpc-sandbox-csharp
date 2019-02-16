using Microsoft.Extensions.CommandLineUtils;
using System;
using Extensions.CommandLineUtils;

namespace GrpcSandbox.Client
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                return new CommandLineApplication()
                {
                    Name = nameof(Client),
                }
                .AddCommand<ConsoleCommands.Streaming.Command>("stream-bytes")
                .OnExecuteShowHelp()
                .Execute(args);
            }
            catch (CommandParsingException ex)
            {
                Console.WriteLine(ex.Message);
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
