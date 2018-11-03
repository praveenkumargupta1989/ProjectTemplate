using ACIPL.Template.Core.Logging;
using System.Web.Http;

namespace ACIPL.Template.Server.Services.Controllers
{
    public class BaseApiController : ApiController
    {
        public ILogger Logger { get; set; }

        public BaseApiController()
        {
            Logger = LoggerFactory.GetLogger();
        }
    }
}