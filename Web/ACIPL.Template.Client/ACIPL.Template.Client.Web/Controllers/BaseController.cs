using ACIPL.Template.Core.Logging;
using System.Web.Mvc;

namespace ACIPL.Template.Client.Web.Controllers
{
    public class BaseController : Controller
    {
        public Logger Logger = LoggerFactory.GetLogger();
        public PartialViewResult NoDataFound()
        {
            return PartialView();
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            //Log the error!!
            Logger.Fatal(filterContext.Exception);

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Index", "ErrorHandler");
            // OR 
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/ErrorHandler/Index.cshtml"
            };
        }
    }
}