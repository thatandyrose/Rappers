using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Rappers.HipHop.Services.Implementations
{
    public class ParsedLog
    {
        public DateTime Time { get; set; }
    }
    public class S3LogParser
    {
        public List<ParsedLog> Parse(FileInfo logFile)
        {
            var logs = new List<ParsedLog>();
            var lines = File.ReadAllLines(logFile.FullName);
            foreach (var line in lines)
            {
                var log = new ParsedLog()
                {
                    Time = ParseDate(line),
                };
            }

            return logs;
        }

        public DateTime ParseDate(string line)
        {
            string raw = Regex.Match(line, @"\[[^\]\[]+\]").Value.Replace("[","").Replace("]","");

            string[] parts = raw.Split(':');
            string date = parts[0];
            string time = string.Join(":", parts.Skip(1));
            string datetime = string.Format("{0} {1}", date, time);
            return DateTime.Parse(datetime);
        }
    }
}
