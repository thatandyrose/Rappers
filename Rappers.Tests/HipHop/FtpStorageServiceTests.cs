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
            var ftp = new FtpStorageService("d3.viocorp.com", "technical.team", "LumpyDe$ign30");

            ftp.UploadFile(new FileInfo(@"resources\test_01.mp4"),(done)=>
            {
                Console.Write("\r{0}%   ", done);                                                      
            });

        }
    }
}
