using ACIPL.Template.Server.DataAccess;
using ACIPL.Template.Server.DataAccess.Extensions;
using ACIPL.Template.Server.Models;
using ACIPL.Template.Server.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ACIPL.Template.Server.Repositories
{
    public interface IDBGenerationRepository
    {
        MasterDBStructure GetMasterTableName();
        IEnumerable<DivisionMaster> GetDivision();
        IEnumerable<MasterDBStructure> GetMasterTableNameList();
        DataTable GetTableData(string tableName, int divisionId);
        bool UpdateMasterTableName();
        void AddEditDbGenerationLog(DbGenerationLog entity);
        DbGenerationLog GetDBGenerationLog(string token);
        DataTable GetEmployeeWiseMasterTableData(string tableName, int divisionId, int employeeId, int orgId);
    }

    public class DBGenerationRepository : IDBGenerationRepository
    {
        private readonly IDataAccess dataAccess;

        public DBGenerationRepository(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public MasterDBStructure GetMasterTableName()
        {
            var dr = dataAccess.ExecuteReader("dspMasterTableNamePMOB", null, CommandType.StoredProcedure);
            return dr.Select(dataRecord => new MasterDBStructure
            {
                Result = ConvertTo<int>.From(dataRecord["Result"])
            }).SingleOrDefault();
        }

        public IEnumerable<DivisionMaster> GetDivision()
        {
            var dr = dataAccess.ExecuteReader("dspGetDivisionPMOB", null, CommandType.StoredProcedure);
            return dr.Select(dataRecord => new DivisionMaster
            {
                Id = ConvertTo<int>.From(dataRecord["Id"]),
                Name = ConvertTo<string>.From(dataRecord["Name"]),
                Abbreviation = ConvertTo<string>.From(dataRecord["Abbreviation"]),
                CompanyMasterId = ConvertTo<int>.From(dataRecord["CompanyMasterId"]),
                EditedDate = ConvertTo<DateTime>.From(dataRecord["EditedDate"])
            }).ToList();
        }

        public IEnumerable<MasterDBStructure> GetMasterTableNameList()
        {
            var dr = dataAccess.ExecuteReader("dspMasterTableNameListPMOB", null, CommandType.StoredProcedure);
            return dr.Select(dataRecord => new MasterDBStructure
            {
                Id = ConvertTo<int>.From(dataRecord["Id"]),
                TableName = ConvertTo<string>.From(dataRecord["TableName"]),
                EditedDate = ConvertTo<DateTime>.From(dataRecord["EditedDate"]),
                Active = ConvertTo<bool>.From(dataRecord["Active"])
            }).ToList();
        }

        public DataTable GetTableData(string tableName, int divisionId)
        {
            var parameters = new List<Parameter>
            {
                    new Parameter("@TableName", tableName),
                    new Parameter("@DivisionId",divisionId)
            };

            DataTable dt = null;
            dt = dataAccess.ExecuteCommand("dspGetTableDataPMOB", parameters, CommandType.StoredProcedure);
            return dt;
        }

        public bool UpdateMasterTableName()
        {
            int i = dataAccess.ExecuteNonQuery("iudMasterTableNamePMOB", null, CommandType.StoredProcedure);
            return Convert.ToBoolean(i);
        }

        public void AddEditDbGenerationLog(DbGenerationLog entity)
        {
            var parameters = new List<Parameter>
            {
                    new Parameter("@Token", entity.Token),
                    new Parameter("@EmployeeId",entity.EmployeeId),
                    new Parameter("@ProcessStatusMasterId",entity.ProcessStatusMasterId),
            };
            dataAccess.ExecuteNonQuery("iudDbGenrationLogPMOB", parameters, CommandType.StoredProcedure);
        }

        public DbGenerationLog GetDBGenerationLog(string token)
        {
            var parameters = new List<Parameter>
            {
                    new Parameter("@Token", token),
            };
            var dr = dataAccess.ExecuteReader("dspDBGenerationLogPMOB", parameters, CommandType.StoredProcedure);

            return dr.Select(x => GetDBGenerationModel(x)).FirstOrDefault();
        }

        private DbGenerationLog GetDBGenerationModel(IDataRecord x)
        {
            var obj = new DbGenerationLog();
            obj.Id = ConvertTo<int>.From(x["Id"]);
            obj.Token = ConvertTo<string>.From(x["Token"]);
            obj.EmployeeId = ConvertTo<int>.From(x["EmployeeId"]);
            obj.ProcessStatusMasterId = ConvertTo<ProcessStatusMaster>.From(x["ProcessStatusMasterId"]);
            obj.CreatedDate = Convert.ToDateTime((ConvertTo<DateTime?>.From(x["CreatedDate"])));
            obj.EditedDate = Convert.ToDateTime(ConvertTo<DateTime?>.From(x["EditedDate"]));
            return obj;
        }

        public DataTable GetEmployeeWiseMasterTableData(string tableName, int divisionId, int employeeId, int orgId)
        {
            var parameters = new List<Parameter>
            {
                    new Parameter("@TableName", tableName),
                    new Parameter("@DivisionId",divisionId),
                    new Parameter("@OrgId",orgId),
                    new Parameter("@EmployeeId",employeeId)
            };

            DataTable dt = null;
            dt = dataAccess.ExecuteCommand("dspGetEmployeeWiseTableDataForMasterPMOB", parameters, CommandType.StoredProcedure);
            return dt;
        }
    }
}
