using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Rappers.HipHop.Models;

namespace Rappers.HipHop.Services.Implementations
{
    public abstract class BaseLogParser : IParseLogs
    {
        public List<ParsedLog> Parse(DirectoryInfo directoryInfo, string filePrefix)
        {
            var logs = new List<ParsedLog>();
            ParseAction(directoryInfo, filePrefix, (l) =>
            {
                logs.Add(l);
            });

            return logs;
        }

        public void ParseAction(DirectoryInfo directoryInfo, string filePrefix, Action<ParsedLog> action)
        {
            directoryInfo.GetFiles().Where(f => f.Name.StartsWith(filePrefix))
            .ToList()
            .ForEach(f =>
            {
                Parse(f).ForEach(l => action(l));
            });
        }

        public abstract List<ParsedLog> Parse(FileInfo logFile);
        public abstract string FindUrl(string line);
        public abstract DateTime FindDate(string line);
        public abstract int FindHttpCode(string line);
        public abstract long FindBytesSent(string line);
        public abstract long FindBytesSize(string line);
    }
}
