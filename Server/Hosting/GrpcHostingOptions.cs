using Grpc.Core;
using GrpcSandbox.Server.GrpcServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcSandbox.Server.Hosting
{
    public class GrpcHostingOptions : IGrpcChannelOptions
    {
        public List<ServerPort> ServerPorts { get; set; } = new List<ServerPort>();

        public int MaxReceiveMessageLength { get; set; }
    }
}
