using ACIPL.Template.Client.Web.Handlers;
using ACIPL.Template.Core.Utilities;
using System.Web.Mvc;

namespace ACIPL.Template.Client.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IConfigurationManager configurationManager;
        private readonly IRestClientHandler restClientHandler;
        public HomeController(IConfigurationManager configurationManager,
            IRestClientHandler restClientHandler)
        {
            this.configurationManager = configurationManager;
            this.restClientHandler = restClientHandler;
        }
        public ActionResult Index()
        {


            Logger.Info("Index");
            return View();
        }
    }
}