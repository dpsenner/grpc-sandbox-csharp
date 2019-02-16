using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcSandbox.Server.GrpcServices
{
    public interface IGrpcChannelOptions
    {
        int MaxReceiveMessageLength { get; }
    }
}
