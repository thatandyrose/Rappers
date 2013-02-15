using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rappers.Baseline.Extensions
{
    public static class DateTimeExtensions
    {
        public static double ToUnixTime(this DateTime source)
        {
            return source.ToUniversalTime()
               .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
               .TotalMilliseconds;
        }
    }
}
