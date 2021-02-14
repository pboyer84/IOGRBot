using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOGRBot
{
    public class IOGRFetcherConfiguration
    {
        public string InputJsonFile { get; set; }
        public string IogrApiGenerateEndpoint { get; set; }
        public string IogrAppPermalinkBaseUrl { get; set; }
    }
}
