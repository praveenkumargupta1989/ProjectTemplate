using ACIPL.Template.Core.Utilities;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ACIPL.Template.Server.Services.Controllers
{
    public class DownloadController : BaseApiController
    {
        private readonly IConfigurationManager configurationManager;

        public DownloadController(IConfigurationManager configurationManager)
        {
            this.configurationManager = configurationManager;
        }

        [HttpGet]
        public HttpResponseMessage DBStructure(string id)
        {
            var rootPath = configurationManager.GetConfigurationValue("RootPath");
            var uploadPath = rootPath + configurationManager.GetConfigurationValue("UserDBPath");
            Logger.Info("DBStructure UserDBPath -" + uploadPath);

            var zipFile = uploadPath + id + ".zip";

            Logger.Info("DBStructure zipFilePath -" + zipFile);

            if (!File.Exists(zipFile))
            {
                HttpResponseMessage message = new HttpResponseMessage(HttpStatusCode.NotImplemented);
                message.Content = new StringContent("Download fail.");
                return message;
            }

            FileStream fileStream = new FileStream(zipFile, FileMode.Open, FileAccess.Read);
            long fileLength = new FileInfo(zipFile).Length;

            var response = new HttpResponseMessage();
            response.Content = new StreamContent(fileStream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = id + ".zip";
            response.Content.Headers.ContentLength = fileLength;
            return response;
        }
    }
}
