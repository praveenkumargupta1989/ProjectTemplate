using ACIPL.Template.Client.Web.Handlers;
using ACIPL.Template.Core.Logging;
using ACIPL.Template.Core.Utilities;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ACIPL.Template.Client.Web
{
    public class MvcApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IConfigurationManager>().To<ConfigurationManager>();
            kernel.Bind<IFileManager>().To<FileManager>();
            kernel.Bind<IEmailManager>().To<IEmailManager>();
            kernel.Bind<IRestClientHandler>().To<RestClientHandler>();
        }

        public void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs
            Logger Logger = LoggerFactory.GetLogger();
            var exception = Server.GetLastError().GetBaseException();
            Logger.Fatal(exception);
        }
    }
}
