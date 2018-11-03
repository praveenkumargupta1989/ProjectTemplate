using ACIPL.Template.Core.Logging;
using ACIPL.Template.Core.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ACIPL.Template.Server.Services.Attributes
{
    public class GlobalExceptionAttribute : ExceptionFilterAttribute
    {
        private readonly Logger Logger = LoggerFactory.GetLogger();
        private readonly IEmailManager EmailManager = EmailManagerFactory.GetEmailManager();

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            var exceptionType = actionExecutedContext.Exception.GetType();
            var ipAddress = HttpContext.Current.Request.UserHostAddress;

            if (exceptionType == typeof(ValidationException))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent(actionExecutedContext.Exception.Message), ReasonPhrase = "ValidationException", };
                var message = string.Format("{3}->{0}->{1}->{2}{5}{4}", actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.Name,
                actionExecutedContext.ActionContext.ActionDescriptor.ActionName, exceptionType, ipAddress, actionExecutedContext.Exception.ToString(), Environment.NewLine);

                Logger.Error(message);
                SendEmail(message);
                throw new HttpResponseException(resp);

            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                var message = string.Format("{3}->{0}->{1}->{2}{5}{4}", actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.Name,
                actionExecutedContext.ActionContext.ActionDescriptor.ActionName, exceptionType, ipAddress, actionExecutedContext.Exception.ToString(), Environment.NewLine);

                Logger.Error(message);
                SendEmail(message);
                throw new HttpResponseException(actionExecutedContext.Request.CreateResponse(HttpStatusCode.Unauthorized));
            }
            else
            {
                var message = string.Format("{3}->{0}->{1}->{2}{5}{4}", actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerType.Name,
                actionExecutedContext.ActionContext.ActionDescriptor.ActionName, exceptionType, ipAddress, actionExecutedContext.Exception.ToString(), Environment.NewLine);

                Logger.Fatal(message);
                SendEmail(message);
                throw new HttpResponseException(actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError));
            }
        }

        private void SendEmail(string message)
        {
            var email = new MailDto()
            {
                Subject = "Unhandle Exception From ACIPL.Template.Service",
                Body = message
            };
            EmailManager.Send(email);
        }
    }
}