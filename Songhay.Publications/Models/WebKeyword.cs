
namespace Songhay.Publications.Models
{
    public partial class WebKeyword : IWebKeyword
    {
        public int DocumentId { get; set; }
        public string KeywordValue { get; set; }
        public virtual Document Document { get; set; }
    }
}
