#if NET452 || NET462

using System.Collections.ObjectModel;
using System.Data.Services.Client;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="DataServiceCollection{T}"/>.
    /// </summary>
    public static partial class DataServiceCollectionExtensions
    {
        /// <summary>
        /// Converts the specified OData collection into an instance
        /// of <see cref="ObservableCollection{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceCollection">The service collection.</param>
        public static ObservableCollection<T> ToObservableCollection<T>(this DataServiceCollection<T> serviceCollection)
            where T : class
        {
            if(serviceCollection == null) return null;
            var output = new ObservableCollection<T>(serviceCollection);
            return output;
        }
    }
}

#endif
