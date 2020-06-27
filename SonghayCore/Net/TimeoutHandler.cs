#if NETSTANDARD

using Songhay.Extensions;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Songhay.Net
{
    /// <summary>
    /// Defines timeout and cancellation support
    /// for <see cref="HttpClient"/>.
    /// </summary>
    /// <seealso cref="DelegatingHandler" />
    /// <remarks>
    /// 📖 see “Better timeout handling with HttpClient”
    /// by @thomaslevesque [ https://github.com/thomaslevesque ]
    /// [ https://thomaslevesque.com/2018/02/25/better-timeout-handling-with-httpclient/ ]
    /// 
    /// </remarks>
    public class TimeoutHandler : DelegatingHandler
    {
        /// <summary>
        /// Gets or sets the default timeout.
        /// </summary>
        /// <value>
        /// The default timeout.
        /// </value>
        public TimeSpan DefaultTimeout { get; set; } = TimeSpan.FromSeconds(100);

        /// <summary>
        /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
        /// </summary>
        /// <param name="request">The HTTP request message to send to the server.</param>
        /// <param name="cancellationToken">A cancellation token to cancel operation.</param>
        /// <returns>
        /// The task object representing the asynchronous operation.
        /// </returns>
        /// <exception cref="TimeoutException"></exception>
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var cts = GetCancellationTokenSource(request, cancellationToken))
            {
                try
                {
                    return await base.SendAsync(request, cts?.Token ?? cancellationToken);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
            }
        }

        private CancellationTokenSource GetCancellationTokenSource(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var timeout = request.GetTimeout() ?? DefaultTimeout;
            if (timeout == Timeout.InfiniteTimeSpan)
            {
                // No need to create a CTS if there's no timeout
                return null;
            }
            else
            {
                var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                cts.CancelAfter(timeout);
                return cts;
            }
        }
    }
}

#endif
