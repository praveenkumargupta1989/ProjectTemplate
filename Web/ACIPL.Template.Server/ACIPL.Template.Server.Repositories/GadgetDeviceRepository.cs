using ACIPL.Template.Server.DataAccess;
using ACIPL.Template.Server.DataAccess.Extensions;
using ACIPL.Template.Server.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ACIPL.Template.Server.Repositories
{
    public interface IGadgetDeviceRepository
    {
        GadgetDeviceDetail GetGadgetDeviceDetail(int employeeId);
    }

    public class GadgetDeviceRepository : IGadgetDeviceRepository
    {
        private readonly IDataAccess dataAccess;

        public GadgetDeviceRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public GadgetDeviceDetail GetGadgetDeviceDetail(int employeeId)
        {
            var parameters = new List<Parameter>
                {
                    new Parameter("@EmployeeId", employeeId)
                };

            var dr = dataAccess.ExecuteReader("dspGadgetDetailByEmpIdPMOB", parameters, CommandType.StoredProcedure);
            return dr.Select(dataRecord => new GadgetDeviceDetail
            {
                DeviceId = ConvertTo<string>.From(dataRecord["DeviceNumber"]),
                AppVersion = ConvertTo<string>.From(dataRecord["VersionName"])
            }).FirstOrDefault();
        }
    }
}
