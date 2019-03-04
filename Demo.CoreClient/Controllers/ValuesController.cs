using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Orleans.ClientCore;

namespace Demo.CoreClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IClusterClient _OrleansClusterClientProxy;

        //public IBaseService BaseServiceProvider
        //{
        //    get
        //    {
        //        return this._OrleansClusterClientProxy.GetGrain<IBaseService>();
        //    }
        //}


        public ValuesController(OrleansClusterProxy orleansClientProvider)
        {
            //this._OrleansClusterClientProxy = orleansClientProvider.GetClusterClient<IBaseService>();
        }


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
