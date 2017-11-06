
namespace Songhay.Models
{
    /// <summary>
    /// Defines the meta-data of an Application resource.
    /// </summary>
    /// <typeparam name="TResource">The type of the resource.</typeparam>
    public class PackedResource<TResource> : PackedResource
    {
        /// <summary>
        /// Gets or sets the XAML string.
        /// </summary>
        /// <value>The XAML string.</value>
        public string XamlString { get; set; }

        /// <summary>
        /// Gets or sets the XAML object.
        /// </summary>
        /// <value>The XAML object.</value>
        public TResource XamlObject { get; set; }
    }
}
