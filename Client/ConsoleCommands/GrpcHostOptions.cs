using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcSandbox.Client.ConsoleCommands
{
    public class GrpcHostOptions
    {
        public CommandOption Host { get; set; }

        public CommandOption Port { get; set; }
    }
}
