using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhenWillDaddyBeHome.Models;

namespace WhenWillDaddyBeHome.Controllers
{
    public class WhereIsController : Controller
    {
        public ActionResult Daddy(Guid? id)
        {
			var cachedToken = Request.RequestContext.HttpContext.Cache[id.GetValueOrDefault().ToString()] as Location;
			return View(cachedToken);
        }

        public ActionResult Here(Guid id)
        {
            return View(id);
        }
    }
}
