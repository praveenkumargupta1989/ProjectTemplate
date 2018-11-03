using ACIPL.Template.Server.DataAccess;
using ACIPL.Template.Server.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace ACIPL.Template.Server.Repositories
{
    public interface ILoginRepository : IBaseRepository<Login>
    {
        Login ValidateUser(Login entity);
    }

    public class LoginRepository : BaseRepository<Login>, ILoginRepository
    {
        public LoginRepository()
        {
        }

        public Login ValidateUser(Login entity)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("@UserName",entity.UserName),
                new Parameter("@password",entity.Password)
            };

            var dr = DataAccess.ExecuteReader("dspValidateUser", parameters, CommandType.StoredProcedure);
            var data = new Login();
            foreach (var item in dr)
            {
                data.IsValidUser = Convert.ToBoolean(item["IsValidUser"]);
            }
            return data;
        }
    }
}