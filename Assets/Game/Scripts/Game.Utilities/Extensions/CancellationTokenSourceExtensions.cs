using System;
using System.Threading;

namespace Game.Utilities
{
    public static class CancellationTokenSourceExtensions
    {
        public static void CancelAndDisposeSafe(this CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null)
            {
                return;
            }

            try
            {
                cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }
            catch (ObjectDisposedException)
            {
            }
        }
    }
}