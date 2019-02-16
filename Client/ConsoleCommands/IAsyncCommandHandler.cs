using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GrpcSandbox.Client.ConsoleCommands
{
    public interface IAsyncCommandHandler
    {
        Task RunAsync(CancellationToken token);
    }
}
