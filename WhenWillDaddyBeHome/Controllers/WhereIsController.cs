using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WhenWillDaddyBeHome.Controllers
{
    public class WhereIsController : Controller
    {
        public ActionResult Daddy(Guid? id)
        {
            return View(id);
        }

        public ActionResult Here(Guid id)
        {
            return View(id);
        }
    }
}
