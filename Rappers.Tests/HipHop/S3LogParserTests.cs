using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rappers.HipHop.Services.Implementations;

namespace Rappers.Tests.HipHop
{
    [TestFixture]
    public class S3LogParserTests
    {
        [Test]
        public void CanParseDate()
        {
            var lines = File.ReadAllLines(@"c:\temp\s3logs\access_log-2012-08-08-19-16-10-62DFBE9829618C50");
            var line = lines.ElementAt(10);

            var parser = new S3LogParser();
            Assert.DoesNotThrow(() => parser.ParseDate(line));
        }
    }
}
