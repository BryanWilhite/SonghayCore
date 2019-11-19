using System;

namespace Songhay.Publications.Models
{
    public interface IWebKeyword
    {
        int DocumentId { get; set; }
        string KeywordValue { get; set; }
    }
}
