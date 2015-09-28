using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebAPISuite.Models;

namespace WebAPISuite
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SetupRefreshJob();
        }

        private static void SetupRefreshJob()
        {
            //remove a previous job
            Action remove = HttpContext.Current.Cache["WebApiSuiteRefresh"] as Action;
            if (remove is Action)
            {
                HttpContext.Current.Cache.Remove("WebApiSuiteRefresh");
                remove.EndInvoke(null);
            }

            //get the worker
            Action work = () =>
            {
                while (true)
                {
                    Thread.Sleep(60000);
                    WebClient refresh = new WebClient();
                    try
                    {
                        refresh.UploadString("http://www.elementalstudios.co.uk/Home/Contact", string.Empty);
                        refresh.DownloadString("http://elementalstudios.co.uk/api/SendEmailApi/GetClientSettings?clientName=Shreyas");
                    }
                    catch (Exception)
                    {
                        //snip...
                    }
                    finally
                    {
                        refresh.Dispose();
                    }
                }
            };
            work.BeginInvoke(null, null);

            //add this job to the cache
            HttpContext.Current.Cache.Add(
                "WebApiSuiteRefresh",
                work,
                null,
                Cache.NoAbsoluteExpiration,
                Cache.NoSlidingExpiration,
                CacheItemPriority.Normal,
                (s, o, r) => { SetupRefreshJob(); }
                );
        }
    }
}
