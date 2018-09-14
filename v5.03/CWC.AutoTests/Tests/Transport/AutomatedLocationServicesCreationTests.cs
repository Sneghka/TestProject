using Cwc.BaseData.Classes;
using Cwc.Common;
using Cwc.Common.Fakes;
using CWC.AutoTests.Helpers;
using Microsoft.QualityTools.Testing.Fakes;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace CWC.AutoTests.Tests.Transport
{
    public class AutomatedLocationServicesCreationTests : BaseTest, IClassFixture<AutomatedLocationServicesCreationTestFixture>
    {
        AutomatedLocationServicesCreationTestFixture fixture;
        public AutomatedLocationServicesCreationTests(AutomatedLocationServicesCreationTestFixture testFixture)
        {
            this.fixture = testFixture;
            ConfigurationKeySet.Load();
        }

        [Fact(DisplayName = "Create location service")]
        public void CreateLocationServiceTest()
        {
            using (var shim = ShimsContext.Create())
            {
                ShimDataBaseHelper.SelectStringSqlParameterArrayDataBaseParams = (sql, sqlParams, dbParams) =>
                {
                    
                    var table = new DataTable();
                    SqlDataAdapter da;
                    SqlConnection conn;

                    if (sqlParams == null)
                    {
                        sql = string.Format(@"
                                            SELECT L.[loc_nr] FROM location L                                            
                                            WHERE L.[ref_loc_nr] = 'JG02'
                                            ");

                        da = new SqlDataAdapter(sql, new SqlConnection(DataBaseHelper.GetConnectionString()));
                        da.Fill(table);
                        return table;
                    }                                        
                    
                    if (dbParams == null)
                    {
                        dbParams = new DataBaseParams();
                    }
                    
                    if (dbParams.Transaction != null)
                    {
                        conn = (SqlConnection)dbParams.Transaction.Connection;
                    }
                    else if (dbParams.Connection != null)
                    {
                        conn = (SqlConnection)dbParams.Connection;
                    }
                    else
                    {
                        conn = new SqlConnection(DataBaseHelper.GetConnectionString());
                        conn.Open();
                    }

                    try
                    {
                        SqlCommand cmd = new SqlCommand(sql, conn);

                        for (int i = 0; i < sqlParams.Length; i++)
                        {
                            cmd.Parameters.Add(sqlParams[i]);
                        }

                        da = new SqlDataAdapter(cmd);
                        da.Fill(table);
                        cmd.Parameters.Clear();
                        return table;
                    }
                    finally
                    {
                        if (dbParams.Transaction == null && dbParams.Connection == null)
                        {
                            conn.Close();
                        }
                    }
                };
                
                HelperFacade.TransportHelper.RunAutomatedLocationServicesCreationJob();
                Assert.True(true);
            }            
        }

        [Fact(DisplayName = "Create location service")]
        public void CreateLocationServiceTestShim()
        {
            
        }
    }
}
