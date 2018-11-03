using ACIPL.Template.Server.DataAccess;
using ACIPL.Template.Server.DataAccess.Extensions;
using ACIPL.Template.Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ACIPL.Template.Server.Repositories
{
    public interface ISyncRepository
    {
        IEnumerable<SyncTableOrder> GetSyncTableOrder();
        string SyncData(int employeeId, string tableName, string storedProcedureName);
        SyncAckNack SyncAckNack(SyncAckNack entity);
        SyncTableOrder GetTableDetail(string tableName);
        DataTable GetAllEmployeeHierarchyByEmployeeId(int employeeId);
    }
    public class SyncRepository : ISyncRepository
    {
        private readonly IDataAccess dataAccess;

        public SyncRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public IEnumerable<SyncTableOrder> GetSyncTableOrder()
        {
            var dr = dataAccess.ExecuteReader("dspSyncTableOrderListPMOB", null, CommandType.StoredProcedure);

            return dr.Select(dataRecord => new SyncTableOrder
            {
                Id = ConvertTo<int>.From(dataRecord["Id"]),
                Name = ConvertTo<string>.From(dataRecord["Name"]),
                TempTableName = ConvertTo<string>.From(dataRecord["TempTableName"]),
                StoredProcedureName = ConvertTo<string>.From(dataRecord["StoredProcedure"]),
                Order = ConvertTo<int>.From(dataRecord["Order"]),
                ResponseData = ConvertTo<bool>.From(dataRecord["ResponseData"]),
                SyncMethod = ConvertTo<string>.From(dataRecord["SyncMethod"]),
                Action = ConvertTo<string>.From(dataRecord["Action"])
            }).ToList();
        }

        public string SyncData(int employeeId, string tableName, string storedProcedureName)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("@EmployeeId",employeeId),
                new Parameter("@TableName",tableName)
            };
            var dt = dataAccess.ExecuteCommand(storedProcedureName, parameters, CommandType.StoredProcedure);
            return Convert.ToString(dt.Rows[0].ItemArray[0]);
        }

        public SyncAckNack SyncAckNack(SyncAckNack entity)
        {
            var parameters = new List<Parameter>
            {
                    new Parameter("@TableName",entity.Name),
                    new Parameter("@AckNack",entity.AckNack),
                    new Parameter("@EmployeeId",entity.EmployeeId)
            };
            var dr = dataAccess.ExecuteReader("iudSyncAckNack", parameters, CommandType.StoredProcedure);
            return dr.Select(dataRecord => new SyncAckNack
            {
                Name = ConvertTo<string>.From(dataRecord["TableName"]),
                Status = ConvertTo<string>.From(dataRecord["Status"]),
                AckNack = ConvertTo<string>.From(dataRecord["AckNack"]),
                EmployeeId = ConvertTo<int>.From(dataRecord["EmployeeId"])
            }).FirstOrDefault();
        }

        public SyncTableOrder GetTableDetail(string tableName)
        {
            var parameters = new List<Parameter>
            {
                    new Parameter("@TableName",tableName)
            };
            var dr = dataAccess.ExecuteReader("dspGetTableDetailByName", parameters, CommandType.StoredProcedure);

            return dr.Select(dataRecord => new SyncTableOrder
            {
                Id = ConvertTo<int>.From(dataRecord["Id"]),
                Name = ConvertTo<string>.From(dataRecord["Name"]),
                TempTableName = ConvertTo<string>.From(dataRecord["TempTableName"]),
                StoredProcedureName = ConvertTo<string>.From(dataRecord["StoredProcedure"]),
                Order = ConvertTo<int>.From(dataRecord["Order"]),
                ResponseData = ConvertTo<bool>.From(dataRecord["ResponseData"]),
                SyncMethod = ConvertTo<string>.From(dataRecord["SyncMethod"])
            }).FirstOrDefault();
        }

        public DataTable GetAllEmployeeHierarchyByEmployeeId(int employeeId)
        {
            var parameters = new List<Parameter>
            {
                new Parameter("@EmployeeId", employeeId),
            };

            DataTable dt = null;
            dt = dataAccess.ExecuteCommand("GetAllEmployeeHierarchyByEmployeeId", parameters, CommandType.StoredProcedure);
            return dt;
        }
    }
}
