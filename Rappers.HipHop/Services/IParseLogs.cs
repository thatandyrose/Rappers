using System;
using System.Collections.Generic;
using System.IO;
using Rappers.HipHop.Models;

namespace Rappers.HipHop.Services
{
    public interface IParseLogs
    {
        List<ParsedLog> Parse(DirectoryInfo directoryInfo, string filePrefix);
        List<ParsedLog> Parse(FileInfo logFile);
        string FindUrl(string line);
        DateTime FindDate(string line);
        int FindHttpCode(string line);
    }
}