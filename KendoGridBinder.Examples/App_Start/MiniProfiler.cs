[assembly: WebActivator.PreApplicationStartMethod(typeof(KendoGridBinder.Examples.App_Start.MiniProfilerPackage), "PreStart")]
[assembly: WebActivator.PostApplicationStartMethod(typeof(KendoGridBinder.Examples.App_Start.MiniProfilerPackage), "PostStart")]

namespace KendoGridBinder.Examples.App_Start 
{
    using System.Web;
    using System.Web.Mvc;
    using System.Linq;
    using MvcMiniProfiler;
    using MvcMiniProfiler.MVCHelpers;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    public static class MiniProfilerPackage
    {
        public static void PreStart()
        {
            MiniProfiler.Settings.SqlFormatter = new MvcMiniProfiler.SqlFormatters.SqlServerFormatter();
			MiniProfilerEF.Initialize();
            DynamicModuleUtility.RegisterModule(typeof(MiniProfilerStartupModule));
            GlobalFilters.Filters.Add(new ProfilingActionFilter());
        }

        public static void PostStart()
        {
            var copy = ViewEngines.Engines.ToList();
            ViewEngines.Engines.Clear();
            foreach (var item in copy)
            {
                ViewEngines.Engines.Add(new ProfilingViewEngine(item));
            }
        }
    }

    public class MiniProfilerStartupModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) => MiniProfiler.Start();
            context.EndRequest += (sender, e) => MiniProfiler.Stop();
        }

        public void Dispose() { }
    }
}

