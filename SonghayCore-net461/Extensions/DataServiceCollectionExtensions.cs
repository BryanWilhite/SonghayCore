using System.Collections.ObjectModel;
using System.Data.Services.Client;
using System.Linq;

namespace Songhay.Extensions
{
    /// <summary>
    /// Extensions of <see cref="System.Data.Services.Client.DataServiceCollection&lt;T&gt;"/>.
    /// </summary>
    public static partial class DataServiceCollectionExtensions
    {
        /// <summary>
        /// Converts the specified OData collection into an instance
        /// of <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;"/>.
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
