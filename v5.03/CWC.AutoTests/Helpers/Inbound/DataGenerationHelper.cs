using System;
using System.Data.SqlClient;
using Cwc.Common;
using Cwc.CashCenter;
using Cwc.BaseData;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CWC.AutoTests.Core;
using Cwc.Common.Extensions.Data;

namespace CWC.AutoTests.Helpers.Inbound
{
    public class DataGenerationHelper
    {
        public DataGenerationHelper()
        {            
        }

        private const string dateFormat = "ddMMyyHHmmssffff";

        // Generation of Deposit Number list for testing of capturing without stock positions
        public List<string> GenerateDepositNumberList(int number, string locationCode)
        {
            List<string> depositNumberList = new List<string>();

            for (int i = 0; i < number; i++)
            {
                depositNumberList.Add("PRE" + DateTime.Now.ToString(dateFormat));
            }

            return depositNumberList;
        }
        
        // Customer electronic pre-announcements generation for testing of fast way capturing        
        // first parameter = number of pre-announcements
        // second parameter = Location_From_Id
        // third parameter = Flag for stock positions generation
        public List<string> GenerateElectronicPreannouncements(int number, string locationCode)
        {
            List<string> depositNumberList = new List<string>();
            int i;
            Result rslt;            
            StockContainer[] sc = new StockContainer[number];
            var locationId = SettingsHelper.GetMandatoryLocationId(locationCode);
            var stockLocationId = GetStockLocationId(Configuration.StockLocationName, Configuration.StockLocationDescription);
            var eur5 = GetMaterialId(5);
            var eur10 = GetMaterialId(10);
            var eur20 = GetMaterialId(20);
            var eur50 = GetMaterialId(50);
            var eur100 = GetMaterialId(100);
            var matIds = new List<Dictionary<string, int>> { /*eur5, eur10, eur20, eur50,*/ eur100 };                       
            var dbExecutor = new DataBaseExecutor();
            dbExecutor.OpenConnection();

            try
            {                   
                for (i = 0; i < number; i++)
                {                        
                    sc[i] = FillStockContainerRecord(locationId, stockLocationId, PreannouncementType.CustomerElectronic);
                    rslt = DataBaseHelper.SaveObject(sc[i], dbExecutor.DataBaseParams);
                    if (!rslt.IsSuccess)
                    {
                        throw new InvalidOperationException(rslt.GetMessage());
                    }

                    depositNumberList.Add(sc[i].Number);
                    foreach (var matId in matIds)
                    {
                        var sp = FillStockPositionRecord(matId, sc[i]);                            
                        rslt = DataBaseHelper.SaveObject(sp, dbExecutor.DataBaseParams);
                        if (!rslt.IsSuccess)
                        {
                            throw new InvalidOperationException(rslt.GetMessage());
                        }
                    }

                    var spTotal = FillTotalStockPositionRecord(sc[i]);                        
                    rslt = DataBaseHelper.SaveObject(spTotal, dbExecutor.DataBaseParams);
                    if (!rslt.IsSuccess)
                    {
                        throw new InvalidOperationException(rslt.GetMessage());
                    }
                }
            }

            finally
            {
                dbExecutor.CloseConnection();
            }

            return depositNumberList;
        }

        // Customer transport pre-announcements generation for testing of fast way capturing       
        // first parameter = number of mother pre-announcements
        // second parameter = number of inner pre-announcements for each mother pre-announcement
        // third parameter = Location_From_Id
        // fourth parameter = flag for generation of electronic pre-announcements
        public string[,] GenerateTransportPreannouncements(int motherNumber, int innerNumber, string locationCode, bool electronic)
        {
            string[,] preannouncements = new string[motherNumber, innerNumber + 1];
            int i, j;
            Result rslt;            
            StockContainer[] sc = new StockContainer[motherNumber];
            StockContainer[] innerDeposit = new StockContainer[innerNumber];
            StockContainer[] electronicPreannouncement = new StockContainer[innerNumber];
            var locationId = SettingsHelper.GetMandatoryLocationId(locationCode);
            var stockLocationId = GetStockLocationId(Configuration.StockLocationName, Configuration.StockLocationDescription);
            var eur5 = GetMaterialId(5);
            var eur10 = GetMaterialId(10);
            var eur20 = GetMaterialId(20);
            var eur50 = GetMaterialId(50);
            var eur100 = GetMaterialId(100);
            var matIds = new List<Dictionary<string,int>> { /*eur5, eur10, eur20, eur50,*/ eur100 };            
            var dbExecutor = new DataBaseExecutor();
            dbExecutor.OpenConnection();

            try
            {
                for (i = 0; i < motherNumber; i++)
                {
                    sc[i] = FillStockContainerRecord(locationId, stockLocationId, PreannouncementType.Transport, innerNumber);
                    rslt = DataBaseHelper.SaveObject(sc[i], dbExecutor.DataBaseParams);
                    if (!rslt.IsSuccess)
                    {
                        throw new InvalidOperationException(rslt.GetMessage());
                    }

                    preannouncements[i, 0] = sc[i].Number;
                    for (j = 0; j < innerNumber; j++)
                    {
                        innerDeposit[j] = FillInnerStockContainerRecord(locationId, stockLocationId, sc[i], PreannouncementType.Transport, j);                  
                        rslt = DataBaseHelper.SaveObject(innerDeposit[j], dbExecutor.DataBaseParams);
                        if (!rslt.IsSuccess)
                        {
                            throw new InvalidOperationException(rslt.GetMessage());
                        }

                        preannouncements[i, j + 1] = innerDeposit[j].Number;
                        if (electronic)
                        {
                            electronicPreannouncement[j] = FillInnerStockContainerRecord(locationId, stockLocationId, sc[i], PreannouncementType.CustomerElectronic, j);                          
                            rslt = DataBaseHelper.SaveObject(electronicPreannouncement[j], dbExecutor.DataBaseParams);
                            if (!rslt.IsSuccess)
                            {
                                throw new InvalidOperationException(rslt.GetMessage());
                            }

                            foreach (var matId in matIds)
                            {
                                var sp = FillStockPositionRecord(matId, electronicPreannouncement[j]);
                                rslt = DataBaseHelper.SaveObject(sp, dbExecutor.DataBaseParams);
                                if (!rslt.IsSuccess)
                                {
                                    throw new InvalidOperationException(rslt.GetMessage());
                                }
                            }

                            var spTotal = FillTotalStockPositionRecord(electronicPreannouncement[j]);
                            rslt = DataBaseHelper.SaveObject(spTotal, dbExecutor.DataBaseParams);

                            if (!rslt.IsSuccess)
                            {
                                throw new InvalidOperationException(rslt.GetMessage());
                            }
                        }
                    }
                }
            }
            finally
            {
                dbExecutor.CloseConnection();
            }

            return preannouncements;
        }
        
        // Get Material Id
        private Dictionary<string, int> GetMaterialId(int denomination)
        {
            var whereConditions = new WhereConditions();
            whereConditions.And("Denomination = @denomination", denomination);
            whereConditions.And("curCode = @currency", "EUR");
            whereConditions.And("matTypeCode = @materialtype", "NOTE");
            var dataTable = DataBaseHelper.Select(typeof(Material).GetTableName(), null, whereConditions.Sql, whereConditions.ParamsAsDictionary, null);
            DataRow[] rows = dataTable.Select();
            if (rows != null)
            {
                // "materialID" attribute should be changed after database refactoring!
                var material = new Dictionary<string,int>()
                { 
                    { DataUtils.GetString(rows[0]["materialID"]), denomination }
                };

            return material;
            }

            else throw new Exception("Material has not been found!");
        }
        
        // Get Stock Location Id
        private int GetStockLocationId(string name, string description)
        {
            var whereConditions = new WhereConditions();
            whereConditions.And("ReferenceNumber = @reference_number", name);
            whereConditions.And("Description = @description", description);
            var dataTable = DataBaseHelper.Select(typeof(StockLocation).GetTableName(), null, whereConditions.Sql, whereConditions.ParamsAsDictionary, null);
            DataRow[] rows = dataTable.Select();
            if (rows != null)
            {
                //return Convert.ToInt32(rows[0].ItemArray[dataTable.Columns.IndexOf("id")].ToString());
                return DataUtils.GetInt32(rows[0]["id"]);
            }

            else throw new Exception("Required location has not been found!");
        }

        /// <summary>
        /// Fill electronic pre-announcement data for saving to database
        /// </summary>
        /// <param name="locationId">Location Id</param>
        /// <param name="stockLocationId">Stock Location Id</param>
        /// <returns>Stock Container</returns>
        private StockContainer FillStockContainerRecord(decimal locationId, int stockLocationId, PreannouncementType type, int? innersNumber = null)
        {
            var date = DateTime.Now;
            var sc = new StockContainer();            
            sc.SetNumber("PRE" + date.ToString(dateFormat));
            sc.SetType(StockContainerType.Deposit);
            sc.SetPreannouncementType(type == PreannouncementType.CustomerElectronic ? PreannouncementType.CustomerElectronic : PreannouncementType.Transport);
            sc.ServiceDate = date.Date;
            sc.SetStatus(SealbagStatus.Received);
            sc.TotalQuantity = (type == PreannouncementType.CustomerElectronic) ? 10 : 0;
            sc.TotalValue = (type == PreannouncementType.CustomerElectronic) ? 1000 : 0;
            sc.TotalWeight = (type == PreannouncementType.CustomerElectronic) ? 0.01m : 0;
            sc.LocationFrom_id = locationId;
            sc.StockLocation_id = (type == PreannouncementType.CustomerElectronic) ? stockLocationId : (int?)null;            
            sc.DeclaredTotalInners = innersNumber;
            sc.DateCollected = date.Date;            
            return sc;
        }

        /// <summary>
        /// Fill inner stock container record for saving to database
        /// </summary>
        /// <param name="locationId">Location Id</param>
        /// <param name="stockLocationId">Stock Location Id</param>
        /// <param name="motherDeposit">Mother deposit</param>
        /// <param name="j">Index number</param>
        /// <returns>Inner stock container</returns>
        private StockContainer FillInnerStockContainerRecord(decimal locationId, int stockLocationId, StockContainer motherDeposit, PreannouncementType type, int j)
        {
            var date = DateTime.Now;
            var innerDeposit = new StockContainer();            
            innerDeposit.SetNumber(motherDeposit.Number + "-" + String.Format("{0}", j + 1));
            innerDeposit.SetType(StockContainerType.Deposit);
            innerDeposit.SetPreannouncementType(type == PreannouncementType.CustomerElectronic ? PreannouncementType.CustomerElectronic : PreannouncementType.Transport);
            innerDeposit.ServiceDate = date.Date;
            innerDeposit.SetStatus(SealbagStatus.Received);
            innerDeposit.TotalQuantity = (type == PreannouncementType.CustomerElectronic) ? 10 : 0;
            innerDeposit.TotalValue = (type == PreannouncementType.CustomerElectronic) ? 1000 : 0;
            innerDeposit.TotalWeight = (type == PreannouncementType.CustomerElectronic) ? 0.01m : 0;
            innerDeposit.LocationFrom_id = locationId;
            innerDeposit.StockLocation_id = (type == PreannouncementType.CustomerElectronic) ? stockLocationId : (int?)null;
            innerDeposit.ParentContainer_id = motherDeposit.ID;
            innerDeposit.DateCollected = date.Date;     
            return innerDeposit;
        }

        /// <summary>
        /// Fill stock position/-s per materials for electronic pre-announcements
        /// </summary>
        /// <param name="materialId">Material Id</param>
        /// <param name="matDenominations">Array of material denominations</param>
        /// <param name="matIds">List of materials Ids</param>
        /// <param name="sc">Corresponding customer electronic pre-announcement</param>
        /// <returns></returns>
        private StockPosition FillStockPositionRecord(Dictionary<string,int> materialId, StockContainer sc)
        {
            var sp = new StockPosition();
            sp.SetIsVerified(false);
            sp.QualificationType = QualificationType.Unfit;
            sp.SetStatus(StockPositionStatus.Active);
            sp.Quantity = 10;
            sp.Value = materialId.First().Value * sp.Quantity;
            sp.Weight = 0.01m;
            sp.Material_id = materialId.First().Key;
            sp.StockContainer_id = sc.ID;
            sp.Currency_id = "EUR";
            sp.SetIsTotal(false);
            return sp;
        }

        private StockPosition FillTotalStockPositionRecord(StockContainer sc)
        {
            var spTotal = new StockPosition();
            spTotal.SetIsVerified(false);
            spTotal.QualificationType = QualificationType.Unfit;
            spTotal.SetStatus(StockPositionStatus.Active);
            spTotal.Quantity = 0;
            spTotal.Value = 1000;
            spTotal.Weight = 0.01m;
            spTotal.Material_id = null;
            spTotal.StockContainer_id = sc.ID;
            spTotal.Currency_id = "EUR";
            spTotal.SetIsTotal(true);
            return spTotal;
        }

        /// <summary>
        /// Clearing of generated deposit numbers list
        /// </summary>
        /// <param name="list"> List of deposit numbers </param>
        public void ClearList(List<string> list)
        {
            list.Clear();
        }        
    }
}
