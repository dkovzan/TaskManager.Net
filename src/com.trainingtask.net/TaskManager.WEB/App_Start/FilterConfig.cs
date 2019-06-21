using System.Web.Mvc;

namespace TaskManager.WEB
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Remove(new HandleErrorAttribute());
        }
    }
}
