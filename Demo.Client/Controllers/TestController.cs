using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orleans;
using Orleans.Client;

namespace Demo.Client.Controllers
{
    public class TestController : Controller
    {
        //public IGoodsService BaseService
        //{
        //    get
        //    {
        //        //return null;
        //        return OrleansClient.ClusterClientProxy<IGoodsService>().GetGrain<IGoodsService>();
        //    }
        //}



        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
    }
}