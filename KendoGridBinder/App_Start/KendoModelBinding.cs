[assembly: WebActivator.PreApplicationStartMethod(typeof(KendoGridBinder.App_Start.KendoModelBinding), "Start")]

namespace KendoGridBinder.App_Start
{
    using System.Web.Mvc;

    internal class KendoModelBinding
    {
        public static void Start()
        {
            ModelBinders.Binders[typeof(KendoGridRequest)] = new KendoGridModelBinder();
        }
    }
}
