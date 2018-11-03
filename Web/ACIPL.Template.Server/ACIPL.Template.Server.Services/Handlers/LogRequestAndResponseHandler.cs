using ACIPL.Template.Core.Logging;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ACIPL.Template.Server.Services.Handlers
{
    public class LogRequestAndResponseHandler : DelegatingHandler
    {
        private readonly Logger Logger = LoggerFactory.GetLogger();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // log request body
            string requestBody = await request.Content.ReadAsStringAsync();

            // IP Address
            var ipAddress = HttpContext.Current.Request.UserHostAddress;

            //log request information
            var requestInfo = string.Format("[{3}][Req:{0}][{1}:{2}]", request.Method, request.RequestUri.AbsolutePath, requestBody, ipAddress);
            Logger.Info(requestInfo);

            // let other handlers process the request
            var result = await base.SendAsync(request, cancellationToken);

            if (result.Content != null)
            {
                // once response body is ready, log it
                var responseBody = await result.Content.ReadAsStringAsync();
                var responseInfo = string.Format("[{3}][Res:{0}][{1}:{2}]", request.Method, request.RequestUri.AbsolutePath, responseBody, ipAddress);
                Logger.Info(responseInfo);
            }

            return result;
        }
    }
}