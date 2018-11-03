using ACIPL.Template.Server.Repositories;
using ACIPL.Template.Server.Services.Attributes;
using System.Web.Http;

namespace ACIPL.Template.Server.Services.Controllers
{
    public class LoginController : BaseApiController
    {
        private readonly ILoginRepository loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            this.loginRepository = loginRepository;
        }

        [SkipMyGlobalActionFilter]
        public IHttpActionResult Get()
        {
            return Ok(new Models.Login { });
        }

        [HttpPost]
        public IHttpActionResult Authorize(Models.Login login)
        {
            loginRepository.ValidateUser(login);
            return Ok(new Models.Login { UserName = login.UserName, Password = login.Password });
        }
    }
}
