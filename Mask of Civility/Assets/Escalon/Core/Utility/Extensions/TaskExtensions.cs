using System;
using System.Threading;
using System.Threading.Tasks;

namespace Escalon
{
    public static class TaskExtensions
    {
        /// <summary>
        /// REF: https://stackoverflow.com/questions/4238345/asynchronously-wait-for-taskt-to-complete-with-timeout
        /// </summary>
        public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            var taskCancellationTokenSource = new CancellationTokenSource();

            using (var timeoutCancellationTokenSource = new CancellationTokenSource())
            {

                var completedTask =
                    await Task.WhenAny(Task.Run(async () => await task, taskCancellationTokenSource.Token),
                        Task.Delay(timeout, timeoutCancellationTokenSource.Token));
                if (completedTask == task)
                {
                    timeoutCancellationTokenSource.Cancel();
                    return await task;
                }
                else
                {
                    taskCancellationTokenSource.Cancel();
                    throw new TimeoutException("The operation has timed out.");
                }
            }
        }
    }
}