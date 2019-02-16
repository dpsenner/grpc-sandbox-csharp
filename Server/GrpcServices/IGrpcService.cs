using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcSandbox.Server.GrpcServices
{
    public interface IGrpcService
    {
        ServerServiceDefinition GetServerServiceDefinition();
    }
}
