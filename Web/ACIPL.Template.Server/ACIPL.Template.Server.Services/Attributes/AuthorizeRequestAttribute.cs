using ACIPL.Template.Core.Logging;
using ACIPL.Template.Core.Utilities;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ACIPL.Template.Server.Services.Attributes
{
    public class AuthorizeRequestAttribute : ActionFilterAttribute
    {
        private readonly Logger Logger = LoggerFactory.GetLogger();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var message = new StringBuilder();
            message.Append("Headers:");

            foreach (var item in actionContext.Request.Headers)
            {
                message.Append(string.Format("{0}|{1}", item.Key, item.Value.First().ToString()));
            }

            Logger.Info(message.ToString());

            if (actionContext.ActionDescriptor.GetCustomAttributes<SkipMyGlobalActionFilterAttribute>().Any())
            {
                Logger.Debug("SkipMyGlobalActionFilterAttribute found.");
                return;
            }

            if (!actionContext.Request.Headers.Contains("ACIPLToken"))
            {
                Logger.Error("ACIPL Token is not present in request.");
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "No token is associated with request.");
                return;
            }

            if (!GetToken(actionContext.Request.Headers.GetValues("ACIPLToken").First().ToString()))
            {
                Logger.Error("ACIPL Token data is not found.");
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Invalid Token");
            }
        }

        private bool GetToken(string token)
        {
            var obj = SessionManager.Get(token);
            return obj != null;
        }
    }
}