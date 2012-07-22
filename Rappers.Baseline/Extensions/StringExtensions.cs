using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rappers.Baseline.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }
        public static string Urlify(this string source)
        {
            var invalidChars = new List<char>();
            source.ToList().ForEach(c=>
            {
                 if(HttpUtility.UrlEncode(c.ToString()).Contains("%"))
                 {
                     invalidChars.Add(c);
                 }                                                  
            });
            invalidChars.ForEach(c=>
            {
                source = source.Replace(c.ToString(), "");
            });
            return HttpUtility.UrlEncode(source);
        }
    }
}
