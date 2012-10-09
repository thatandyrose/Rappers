using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Rappers.HipHop.Services.Implementations.FTP;

namespace Rappers.Tests.HipHop
{
    [TestFixture]
    public class FtpStorageServiceTests
    {
        [Test]
        [Ignore]
        public void CanUploadFile()
        {
            var ftp = new FtpStorageService("ftpuploader.app.viostream.com", "viocorp\andrew.rose", "AR12Viocorp");

            ftp.UploadFile(new FileInfo(@"resources\test_01.mp4"));

        }
    }
}
