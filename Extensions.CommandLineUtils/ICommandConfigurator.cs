using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.CommandLineUtils
{
    public interface ICommandConfigurator
    {
        void Configure(CommandLineApplication command);
    }
}
