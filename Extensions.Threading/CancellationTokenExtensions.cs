using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Extensions.Threading
{
    public static class CancellationTokenExtensions
    {
        public static Task AwaitCancellationAsync(this CancellationToken token)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            token.Register(() =>
            {
                taskCompletionSource.TrySetResult(true);
            });
            return taskCompletionSource.Task;
        }
    }
}
