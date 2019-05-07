#if NET452 || NET462

using System;
using System.ServiceModel;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="ICommunicationObject"/>.
    /// </summary>
    public static class ICommunicationObjectExtensions
    {
        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <remarks>
        /// For more detail see “IDisposable and WCF”
        /// by Steve Smith
        /// [http://stevesmithblog.com/blog/idisposable-and-wcf/]
        /// </remarks>
        public static void CloseConnection(this ICommunicationObject serviceClient)
        {
            if(serviceClient == null) return;
            if(serviceClient.State != CommunicationState.Opened) return;
            try
            {
                serviceClient.Close();
            }
            catch(CommunicationException)
            {
                serviceClient.Abort();
            }
            catch(TimeoutException)
            {
                serviceClient.Abort();
            }
            catch(Exception)
            {
                serviceClient.Abort();
                throw;
            }
        }

        /// <summary>
        /// Closes the connection or abort.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        public static void CloseConnectionOrAbort(this ICommunicationObject serviceClient)
        {
            serviceClient.CloseConnection();
            if(serviceClient == null) return;
            if((serviceClient.State != CommunicationState.Closed) || (serviceClient.State != CommunicationState.Closing))
            {
                serviceClient.Abort();
            }
        }

        /// <summary>
        /// Determines whether the specified service client is opened.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <returns>
        ///   <c>true</c> if the specified service client is opened; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOpened(this ICommunicationObject serviceClient)
        {
            if(serviceClient == null) return false;
            return (serviceClient.State == CommunicationState.Opened);
        }
    }
}

#endif
