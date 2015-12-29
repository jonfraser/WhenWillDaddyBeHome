using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WhenWillDaddyBeHome.Controllers
{
    public class Location
    {
        public Guid Id { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
    }

    public class IAmController : ApiController
    {
        // GET api/iam
        public Guid Get()
        {
            //return a unique token that is meaningless unless someone is listening on it
            return Guid.NewGuid();
        }

        // GET api/iam/guid
        public string Get(Guid id)
        {
            return "ok";
        }

        // POST api/iam
        public void Post([FromBody]Location value)
        {
            //TODO: signalr push a location message
        }
        
    }
}
