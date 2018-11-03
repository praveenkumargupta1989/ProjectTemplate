using ACIPL.Template.Server.DataAccess;
using ACIPL.Template.Server.DataAccess.Extensions;
using ACIPL.Template.Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ACIPL.Template.Server.Repositories
{
    public interface INotificationRepository
    {
        NotificationStatus SaveNotificationDetail(NotificationStatus entity);
        IEnumerable<NotificationStatus> GetNotificationListByEmployeeId(int EmployeeId);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly IDataAccess dataAccess;

        public NotificationRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public NotificationStatus SaveNotificationDetail(NotificationStatus entity)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("@Id",entity.Id),
                new Parameter("@EmployeeId",entity.EmployeeId),
                new Parameter("@Command",entity.Command),
                new Parameter("@IsAck",entity.IsAck)
            };
            var dr = dataAccess.ExecuteReader("iudNotificationStatusPMOB", parameters, CommandType.StoredProcedure);
            foreach (var dataRecord in dr)
            {
                entity.Id = ConvertTo<int>.From(dataRecord["Id"]);
            }
            return entity;
        }

        public IEnumerable<NotificationStatus> GetNotificationListByEmployeeId(int EmployeeId)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("@EmployeeId",EmployeeId)
            };
            var dr = dataAccess.ExecuteReader("dspNotificationStatusByEmployeeIdMOB", parameters, CommandType.StoredProcedure);
            return dr.Select(dataRecord => new NotificationStatus
            {
                Id = ConvertTo<int>.From(dataRecord["Id"]),
                EmployeeId = ConvertTo<int>.From(dataRecord["EmployeeId"]),
                Command = Convert.ToString(dataRecord["Command"]),
                IsAck = Convert.ToBoolean(dataRecord["IsAck"]),
                //CreatedDate = ConvertTo<DateTime>.From(dataRecord["CreatedDate"]),
                //EditedDate = ConvertTo<DateTime>.From(dataRecord["EditedDate"])
            }).ToList();
        }
    }
}
