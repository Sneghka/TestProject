using Cwc.Common;
using CWC.AutoTests.Enums;
using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.Common.Extensions.Data;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace CWC.AutoTests.Helpers.Inbound
{
    public static class ContainerProcessingHelper
    {
        /// <summary>
        /// Check Status of processed deposits
        /// </summary>
        /// <param name="process"> Cash Center process for which status verification should be made </param>
        /// <param name="depositNumberList"> List of deposit numbers </param>
        /// <returns> True - if all deposits have expected status, otherwise returns False </returns>
        public static bool CheckContainerStatus(InboundProcess process, List<string> depositNumberList)
        {
            bool checkResult = false;
            var sql = string.Format(@"SELECT * FROM {0}
                                    WHERE [Number] IN ({1})
                                    AND [PreannouncementType] IS NULL",
                                    typeof(StockContainer).GetTableName(),
                                    "'" + string.Join("','", depositNumberList.ToArray()) + "'");

            var deposits = DataBaseHelper.Select<StockContainer>(sql, null, null);

            if (process == InboundProcess.Capturing)
            {
                foreach (var deposit in deposits)
                {
                    if (deposit.Status == SealbagStatus.Captured)
                    {
                        checkResult = true;
                    }
                    else return checkResult = false;
                }
            }

            if (process == InboundProcess.Counting)
            {
                foreach (var deposit in deposits)
                {
                    if (deposit.Status == SealbagStatus.Counted)
                    {
                        checkResult = true;
                    }
                    else return checkResult = false;
                }
            }
            return checkResult;
        }

        /// <summary>
        /// Check Status of processed mother deposits that have inner deposits
        /// </summary>
        /// <param name="process"> Cash Center process for which status verification should be made </param>
        /// <param name="preannouncements"> Multidimensional array with mother and inner deposit numbers </param>
        /// <returns> True - if all deposits have expected status, otherwise returns False </returns>
        public static bool CheckContainerStatus(InboundProcess process, string[,] preannouncements)
        {
            string[] depositNumberList = new string[preannouncements.GetLength(0) * preannouncements.GetLength(1)];
            bool checkResult = false;

            depositNumberList = MultiArrayToSingleArray(preannouncements);

            var sql = string.Format(@"SELECT * FROM {0}
                                    WHERE [Number] IN ({1})
                                    AND [PreannouncementType] IS NULL",
                                    typeof(StockContainer).GetTableName(),
                                    "'" + string.Join("','", depositNumberList) + "'");

            var deposits = DataBaseHelper.Select<StockContainer>(sql, null, null);

            if (process == InboundProcess.Capturing)
            {
                foreach (var deposit in deposits)
                {
                    if (deposit.Status == SealbagStatus.Captured)
                    {
                        checkResult = true;
                    }
                    else return checkResult = false;
                }
            }

            if (process == InboundProcess.Counting)
            {
                foreach (var deposit in deposits)
                {
                    if (deposit.Status == SealbagStatus.Counted)
                    {
                        checkResult = true;
                    }
                    else return checkResult = false;
                }
            }
            return checkResult;
        }

        public static void ClearProcessingSession(string workstation)
        {
            SqlConnection sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["LocalWebPortalConnectionString"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Parameters.Add(new SqlParameter("Workstation", workstation));
            cmd.CommandText = string.Format(@"DELETE [PS] FROM {0} AS [PS]
                                              INNER JOIN {1} AS [W]
                                              ON [W].[id] = [PS].[WorkstationID]
                                              WHERE [W].[Name] LIKE @Workstation", 
                                              typeof(ProcessingSession).GetTableName(), 
                                              typeof(Workstation).GetTableName());            
            cmd.Connection = sqlConnection;
            sqlConnection.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }

            finally
            {
                sqlConnection.Close();
            }
        }

        /// <summary>
        /// Input Location for matching
        /// </summary>
        /// <param name="element"> Input field on a form </param>
        /// <param name="location"> Location that should be matched </param>
        public static void InputLocation(IWebElement element, string location)
        {
            try
            {
                element.Clear();
                element.SendKeys(location);
                element.SendKeys(Keys.Enter);
            }

            catch (StaleElementReferenceException)
            {
                InputLocation(element, location);
            }

            catch (InvalidElementStateException)
            {
                InputLocation(element, location);
            }
        }

        /// <summary>
        /// Convert multidimensional array to single array
        /// </summary>
        /// <param name="array"> Multidimensional array </param>
        /// <returns> Single array </returns> 
        public static string[] MultiArrayToSingleArray(string[,] array)
        {
            int index = 0;
            int width = array.GetLength(0);
            int height = array.GetLength(1);
            string[] single = new string[width * height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    single[index] = array[i, j];
                    index++;
                }
            }
            return single;
        }
    }
}
