using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Rappers.HipHop.Models;

namespace Rappers.HipHop.Services.Implementations.S3
{
    public class S3LogParser : BaseLogParser, IParseLogs
    {
        public override List<ParsedLog> Parse(FileInfo logFile)
        {
            var logs = new List<ParsedLog>();
            var lines = File.ReadAllLines(logFile.FullName);
            foreach (var line in lines.Where(l => !string.IsNullOrEmpty(l) && l.Contains("REST.GET.OBJECT") && l.Contains("\"GET /")))
            {
                var log = new ParsedLog()
                {
                    Time = FindDate(line),
                    Url = FindUrl(line),
                    Method = "REST.GET.OBJECT",
                    BytesSent = FindBytesSent(line),
                    BytesSize = FindBytesSize(line),
                    HttpResponseCode = FindHttpCode(line)
                };
                logs.Add(log);
            }

            return logs;
        }

        public override string FindUrl(string line)
        {
            var i = line.IndexOf("\"GET", StringComparison.Ordinal);
            var fromGet = line.Substring(i);
            return fromGet.Split(' ')[1];
        }

        public override int FindHttpCode(string line)
        {
            var i = line.IndexOf("\"GET", StringComparison.Ordinal);
            var fromGet = line.Substring(i);
            return int.Parse(fromGet.Split(' ')[3]);
        }

        public override long FindBytesSent(string line)
        {
            var i = line.IndexOf("\"GET", StringComparison.Ordinal);
            var fromGet = line.Substring(i);
            string sent = fromGet.Split(' ')[5];
            if(sent.Contains("-"))
            {
                return 0;
            }
            return long.Parse(sent);
        }

        public override long FindBytesSize(string line)
        {
            var i = line.IndexOf("\"GET", StringComparison.Ordinal);
            var fromGet = line.Substring(i);
            string sent = fromGet.Split(' ')[6];
            if (sent.Contains("-"))
            {
                return 0;
            }
            return long.Parse(sent);
        }

        public override DateTime FindDate(string line)
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
