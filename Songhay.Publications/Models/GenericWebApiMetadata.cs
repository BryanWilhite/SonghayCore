using Songhay.Models;
using System.Collections.Generic;

namespace Songhay.Publications.Models
{
    public class GenericWebApiMetadata
    {
        public RestApiMetadata ApiMetadata { get; set; }

        public Dictionary<string, string> ClaimsSet { get; set; }
    }
}
