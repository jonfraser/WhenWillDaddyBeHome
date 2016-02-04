using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WhenWillDaddyBeHome.Hubs;
using WhenWillDaddyBeHome.Models;
using System.Web.Caching;

namespace WhenWillDaddyBeHome.Controllers
{
    public class AController : Controller
    {
        [HttpGet]
        public ActionResult GetNewToken()
        {
            //return a unique token that is meaningless unless someone is listening on it
            return Json(Guid.NewGuid(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void UpdateMyLocation(Location value)
        {
            if(HttpContext.Cache[value.Id.ToString()] != null)
            {
                HttpContext.Cache.Remove(value.Id.ToString());
            }
			Request.RequestContext.HttpContext.Cache.Add(value.Id.ToString(), value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<LocationHub>();
            hubContext.Clients.All.locationMessageReceived(value.Id, 
                value.Lat, 
                value.Long, 
                DateTime.UtcNow.Subtract(new DateTime(1970,1,1)).TotalMilliseconds);
        }
    }
}