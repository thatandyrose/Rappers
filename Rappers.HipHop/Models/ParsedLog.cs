using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rappers.HipHop.Models
{
    public class ParsedLog
    {
        public DateTime Time { get; set; }
        public string Url { get; set; }
        public string Method { get; set; }
        public string HttpResponseCode { get; set; }
        public long BytesSent { get; set; }
        public long BytesSize { get; set; }
    }
}
