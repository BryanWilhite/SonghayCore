#if NET452 || NET462

using System.ServiceModel.Syndication;

namespace Songhay.Models
{
    /// <summary>
    /// Specifies that a Model supports syndication.
    /// </summary>
    public interface ISyndicatable
    {
        /// <summary>
        /// Gets the syndication feed.
        /// </summary>
        SyndicationFeed GetSyndicationFeed();

        /// <summary>
        /// Gets the syndication item.
        /// </summary>
        SyndicationItem GetSyndicationItem();
    }
}

#endif
