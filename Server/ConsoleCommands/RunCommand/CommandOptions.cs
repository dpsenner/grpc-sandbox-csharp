using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcSandbox.Server.ConsoleCommands.RunCommand
{
    public class CommandOptions
    {
        public CommandOption Listen { get; set; }
    }
}
