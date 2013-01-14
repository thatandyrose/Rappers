using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rappers.HipHop.Services;
using Rappers.HipHop.Services.Implementations.S3;

namespace Rappers.Frontend.Controllers
{
    public class HipHopController : Controller
    {
        private readonly IParseLogs _logParser;
        public HipHopController()
        {
            _logParser = new S3LogParser();
        }

        public ActionResult S3Logs()
        {
           // _logParser.
            return View();
        }
        
    }
}
