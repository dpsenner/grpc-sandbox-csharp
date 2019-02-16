using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;

namespace GrpcSandbox.Client.ConsoleCommands.StreamBytes
{
    public class CommandOptions
    {
        public GrpcHostOptions GrpcHost { get; set; }

        public CommandOption ChunkSize { get; set; }

        public CommandOption TotalChunks { get; set; }
    }
}
