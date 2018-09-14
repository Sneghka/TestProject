using Cwc.BaseData.Classes;
using Cwc.Common;
using Cwc.Common.Extensions.Data;
using Cwc.Jobs;
using Cwc.Sync;
using CWC.AutoTests.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Configuration;
using System.Xml;

namespace CWC.AutoTests.Helpers
{
    public class BasicImportHelper
    {
        private Dictionary<string, Type> syncMappers = null;
        private Dictionary<string, string> syncNameMappers = null;
        private string folderPath;
        private string fileName;

        public BasicImportHelper(string path)
        {
            folderPath = path;
            syncMappers = SyncConfiguration.LoadSyncMappers();
            syncNameMappers = SyncConfiguration.LoadSyncNameMappers();
        }

        /// <summary>
        /// Save XML document
        /// </summary>
        /// <param name="folderPath">Path to import folder</param>
        /// <param name="entity">Entity which will be imported</param>
        /// <param name="content">XML content</param>
        public void SaveXml(BasicImportEntity entity, string content)
        {         
            fileName = this.ComposeFileName(folderPath, entity);
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(content);
            using (XmlTextWriter writer = new XmlTextWriter(fileName, null))
            {
                writer.Formatting = Formatting.Indented;
                xdoc.Save(writer);
            }

            var createTime = File.GetCreationTime(fileName);
            File.SetCreationTime(fileName, createTime.AddSeconds(-30));

            var lastTime = File.GetLastWriteTime(fileName);
            File.SetLastWriteTime(fileName, lastTime.AddSeconds(-30));            
        }

        /// <summary>
        /// Import xml document via Basic Import. Methods uses Basic Import job logic from CWC solution
        /// </summary>
        public void Import()
        {
            ConfigurationKeySet.Load();
            DataRow basicImport = GetBasicImportJobInstanceDataRow();
            SyncSettings settings = null;

            using (var sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebPortalConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                settings = SyncFacade.SyncSettingsService.Load((int)basicImport["Settings_id"], new DataBaseParams(sqlConnection));
                if (settings == null)
                {
                    throw new Exception("Error on loading SyncSettings!");
                }
            }

            settings.ExchangeFolder = folderPath;
            settings.JobInstance_id = (int)basicImport["id"];
            settings.SyncMappers = syncMappers;
            settings.SyncNameMappers = syncNameMappers;
            settings.FormatInfos = new FileFormatInfo[] 
            {
                new FileFormatInfo("*.xml", FileFormat.XML),
                new FileFormatInfo("*.csv", FileFormat.CSV)
            };

            settings.SchemasFolderName = JobSchemasHelper.GetSchemasFolderNameByJobType<BasicImportJob>();
            var ts = TransportService.Create(settings);
            ts.Import();
            //while (File.Exists(fileName))
            //{              
            //    ts.Import();
            //    Thread.Sleep(1000);
            //}
        }

        /// <summary>
        /// Verify that entity record is created successfully
        /// </summary>
        /// <param name="tableName">Entity table name</param>       
        /// <param name="dataToVerify">Dictionary with key = unique column name, value = unique column value</param>
        /// <returns>Result</returns>
        public bool VerifyEntity(string tableName, Dictionary<string, object> dataToVerify)
        {
            using (var sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebPortalConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var dataAdapter = new SqlDataAdapter(String.Format(@"SELECT * FROM {0}
                                                                            WHERE {1} = @Param", tableName, dataToVerify.First().Key), sqlConnection))
                {
                    dataAdapter.SelectCommand.Parameters.Add(new SqlParameter("Param", dataToVerify.First().Value));
                    var dt = new DataTable();
                    dataAdapter.Fill(dt);
                    return (dt.Rows.Count > 0);
                }
            }
        }

        /// <summary>
        /// Verify that entity record is updated correctly
        /// </summary>
        /// <param name="tableName">Entity table name</param>
        /// <param name="idColumnData">Dictionary with key = unique column name, value = unique column value. Used to find updated record.</param>
        /// <param name="dataToVerify">Dictionary with keys = column names to verify, values = values of columns to verify</param>
        /// <returns>Result</returns>
        public bool VerifyEntity(string tableName, Dictionary<string, object> idColumnData, Dictionary<string, object> dataToVerify)
        {            
            var result = true;

            using (var sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebPortalConnectionString"].ConnectionString))
            {
                sqlConnection.Open();                               
                using (var dataAdapter = new SqlDataAdapter(String.Format(@"SELECT * FROM {0}
                                                                            WHERE {1} = @Param", tableName, idColumnData.First().Key), sqlConnection))
                {
                    dataAdapter.SelectCommand.Parameters.Add(new SqlParameter("Param", idColumnData.First().Value));
                    var dt = new DataTable();
                    dataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        var i = 0;
                        while (result && i < dataToVerify.Count())
                        {
                            var column = dataToVerify.ElementAt(i);
                            var type = dt.Rows[0].Table.Columns[column.Key].DataType;
                            var expectedValue = column.Value;
                            var actualValue = dt.Rows[0][column.Key];
                            if (type == typeof(string))
                            {
                                result = DataUtils.GetString(expectedValue) == DataUtils.GetString(actualValue);
                            }

                            if (type == typeof(int))
                            {
                                result = DataUtils.GetInt32(actualValue) == (int)expectedValue;
                            }

                            if (type == typeof(decimal))
                            {
                                result = DataUtils.GetDecimal(actualValue) == Convert.ToDecimal(expectedValue);
                            }

                            if (type == typeof(bool))
                            {
                                result = DataUtils.GetBollean(actualValue) == (bool)expectedValue;
                            }

                            if (type == typeof(DateTime))
                            {                                
                                result = DataUtils.GetDateTime(actualValue) == DateTime.ParseExact(expectedValue.ToString(), "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);                               
                            }

                            i += 1;
                        }
                    }

                    else return result = false;
                }
            }

            return result;            
        }

        //public void DeleteEntity(string tableName, string columnName, string paramValue)
        public void DeleteEntity(string tableName, Dictionary<string, object> idColumnData)
        {            
            DataBaseHelper.Delete(tableName, String.Format("{0} = @{0}", idColumnData.First().Key), idColumnData, null);
        }

        /// <summary>
        /// Gets data row of basic import job instance
        /// </summary>
        /// <returns>Data row</returns>
        private DataRow GetBasicImportJobInstanceDataRow()
        {
            DataRow basicImportDataRow;
            using (var sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebPortalConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var dataAdapter = new SqlDataAdapter(String.Format(@"SELECT * FROM {0} AS [JIns]
                                                                                    JOIN {1} AS [JInf] ON [JIns].JobInfo_id = [JInf].id
                                                                                    WHERE [JInf].Name LIKE @JobName",
                                                                                    typeof(JobInstance).GetTableName(),
                                                                                    typeof(JobInfo).GetTableName()), sqlConnection))
                {
                    dataAdapter.SelectCommand.Parameters.Add(new SqlParameter("JobName", "Basic Import"));
                    var dt = new DataTable();
                    dataAdapter.Fill(dt);
                    basicImportDataRow = dt.Rows[0];
                }
            }

            return basicImportDataRow;
        }        

        private string ComposeFileName(string folderPath, BasicImportEntity entity)
        {
            string fileName = String.Concat(folderPath, "\\", entity.ToString(), "-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".xml");
            return fileName;
        }
    }
}
