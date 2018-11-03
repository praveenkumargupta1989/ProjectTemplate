using ACIPL.Template.Client.Web.Models;
using ACIPL.Template.Core.Logging;
using RestSharp;
using System;

namespace ACIPL.Template.Client.Web.Handlers
{
    public interface IRestClientHandler
    {
        IRestResponse Get(RestApiRequest restApiRequest);
        IRestResponse Post(RestApiRequest restApiRequest);

    }
    public class RestClientHandler : IRestClientHandler
    {
        private Logger Logger = LoggerFactory.GetLogger();
        public IRestResponse Get(RestApiRequest restApiRequest)
        {
            RestClient restClient = new RestClient();
            restClient.BaseUrl = restApiRequest.WebApiUri;

            RestRequest request = new RestRequest(restApiRequest.ActionName);
            Logger.Info(string.Format("Req URL:{0}{1}", restApiRequest.WebApiUri, restApiRequest.ActionName));
            foreach (var key in restApiRequest.Headers.Keys)
            {
                request.AddHeader(Convert.ToString(key), Convert.ToString(restApiRequest.Headers[key]));
                Logger.Info(string.Format("Parameter:{0}{1}", Convert.ToString(key), Convert.ToString(restApiRequest.Headers[key])));
            }

            //Call Server Controller Action Method with Request Url and Request Type
            var response = restClient.ExecuteAsGet(request, "GET");
            Logger.Info(string.Format("Res:Code:{0} Content:{1}", response.StatusCode, response.Content));
            return response;
        }

        public IRestResponse Post(RestApiRequest restApiRequest)
        {
            RestClient restClient = new RestClient();
            restClient.BaseUrl = restApiRequest.WebApiUri;

            RestRequest request = new RestRequest(restApiRequest.ActionName);
            Logger.Info(string.Format("Req URL:{0}{1}", restApiRequest.WebApiUri, restApiRequest.ActionName));
            foreach (var key in restApiRequest.Headers.Keys)
            {
                request.AddHeader(Convert.ToString(key), Convert.ToString(restApiRequest.Headers[key]));
                Logger.Info(string.Format("Parameter:{0}{1}", Convert.ToString(key), Convert.ToString(restApiRequest.Headers[key])));
            }

            if (restApiRequest.RequestObject != null)
            {
                request.RequestFormat = RestSharp.DataFormat.Json;
                request.AddBody(restApiRequest.RequestObject);
            }

            //Call Server Controller Action Method with Request Url and Request Type
            var response = restClient.ExecuteAsPost(request, "POST");
            Logger.Info(string.Format("Res:Code:{0} Content:{1}", response.StatusCode, response.Content));
            return response;
        }
    }
}