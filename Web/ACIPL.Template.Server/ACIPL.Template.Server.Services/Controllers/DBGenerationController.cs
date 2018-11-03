using ACIPL.Template.Core.Utilities;
using ACIPL.Template.Server.Models;
using ACIPL.Template.Server.Repositories;
using ACIPL.Template.Server.Services.Attributes;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ACIPL.Template.Server.Services.Controllers
{
    public class DBGenerationController : BaseApiController
    {
        private readonly IDBGenerationRepository dbGenerationRepository;
        private readonly IConfigurationManager configurationManager;
        private readonly IFileManager fileManager;

        public DBGenerationController(IDBGenerationRepository dbGenerationRepository, IConfigurationManager configurationManager,
            IFileManager fileManager)
        {
            this.dbGenerationRepository = dbGenerationRepository;
            this.configurationManager = configurationManager;
            this.fileManager = fileManager;
        }

        [HttpPost]
        [SkipMyGlobalActionFilter]
        public IHttpActionResult DBRefresh()
        {
            //Get MasterTable List for update/delete
            var data = dbGenerationRepository.GetMasterTableName();
            var status = false;

            if (Convert.ToBoolean(data.Result))
            {
                //Generate db files on folder and fill it's data
                var divisionWiseDbPath = configurationManager.GetConfigurationValue("RootPath") + configurationManager.GetConfigurationValue("DivisionWiseDbPath");
                var dBStructurePath = configurationManager.GetConfigurationValue("RootPath") + configurationManager.GetConfigurationValue("DBStructurePath");

                Logger.Info(string.Format("DBRefresh divisionWiseDbPath - {0}", divisionWiseDbPath));
                Logger.Info(string.Format("DBRefresh dBStructurePath - {0}", dBStructurePath));

                fileManager.CreateDirectory(divisionWiseDbPath, true);

                //Get Division wise Master table List with Structure and table name.
                List<DBStructure> lstTblStructure = GetTableListFromDbFile(dBStructurePath);

                //Get Division List for dbgeneration
                //var divisionList = dbGenerationRepository.GetDivision();
                //foreach (var item in divisionList)
                //{
                //    //Get path for division wise db file.
                //    var divisionWiseDbFilePath = string.Format("{0}PharmawareSMD{1}.db", divisionWiseDbPath, item.Id);

                //    //Create Division wise db file in DivisionWiseDbPath folder .                  
                //    File.Copy(dBStructurePath, divisionWiseDbFilePath);
                //    //Create all Tables in division wise db
                //    CreateTable(lstTblStructure, divisionWiseDbFilePath, item.Id);
                //}

                //Update "MasterTableName" Table EditedDate Column with current date.
                status = dbGenerationRepository.UpdateMasterTableName();
            }
            return Ok(status);
        }

        public List<DBStructure> GetTableListFromDbFile(string dBStructurePath)
        {
            var lstTblStruct = new List<DBStructure>();
            //using (SQLiteConnection conn = new SQLiteConnection(@"Data Source= " + dBStructurePath + ";Version=3;"))
            //{
            //    conn.Open();
            //    using (SQLiteCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = "SELECT * FROM Sqlite_Master where type = 'table' order by tbl_name";
            //        cmd.CommandType = CommandType.Text;
            //        using (SQLiteDataReader dr = cmd.ExecuteReader())
            //        {
            //            while (dr.Read())
            //            {
            //                var dbstructure = new DBStructure();
            //                dbstructure.TableName = Convert.ToString(dr["tbl_name"]);
            //                dbstructure.TableStructure = Convert.ToString(dr["sql"]);
            //                lstTblStruct.Add(dbstructure);
            //            }
            //            dr.Close();
            //        }
            //    }
            //    conn.Close();
            //}
            return lstTblStruct;
        }

        public bool CreateTable(List<DBStructure> lstTblStructure, string divisionWiseDbFilePath, int divisionId)
        {
            //SQLiteConnection.ClearAllPools();
            //using (SQLiteConnection conn = new SQLiteConnection(@"Data Source=" + divisionWiseDbFilePath + ";Version=3;"))
            //{
            //    conn.Open();
            //    using (SQLiteCommand cmd = conn.CreateCommand())
            //    {
            //        foreach (var item in lstTblStructure)
            //        {
            //            var dataList = dbGenerationRepository.GetTableData(item.TableName, divisionId);

            //            if (dataList.Rows.Count > 0)
            //            {
            //                DataTable columnNameTypes = new DataTable();
            //                //Get Column Name and Data type of table from source database.

            //                cmd.CommandText = string.Format("pragma table_info ='{0}'", item.TableName);
            //                cmd.CommandType = CommandType.Text;
            //                using (SQLiteDataReader dr = cmd.ExecuteReader())
            //                {
            //                    columnNameTypes.Load(dr);
            //                    dr.Close();
            //                }

            //                List<string> allCol = new List<string>();
            //                List<string> allParam = new List<string>();
            //                foreach (DataRow column in columnNameTypes.Rows)
            //                {
            //                    var clientStructureColumn = column.ItemArray[1].ToString();
            //                    if ("rowid".Equals(clientStructureColumn, StringComparison.OrdinalIgnoreCase))
            //                        continue;
            //                    if (dataList.Columns.Contains(clientStructureColumn))
            //                    {
            //                        allCol.Add(clientStructureColumn);
            //                        allParam.Add("@" + clientStructureColumn);
            //                    }
            //                }

            //                var commaSeparated = string.Join(",", allCol);
            //                var filteredData = new DataView(dataList).ToTable("FilteredData", false, allCol.ToArray());

            //                using (var transaction = conn.BeginTransaction())
            //                {
            //                    foreach (DataRow para in filteredData.Rows)
            //                    {
            //                        cmd.Parameters.Clear();
            //                        for (int i = 0; i < allCol.Count; i++)
            //                        {
            //                            cmd.Parameters.AddWithValue("@" + allCol[i], para.ItemArray.GetValue(i));
            //                        }
            //                        cmd.CommandText = "Insert INTO " + item.TableName + "(" + commaSeparated + ") " + "Values(" + string.Join(",", allParam) + ")";
            //                        cmd.ExecuteNonQuery();
            //                    }
            //                    transaction.Commit();
            //                }
            //            }
            //        }
            //    }
            //    conn.Close();
            //}
            return true;
        }
    }
}
