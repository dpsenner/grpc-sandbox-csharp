using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Extensions.CommandLineUtils
{
    public static class CommandLineApplicationExtensions
    {
        public static CommandLineApplication WithDescription(this CommandLineApplication command, string description)
        {
            command.Description = description;
            return command;
        }

        public static CommandLineApplication AddCommand<TCommandConfigurator>(this CommandLineApplication application, string name)
            where TCommandConfigurator : ICommandConfigurator, new()
        {
            application.Command(name, command => new TCommandConfigurator().Configure(command));
            return application;
        }

        public static CommandLineApplication OnExecuteShowHelp(this CommandLineApplication command)
        {
            command.OnExecute(() =>
            {
                command.ShowHelp();
                return 0;
            });
            return command;
        }

        public static CommandLineApplication OnExecuteWithCancellation(this CommandLineApplication application, Func<CancellationToken, Task> func)
        {
            application.OnExecute(async () =>
            {
                using (var cancellationTokenSource = CreateCancellationTokenSourceThatIsCancelledOnCancelKeyPress())
                {
                    await func(cancellationTokenSource.Token).ConfigureAwait(false);
                }

                return 0;
            });

            return application;
        }

        private static CancellationTokenSource CreateCancellationTokenSourceThatIsCancelledOnCancelKeyPress()
        {
            if (Environment.UserInteractive)
            {
                Console.WriteLine("Press CTRL+C to cancel.");
            }

            var cancellationTokenSource = new CancellationTokenSource();

            // attach to cancel key press and set the result on the task
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                // try to set the result
                cancellationTokenSource.Cancel();

                // do not kill the process
                e.Cancel = true;
            };

            return cancellationTokenSource;
        }
    }
}
