using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rappers.HipHop.Services.Implementations;
using Rappers.HipHop.Services.Implementations.S3;

namespace Rappers.Tests.HipHop
{
    [TestFixture]
    public class S3LogParserTests
    {
        [Test]
        public void CanFindDate()
        {
            var lines = File.ReadAllLines(@"Resources\s3log.txt");
            var line = lines.ElementAt(10);

            var parser = new S3LogParser();
            Assert.AreEqual(DateTime.Parse("08/Aug/2012 17:59:48 +0000"), parser.FindDate(line));
        }

        [Test]
        public void CanFindUrl()
        {
            var lines = File.ReadAllLines(@"Resources\s3log.txt");
            var line = lines.ElementAt(10);

            var parser = new S3LogParser();
            Assert.AreEqual("/videos-moshcam/moshcam/1692k/teganandsara_20090108_e_moshcam_1692k_480p.mp4", parser.FindUrl(line));
        }

        [Test]
        public void CanFindHttpStatusCode()
        {
            var lines = File.ReadAllLines(@"Resources\s3log.txt");
            var line = lines.ElementAt(10);

            var parser = new S3LogParser();
            Assert.AreEqual(206, parser.FindHttpCode(line));
        }
    }
}
