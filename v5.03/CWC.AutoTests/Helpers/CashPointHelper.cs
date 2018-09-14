using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.BaseData.Model;
using Cwc.Coin;
using Cwc.Coin.ConstantNames;
using Cwc.Common;
using Cwc.FeedingsEncryptor;
using Cwc.Security;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web.Configuration;
using Cwc.Assets;
using Cwc.CashCenter.Fakes;
using Cwc.Contracts;
using Microsoft.QualityTools.Testing.Fakes;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CWC.AutoTests.Helpers
{    
    public class CashPointHelper 
    {
        #region DataInit
        CoinMachine cm;
        string number;
        string name;
        int status;
        CashPointType type;
        MachineModel model;
        DateTime dateCreated;
        Location location;
        Customer customer;
        CashPointStockPosition sp;
        Material material;
        Product product;
        DataBaseParams dbParams;
        LoginResult lr;

        public CashPointHelper()
        {
            ConfigurationKeySet.Load();
            dbParams = new DataBaseParams();
            cm = new CoinMachine();
            number = "CM0001";
            name = "strakh_cash_point";
            status = 1;
            lr = new LoginResult(123);
            type = BaseDataFacade.CashPointTypeService.Load(40, dbParams);
            model = CoinFacade.MachineModelService.LoadMachineModel(lr, 15, dbParams);
            location = BaseDataFacade.LocationService.LoadByCode("0611");
            material = BaseDataFacade.MaterialService.Load(308, dbParams);
            product = BaseDataFacade.ProductService.Load(311, dbParams);
            dateCreated = new DateTime(2016, 8, 22, 15, 4, 10);
            customer = BaseDataFacade.CustomerService.LoadCustomerById(3000004180);
        }
        #endregion

        public void CreateCashPoint()
        {
            cm = new CashPointBuilder()
                .WithNumber(number)
                //.WithType(type)
                //.WithModel(model.ID)
                .WithStatus(status)
                .WithDateCreated(dateCreated)
                .WithDateUpdated(dateCreated)
                .WithLocationID(location.ID)
                .WithSupplier(Convert.ToDecimal(customer.Cus_nr))
                .WithOwner(Convert.ToDecimal(customer.Cus_nr))
                .WithName(name)
                .WithOptimization(OrderCreation.Automated)
                .WithIndividualStock(true)
                .WithRemainderOfStock(0)
                .WithOrderMandatory(false)
                .WithForceConfirmation(false)
                .WithReplenishment(Cwc.Coin.ConstantNames.Enums.CoinMachine.ReplenishmentMethod.SwapCash)
                .WithStockExpiration(AllowStockExpirationType.PositionStockExpiration)
                .WithManualTransactions(false)
                .SaveToDb();
        }

        public void CreateStockPositions()
        {
            sp = new StockPositionBuilder()
                //.WithMachineType(type.ID)
                //.WithMachineModel(model.ID)
                .WithType(Cwc.Coin.ConstantNames.Enums.StockPosition.Type.Actual)
                .WithQuantity(0)
                .WithDateCreated(dateCreated)
                .WithDateUpdated(dateCreated)
                .WithMaterial(material.MaterialID)
                //.WithCoinMachine(cm.ID)
                .WithDirection(Direction.Recycle)
                .WithCapacity(1000)
                .WithCassetteNumber(1)
                .WithPriority(Priority.Normal)
                .WithIsGrandTotal(false)
                .WithCurrency("EUR")
                .WithResidualCashPercentage(0)
                .WithIsMixed(false)
                .WithMaterialType("NOTE")
                .WithIsOptimized(true)
                .SaveToDb();

            sp = new StockPositionBuilder()
                //.WithMachineType(type.ID)
                //.WithMachineModel(model.ID)
                .WithType(Cwc.Coin.ConstantNames.Enums.StockPosition.Type.Configuration)
                .WithQuantity(0)
                .WithDateCreated(dateCreated)
                .WithDateUpdated(dateCreated)
                .WithMaterial(material.MaterialID)
                //.WithCoinMachine(cm.ID)
                .WithDirection(Direction.Recycle)
                .WithCapacity(1000)
                .WithCassetteNumber(1)
                .WithPriority(Priority.Normal)
                .WithIsGrandTotal(false)
                .WithCurrency("EUR")
                .WithResidualCashPercentage(0)
                .WithIsMixed(false)
                .WithMaterialType("NOTE")
                .WithIsOptimized(true)
                .SaveToDb();

            sp = new StockPositionBuilder()
                //.WithMachineType(type.ID)
                //.WithMachineModel(model.ID)
                .WithType(Cwc.Coin.ConstantNames.Enums.StockPosition.Type.Actual)
                .WithQuantity(0)
                .WithDateCreated(dateCreated)
                .WithDateUpdated(dateCreated)
                //.WithCoinMachine(cm.ID)
                .WithDirection(Direction.Recycle)
                .WithCapacity(1000)
                .WithCassetteNumber(1)
                .WithPriority(Priority.Normal)
                .WithIsGrandTotal(false)
                .WithCurrency("EUR")
                .WithResidualCashPercentage(0)
                .WithIsMixed(true)
                .WithMaterialType("NOTE")
                .WithIsOptimized(true)
                .SaveToDb();

            sp = new StockPositionBuilder()
                //.WithMachineType(type.ID)
                //.WithMachineModel(model.ID)
                .WithType(Cwc.Coin.ConstantNames.Enums.StockPosition.Type.Configuration)
                .WithQuantity(0)
                .WithDateCreated(dateCreated)
                .WithDateUpdated(dateCreated)
                //.WithCoinMachine(cm.ID)
                .WithDirection(Direction.Recycle)
                .WithCapacity(1000)
                .WithCassetteNumber(1)
                .WithPriority(Priority.Normal)
                .WithIsGrandTotal(false)
                .WithCurrency("EUR")
                .WithResidualCashPercentage(0)
                .WithIsMixed(true)
                .WithMaterialType("NOTE")
                .WithIsOptimized(true)
                .SaveToDb();
        }

        public void SendCashPointTransactionFeeding(string xml)
        {
            this.SendFeeding(xml);
        }

        private void SendFeeding(string content)
        {
            var login = "admin";
            var password = "sa";

            System.Security.Cryptography.SHA1Managed sha1 = new System.Security.Cryptography.SHA1Managed();
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(password);
            byte[] hash = sha1.ComputeHash(bytes);
            password = Convert.ToBase64String(hash);

            string sendPassword = FeedingsEncryptorFacade.UniqueInstance.GetHash64(content);

            content = FeedingsEncryptorFacade.UniqueInstance.GetEncryptedXML(content, login, password);
            string sendHash = FeedingsEncryptorFacade.UniqueInstance.GetHash64(content);
            string sendContent = content;

            using (var service = new WebServiceFeedings.WebServiceFeedingsSoapClient())
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var x = service.SendFeedingAsync(login, sendPassword, sendHash, content);
                Debug.WriteLine("Thread id: {0}. Elapsed: {1}", Thread.CurrentThread.ManagedThreadId, stopwatch.ElapsedMilliseconds);
            }
        }

        public bool VerifyEntity(string tableName, Dictionary<string, object> dataToVerify)
        {
            using (var sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebPortalConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var dataAdapter = new SqlDataAdapter(String.Format(@"SELECT * FROM {0}
                                                                            WHERE {1} = @Param", 
                                                                            tableName, dataToVerify.First().Key), sqlConnection))
                {
                    dataAdapter.SelectCommand.Parameters.Add(new SqlParameter("Param", dataToVerify.First().Value));
                    var dt = new DataTable();
                    dataAdapter.Fill(dt);
                    return (dt.Rows.Count > 0);
                }
            }

            
        }

        public string FindRelatedTransactionLine(Dictionary<string, object> dataToVerify)
        {
            using (var sqlConnection = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebPortalConnectionString"].ConnectionString))
            {
                sqlConnection.Open();
                using (var dataAdapter = new SqlDataAdapter(String.Format(@"SELECT [TL].id FROM WP_CM_TransactionLine AS [TL]
                                                                            INNER JOIN WP_CM_Transaction AS [T] ON [T].id = [TL].Transaction_id
                                                                            WHERE [T].{0} = '{1}'",
                                                                            dataToVerify.First().Key,
                                                                            dataToVerify.First().Value), sqlConnection))
                {
                    var dt = new DataTable();
                    dataAdapter.Fill(dt);
                    return dt.Rows.ToString();
                }
            }
        }

        public void DeleteEntity(string tableName, Dictionary<string, object> idColumnData)
        {
            DataBaseHelper.Delete(tableName, String.Format("{0} = @{0}", idColumnData.First().Key), idColumnData, null);
        }
    }
}
