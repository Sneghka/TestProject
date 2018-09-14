using Cwc.Ordering;
using Cwc.Replication;
using CWC.AutoTests.DataModel;
using CWC.AutoTests.Enums;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.BasicImport
{
    public class BasicImportTests : IClassFixture<BasicImportFixture>, IDisposable
    {        
        private Dictionary<string, object> idColumnData;
        private BasicImportHelper basicImportHelper;
        private string tableName;
        protected string branch_cd, CitCode,code, referenceNumber, cus_nr;

        public BasicImportTests(BasicImportFixture setupFixture)
        {            
            idColumnData = new Dictionary<string, object>();
            basicImportHelper = new BasicImportHelper(setupFixture.FolderPath);
            branch_cd = "LOS";
            CitCode = "ttt";
            code = $"1101{new Random().Next(4000, 9999)}";
            referenceNumber = $"1101{new Random().Next(4000, 9999)}";
            cus_nr = SettingsHelper.GetCustomerRefNr();
        }

        public void Dispose()
        {
            basicImportHelper.DeleteEntity(tableName, idColumnData);
        }
                
        /// <summary>
        /// Service Order can be created via Basic Import!
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create new service order")]
        public void CreateServiceOrder()
        {            
            string orderId = "JG" + DateTime.Now.ToString("ddMMyyyy");
            string genericStatus = "REGISTERED";
            int genericStatusNumber = 1;
            string serviceDate = DateTime.Today.ToString("yyyy-MM-dd 00:00:00");
            tableName = String.Concat("dbo.", BasicImportEntity.ServiceOrder);
            idColumnData.Add("Order_ID", orderId);
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?> 
                                        <DocumentElement>
                                            <ServiceOrder act='0'>
                                            <WP_branch_cd>JG</WP_branch_cd>
                                            <Order_ID>{0}</Order_ID>
                                            <Cus_nr>3303</Cus_nr>
                                            <Service_Date>{1}</Service_Date>
                                            <Order_Status>{2}</Order_Status>
                                            <Order_Type>2</Order_Type>
                                            <Order_Level>1</Order_Level>
                                            <WP_loc_nr>3000000026</WP_loc_nr>
                                            <WP_ref_loc_nr>JG02</WP_ref_loc_nr>
                                            <WP_CurrencyCode>EUR</WP_CurrencyCode>                                                
                                            <WP_ServiceType_Code>COLL</WP_ServiceType_Code>
                                            <WP_DateCreated>{1}</WP_DateCreated>
                                            </ServiceOrder>
                                        </DocumentElement>", orderId, serviceDate, genericStatus);
            basicImportHelper.SaveXml(BasicImportEntity.ServiceOrder, content);
            basicImportHelper.Import();
            var dataToVerify = new Dictionary<string, object>();            
            dataToVerify.Add("Cus_nr",3000005788);
            dataToVerify.Add("WP_GenericStatus", genericStatusNumber);                        
            dataToVerify.Add("Service_Date", serviceDate);
            dataToVerify.Add("Order_ID", orderId);
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Service order is imported with errors!");
        }

        /// <summary>
        /// Service order without currency is imported with default currency
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create new service order without currency")]
        public void CreateServiceOrderWithoutCurrency()
        {
            string orderId = "JG" + DateTime.Now.ToString("ddMMyyyy");
            tableName = String.Concat("dbo.", BasicImportEntity.ServiceOrder);            
            idColumnData.Add("Order_ID", orderId);
            var dataToVerify = new Dictionary<string, object>();
            dataToVerify.Add("WP_CurrencyCode", "EUR");
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?> 
                                            <DocumentElement>
                                              <ServiceOrder act='0'>
                                                <WP_branch_cd>JG</WP_branch_cd>
                                                <Order_ID>{0}</Order_ID>
                                                <Cus_nr>3000005788</Cus_nr>
                                                <Service_Date>{1}</Service_Date>
                                                <Order_Status>REGISTERED</Order_Status>
                                                <Order_Type>2</Order_Type>
                                                <Order_Level>1</Order_Level>
                                                <WP_loc_nr>3000000026</WP_loc_nr>
                                                <WP_ref_loc_nr>JG02</WP_ref_loc_nr>
                                                <WP_CurrencyCode></WP_CurrencyCode>
                                                <WP_GenericStatus>6</WP_GenericStatus>
                                                <WP_ServiceType_Code>COLL</WP_ServiceType_Code>
                                                <WP_DateCreated>{1}</WP_DateCreated>
                                              </ServiceOrder>
                                            </DocumentElement>", orderId, DateTime.Today.ToString("yyyy-MM-dd 00:00:00"));            
            basicImportHelper.SaveXml(BasicImportEntity.ServiceOrder, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Currency was not defined properly!");            
        }

        /// <summary>
        /// WP_loc_nr is defined correctly when order is imported with ref_loc_nr only
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create service order without WP_loc_nr")]
        public void CreateServiceOrderWithoutWP_loc_nr()
        {
            string orderId = "JG" + DateTime.Now.ToString("ddMMyyyy");
            string date = DateTime.Today.ToString("yyyy-MM-dd 00:00:00");
            string code = "JG02";
            var loc_nr = SettingsHelper.GetMandatoryLocationId(code);
            tableName = String.Concat("dbo.", BasicImportEntity.ServiceOrder);            
            idColumnData.Add("Order_ID", orderId);
            var dataToVerify = new Dictionary<string, object>();
            dataToVerify.Add("WP_loc_nr", loc_nr);
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?> 
                                            <DocumentElement>
                                              <ServiceOrder act='0'>
                                                <WP_branch_cd>JG</WP_branch_cd>
                                                <Order_ID>{0}</Order_ID>
                                                <Cus_nr>3000005788</Cus_nr>
                                                <Service_Date>{1}</Service_Date>
                                                <Order_Status>REGISTERED</Order_Status>
                                                <Order_Type>2</Order_Type>
                                                <Order_Level>1</Order_Level>                                                
                                                <WP_ref_loc_nr>{2}</WP_ref_loc_nr>
                                                <WP_CurrencyCode></WP_CurrencyCode>
                                                <WP_GenericStatus>6</WP_GenericStatus>
                                                <WP_ServiceType_Code>COLL</WP_ServiceType_Code>
                                                <WP_DateCreated>{1}</WP_DateCreated>
                                                <WP_DateUpdated>{1}</WP_DateUpdated>
                                              </ServiceOrder>
                                            </DocumentElement>", orderId, date, code);            
            basicImportHelper.SaveXml(BasicImportEntity.ServiceOrder, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "WP_loc_nr was not defined properly!");            
        }

        /// <summary>
        /// Service Order can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Update service order")]
        public void UpdateServiceOrder()
        {
            string orderId = "JG" + DateTime.Now.ToString("ddMMyyyy");
            tableName = String.Concat("dbo.", BasicImportEntity.ServiceOrder);            
            idColumnData.Add("Order_ID", orderId);
            var dataToVerify = new Dictionary<string, object>()
            {
                { "WP_GenericStatus", GenericStatus.Cancelled }
            };
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?> 
                                            <DocumentElement>
                                              <ServiceOrder act='0'>
                                                <WP_branch_cd>JG</WP_branch_cd>
                                                <Order_ID>{0}</Order_ID>
                                                <Cus_nr>3000005788</Cus_nr>
                                                <Service_Date>{1}</Service_Date>
                                                <Order_Status>REGISTERED</Order_Status>
                                                <Order_Type>2</Order_Type>
                                                <Order_Level>1</Order_Level>
                                                <WP_loc_nr>3000000026</WP_loc_nr>
                                                <WP_ref_loc_nr>JG02</WP_ref_loc_nr>
                                                <WP_CurrencyCode>EUR</WP_CurrencyCode>
                                                <WP_ServiceType_Code>COLL</WP_ServiceType_Code>
                                                <WP_DateCreated>{1}</WP_DateCreated>
                                              </ServiceOrder>
                                            </DocumentElement>", orderId, DateTime.Today.ToString("yyyy-MM-dd 00:00:00"));            
            basicImportHelper.SaveXml(BasicImportEntity.ServiceOrder, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Service order is imported with errors!");
            content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?> 
                                            <DocumentElement>
                                              <ServiceOrder act='1'>
                                                <WP_branch_cd>JG</WP_branch_cd>
                                                <Order_ID>{0}</Order_ID>
                                                <Cus_nr>3000005788</Cus_nr>
                                                <Service_Date>{1}</Service_Date>
                                                <Order_Status>CANCELLED</Order_Status>
                                                <Order_Type>2</Order_Type>
                                                <Order_Level>1</Order_Level>
                                                <WP_loc_nr>3000000026</WP_loc_nr>
                                                <WP_ref_loc_nr>JG02</WP_ref_loc_nr>
                                                <WP_CurrencyCode>EUR</WP_CurrencyCode>
                                                <WP_ServiceType_Code>COLL</WP_ServiceType_Code>
                                                <WP_DateCreated>{1}</WP_DateCreated>
                                              </ServiceOrder>
                                            </DocumentElement>", orderId, DateTime.Today.ToString("yyyy-MM-dd 00:00:00"));
            basicImportHelper.SaveXml(BasicImportEntity.ServiceOrder, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Service order was not updated successfully!");            
        }

        /// <summary>
        /// Service order line can be created via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create new service order line")]
        public void CreateSOline()
        {
            string orderId = "JG" + DateTime.Now.ToString("ddMMyyyy");
            string orderLineId = orderId + "-1";
            tableName = String.Concat("dbo.", BasicImportEntity.SOline);            
            idColumnData.Add("Orderline_ID", orderLineId);
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                        <DocumentElement>
                                          <SOline act='0'>
                                            <OrderLine_ID>{1}</OrderLine_ID>
                                            <Order_ID>{0}</Order_ID>
                                            <Orderline_status>COMPLETED</Orderline_status>
                                            <loc_nr>3000000026</loc_nr>    
                                            <mast_cd>JHB04</mast_cd>
                                            <Day_nr>1</Day_nr>
                                            <Dai_date>{2}</Dai_date>
                                            <a_time>1111</a_time>
                                            <Visit_Sequence>1</Visit_Sequence>
                                            <branch_nr>20</branch_nr>
                                            <branch_cd>JG</branch_cd>
                                            <Serv_type>Collect</Serv_type>
                                            <Revenue>30</Revenue>
                                            <Orderline_value>10000</Orderline_value>
                                          </SOline>  
                                        </DocumentElement>", orderId, orderLineId, DateTime.Today.ToString("yyyy-MM-dd 00:00:00"));            
            basicImportHelper.SaveXml(BasicImportEntity.SOline, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Service order line is imported with errors!");            
        }

        /// <summary>
        /// Service order line can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Update service order line")]
        public void UpdateSOline()
        {
            string orderId = "JG" + DateTime.Now.ToString("ddMMyyyy");
            string orderLineId = orderId + "-1";
            tableName = String.Concat("dbo.", BasicImportEntity.SOline);            
            idColumnData.Add("Orderline_ID", orderLineId);
            var dataToVerify = new Dictionary<string, object>()
            {
                { "Loc_nr", 3000000025 },
                { "mast_cd", DBNull.Value },                
                { "Dai_date", DateTime.Today.AddDays(1).ToString("yyyy-MM-dd 00:00:00") }
            };
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                        <DocumentElement>
                                          <SOline act='0'>
                                            <OrderLine_ID>{1}</OrderLine_ID>
                                            <Order_ID>{0}</Order_ID>
                                            <Orderline_status>REGISTERED</Orderline_status>
                                            <Loc_nr>3000000026</Loc_nr>    
                                            <mast_cd>JHB04</mast_cd>
                                            <Day_nr>1</Day_nr>
                                            <Dai_date>{2}</Dai_date>
                                            <a_time>1111</a_time>
                                            <Visit_Sequence>1</Visit_Sequence>
                                            <branch_nr>20</branch_nr>
                                            <branch_cd>JG</branch_cd>
                                            <Serv_type>Collect</Serv_type>
                                            <Revenue>30</Revenue>
                                            <Orderline_value>10000</Orderline_value>                                            
                                          </SOline>  
                                        </DocumentElement>", orderId, orderLineId, DateTime.Today.ToString("yyyy-MM-dd 00:00:00"));            
            basicImportHelper.SaveXml(BasicImportEntity.SOline, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Service order line is imported with errors!");
            content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                        <DocumentElement>
                                          <SOline act='1'>
                                            <OrderLine_ID>{1}</OrderLine_ID>
                                            <Order_ID>{0}</Order_ID>
                                            <Orderline_status>COMPLETED</Orderline_status>
                                            <Loc_nr>3000000025</Loc_nr>    
                                            <mast_cd></mast_cd>
                                            <Day_nr>1</Day_nr>
                                            <Dai_date>{2}</Dai_date>
                                            <a_time>1111</a_time>
                                            <Visit_Sequence>1</Visit_Sequence>
                                            <branch_nr>20</branch_nr>
                                            <branch_cd>JG</branch_cd>
                                            <Serv_type>Collect</Serv_type>
                                            <Revenue>30</Revenue>
                                            <Orderline_value>10000</Orderline_value>                                            
                                          </SOline>  
                                        </DocumentElement>", orderId, orderLineId, DateTime.Today.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));            
            basicImportHelper.SaveXml(BasicImportEntity.SOline, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Service order line is imported with errors!");            
        }

        /// <summary>
        /// Picked service order product can be imported via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create new picked service order product")]
        public void CreatePickedSOProduct()
        {
            string orderLineId = "JG" + DateTime.Now.ToString("ddMMyyyy") + "-1";
            tableName = String.Concat("dbo.", BasicImportEntity.SOProduct);            
            idColumnData.Add("Orderline_ID", orderLineId);
            var content = String.Format(@"<?xml version='1.0' standalone='yes'?>
                                            <DocumentElement>
                                             <SOProduct act='0'>
                                             <Orderline_ID>{0}</Orderline_ID>
                                             <ProductCode>23</ProductCode>
                                             <OrderProduct_Number>1</OrderProduct_Number>
                                             <OrderProduct_Value>10000</OrderProduct_Value>
                                             <Actual_Number>1</Actual_Number>
                                             <Actual_Value>10000</Actual_Value>                                             
                                             <WP_count_date>{1}</WP_count_date>                                             
                                             <WP_pack_nr>{0}</WP_pack_nr>
                                             <WP_total_only>0</WP_total_only>	
                                             <WP_reject>0</WP_reject>	                                             
                                             <WP_ref_loc_nr>JG02</WP_ref_loc_nr>
                                             <WP_Picked_Qty>1</WP_Picked_Qty>					
                                             <WP_Picked_Value>10000</WP_Picked_Value>			
                                             <WP_Picked_Date>{1}</WP_Picked_Date>	
                                             <Currency>EUR</Currency>	 
                                             </SOProduct>
                                            </DocumentElement>", orderLineId, DateTime.Today.ToString("yyyy-MM-dd 00:00:00"));            
            basicImportHelper.SaveXml(BasicImportEntity.SOProduct, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Service order product is imported with errors!");            
        }

        /// <summary>
        /// Pre-announced service order product can be imported via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create new pre-announced service order product")]
        public void CreatePreannouncedSOProduct()
        {
            string orderLineId = "JG" + DateTime.Now.ToString("ddMMyyyy") + "-1";            
            tableName = String.Concat("dbo.", BasicImportEntity.SOProduct);            
            idColumnData.Add("Orderline_ID", orderLineId);
            var content = String.Format(@"<?xml version='1.0' standalone='yes'?>
                                            <DocumentElement>
                                             <SOProduct act='0'>
                                             <Orderline_ID>{0}</Orderline_ID>                                             
                                             <Actual_Number>1</Actual_Number>
                                             <Actual_Value>10000</Actual_Value>
                                             <WP_prean_qty>1</WP_prean_qty>
                                             <WP_prean_value>10000</WP_prean_value>
                                             <WP_material_id>310</WP_material_id>
                                             <WP_count_date>{1}</WP_count_date>
                                             <WP_booking_date>{1}</WP_booking_date>
                                             <WP_pack_nr>{0}</WP_pack_nr>
                                             <WP_total_only>0</WP_total_only>	
                                             <WP_reject>0</WP_reject>	                                             
                                             <WP_ref_loc_nr>JG02</WP_ref_loc_nr>                                             	
                                             <Currency>EUR</Currency>	 
                                             </SOProduct>
                                            </DocumentElement>", orderLineId, DateTime.Today.ToString("yyyy-MM-dd 00:00:00"));            
            basicImportHelper.SaveXml(BasicImportEntity.SOProduct, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Service order product is imported with errors!");
            basicImportHelper.DeleteEntity(tableName, idColumnData);
        }

        /// <summary>
        /// Customer can be created via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create new customer")]
        public void CreateCustomer()
        {
            tableName = String.Concat("dbo.", BasicImportEntity.Customer);            
            idColumnData.Add("ReferenceNumber", referenceNumber);
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                        <DocumentElement>
                                          <customer act='0'>                                            
                                            <cus_nr>{0}</cus_nr>                                            
                                            <name>Test Customer 01</name>
                                            <abbrev>AUTOTEST01</abbrev>
                                            <csgrp_cd>TEST</csgrp_cd>
                                            <area_pc></area_pc>
                                            <det_pc></det_pc>
                                            <deb_nr></deb_nr>
                                            <msCode></msCode>
                                            <internal></internal>
                                            <bnk_id>0</bnk_id>
                                            <order_level>0</order_level>
                                            <askSignature></askSignature>
                                            <WP_RecordType>customer</WP_RecordType>
                                          </customer>
                                        </DocumentElement>", referenceNumber);            
            basicImportHelper.SaveXml(BasicImportEntity.Customer, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Customer is imported with errors!");            
        }

        /// <summary>
        /// Customer can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Update customer")]
        public void UpdateCustomer()
        {
            string name = "Auto Test Customer Edited";
            tableName = String.Concat("dbo.", BasicImportEntity.Customer);            
            idColumnData.Add("ReferenceNumber", referenceNumber);
            var dataToVerify = new Dictionary<string, object>()
            {
                { "name", name },
                { "enabled", false }
            };
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                        <DocumentElement>
                                          <customer act='0'>                                            
                                            <cus_nr>{0}</cus_nr>
                                            <ReferenceNumber>{0}</ReferenceNumber>
                                            <name>Auto Test Customer 01</name>
                                            <abbrev>AUTOTEST01</abbrev>
                                            <csgrp_cd>TEST</csgrp_cd>
                                            <area_pc></area_pc>
                                            <det_pc></det_pc>
                                            <deb_nr></deb_nr>
                                            <msCode></msCode>
                                            <internal></internal>
                                            <bnk_id>0</bnk_id>
                                            <order_level>0</order_level>
                                            <askSignature></askSignature>
                                            <WP_RecordType>customer</WP_RecordType>
                                          </customer>
                                        </DocumentElement>", referenceNumber);            
            basicImportHelper.SaveXml(BasicImportEntity.Customer, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Customer is imported with errors!");
            content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                        <DocumentElement>
                                          <customer act='1'>                                            
                                            <cus_nr>{0}</cus_nr>
                                            <ReferenceNumber>{0}</ReferenceNumber>
                                            <name>{1}</name>
                                            <abbrev>AUTOTEST01</abbrev>
                                            <csgrp_cd>TEST</csgrp_cd>
                                            <area_pc></area_pc>
                                            <det_pc></det_pc>
                                            <deb_nr></deb_nr>
                                            <msCode></msCode>
                                            <internal></internal>
                                            <bnk_id>0</bnk_id>
                                            <order_level>0</order_level>
                                            <askSignature></askSignature>
                                            <WP_RecordType>customer</WP_RecordType>
                                            <enabled>false</enabled>
                                          </customer>
                                        </DocumentElement>", referenceNumber, name);
            basicImportHelper.SaveXml(BasicImportEntity.Customer, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Customer has not been updated successfully!");            
        }

        /// <summary>
        /// Location can be created via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create new location")]
        public void CreateLocation()
        {
            using (var context = new AutomationTransportDataContext())
            {
                decimal loc_nr = new Random().Next(40000, 99999); /* random loc_nr */
                tableName = String.Concat("dbo.", BasicImportEntity.Location);
                idColumnData.Add("ref_loc_nr", code);
                var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>                                        
                                            <DocumentElement>
                                              <location act='0'>
                                                <abbrev>{0}</abbrev>                                            
                                                <ref_loc_nr>{0}</ref_loc_nr>                                            
                                                <loc_nr>{2}</loc_nr>
                                                <cus_nr>{1}</cus_nr>
                                                <branch_nr>1</branch_nr>
                                                <name>Auto Test Location</name>
                                                <lctyp_cd>NOR</lctyp_cd>
                                                <branch_nr>1</branch_nr>
                                                <enabled>true</enabled>                                            
                                                <ltCode>1</ltCode>
                                              </location>
                                            </DocumentElement>", code, cus_nr, loc_nr);
                try
                {
                    basicImportHelper.SaveXml(BasicImportEntity.Location, content);
                    basicImportHelper.Import();
                    Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Location is imported with errors!");
                }
                finally
                {
                    context.CitProcessSettingLinks.RemoveRange(context.CitProcessSettingLinks.Where(l => l.LocationID == loc_nr).ToList());
                    context.SaveChanges();
                }
            }        
        }

        /// <summary>
        /// Location can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Update location")]
        public void UpdateLocation()
        {
            using (var context = new AutomationTransportDataContext())
            {
                decimal loc_nr = 9900000025; /* random loc_nr */
                string newName = "Auto Test Location Edited";
                tableName = String.Concat("dbo.", BasicImportEntity.Location);
                idColumnData.Add("ref_loc_nr", code);
                var dataToVerify = new Dictionary<string, object>()
                {
                    { "name", newName },
                    { "enabled", false }
                };
                var createContent = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>                                        
                                            <DocumentElement>
                                              <location act='0'>
                                                <abbrev>{0}</abbrev>                                            
                                                <ref_loc_nr>{0}</ref_loc_nr>                                            
                                                <loc_nr>{2}</loc_nr>
                                                <cus_nr>{1}</cus_nr>
                                                <branch_nr>1</branch_nr>
                                                <name>Auto Test Location</name>
                                                <lctyp_cd>NOR</lctyp_cd>
                                                <enabled>true</enabled>                                            
                                                <!--<ltCode></ltCode>-->
                                              </location>
                                            </DocumentElement>", code, cus_nr, loc_nr);
                var updateContent = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>                                        
                                        <DocumentElement>
                                          <location act='1'>
                                            <abbrev>{0}</abbrev>                                            
                                            <ref_loc_nr>{0}</ref_loc_nr>                                            
                                            <loc_nr>{3}</loc_nr>
                                            <cus_nr>{2}</cus_nr>
                                            <branch_nr>1</branch_nr>
                                            <name>{1}</name>
                                            <lctyp_cd>NOR</lctyp_cd>
                                            <enabled>false</enabled>
                                            <!--<ltCode></ltCode>-->
                                          </location>
                                        </DocumentElement>", code, newName, cus_nr, loc_nr);
                try
                {
                    basicImportHelper.SaveXml(BasicImportEntity.Location, createContent);
                    basicImportHelper.Import();
                    Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Location is imported with errors!");

                    basicImportHelper.SaveXml(BasicImportEntity.Location, updateContent);
                    basicImportHelper.Import();
                    Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Location has not been updated successfully!");
                }
                finally
                {
                    context.CitProcessSettingLinks.RemoveRange(context.CitProcessSettingLinks.Where(l => l.LocationID == loc_nr).ToList());
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Branch can be created via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create branch")]
        public void CreateBranch()
        {
            int branch_nr = 9901;         
            tableName = String.Concat("dbo.", BasicImportEntity.Branch);            
            idColumnData.Add("branch_nr", branch_nr);            
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>                                        
                                        <DocumentElement>
                                          <branch act='0'>
                                            <branch_nr>{0}</branch_nr>
                                            <descript>Test branch</descript>
                                            <branch_cd>TST</branch_cd>
                                            <loc_nr>1999990001</loc_nr>
                                            <ref_loc_nr>1999990001</ref_loc_nr>
                                          </branch>
                                        </DocumentElement>", branch_nr);            
            basicImportHelper.SaveXml(BasicImportEntity.Branch, content);
            basicImportHelper.Import();            
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Branch is imported with errors!");            
        }

        /// <summary>
        /// Branch can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Update branch")]
        public void UpdateBranch()
        {
            int branch_nr = 9901;
            string description = "Test branch 01";
            tableName = String.Concat("dbo.", BasicImportEntity.Branch);            
            idColumnData.Add("branch_nr", branch_nr);
            var dataToVerify = new Dictionary<string, object>()
            {
                { "descript", description }
            };
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>                                        
                                        <DocumentElement>
                                          <branch act='0'>
                                            <branch_nr>{0}</branch_nr>
                                            <descript>Test branch</descript>
                                            <branch_cd>TST</branch_cd>
                                            <loc_nr>1999990001</loc_nr>
                                            <ref_loc_nr>1999990001</ref_loc_nr>
                                          </branch>
                                        </DocumentElement>", branch_nr);            
            basicImportHelper.SaveXml(BasicImportEntity.Branch, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData), "Location is imported with errors!");                    
            content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>                                        
                                        <DocumentElement>
                                          <branch act='1'>
                                            <branch_nr>{0}</branch_nr>
                                            <descript>{1}</descript>
                                            <branch_cd>TST</branch_cd>
                                            <loc_nr>1999990001</loc_nr>
                                            <ref_loc_nr>1999990001</ref_loc_nr>
                                          </branch>
                                        </DocumentElement>", branch_nr, description);
            basicImportHelper.SaveXml(BasicImportEntity.Branch, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Branch has not been updated successfully!");
        }


        /// <summary>
        /// Daily Route can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create Daily Route with citcode")]

        public void VerifyThatDailyRouteCanBeCreatedWithCitCode()
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd");
            tableName = String.Concat("dbo.", BasicImportEntity.daily);
            idColumnData.Add("branch_cd", branch_cd);
            var dataToVerify = new Dictionary<string, object>()
            {
                { "branch_cd", branch_cd }
            };
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                        <DocumentElement>
                                          <daily act='0'>
                                            <branch_cd>{0}</branch_cd>
                                            <compl_lg>true</compl_lg>
                                            <conf_lg>true</conf_lg>
                                            <dai_date>{1}</dai_date>
                                            <day_nr>5</day_nr>
                                            <mast_nr>1</mast_nr>
                                            <on_rt_lg>true</on_rt_lg>
                                            <truck_nr>100</truck_nr>
                                            <s_km>900</s_km>
                                            <e_km>950</e_km>
                                            <t_km>50</t_km>
                                            <p_km>0</p_km>
                                            <a_s_date>{1}</a_s_date>
                                            <a_s_time>093833</a_s_time>
                                            <tib_date>{1}</tib_date>
                                            <tib_time>093831</tib_time>
                                            <a_e_date>{1}</a_e_date>
                                            <a_e_time>094020</a_e_time>
                                            <a_d_time>0002</a_d_time>
                                            <p_e_date>{1}</p_e_date>
                                            <p_d_time>1505</p_d_time>
                                            <p_e_time>2305</p_e_time>
                                            <p_s_date>{1}</p_s_date>
                                            <p_s_time>0800</p_s_time>
                                            <upld_date>{1}</upld_date>
                                            <upld_time>093644</upld_time>
                                            <dwnl_date>{1}</dwnl_date>
                                            <dwnl_time>094130</dwnl_time>
                                            <gen_date>{1}</gen_date>
                                            <gen_time>093510</gen_time>
                                            <mast_cd>LOS1001</mast_cd>
                                            <fixed>false</fixed>
                                        	<citcode>{2}</citcode>
                                          </daily>
                                        </DocumentElement>", branch_cd, date, CitCode);

            basicImportHelper.SaveXml(BasicImportEntity.daily, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Daily Route is imported with errors!");
        }

        /// <summary>
        /// Daily Route can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Update Daily Route with citcode")]

        public void VerifyThatDailyRouteCanBeUpdatedWithCitCode()
        {
            var date = DateTime.Now;
            tableName = String.Concat("dbo.", BasicImportEntity.daily);
            idColumnData.Add("branch_cd", branch_cd);
            var dataToVerify = new Dictionary<string, object>()
                {
                    { "branch_cd", branch_cd }
                };
            var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                            <DocumentElement>
                                              <daily act='0'>
                                                <branch_cd>{0}</branch_cd>
                                                <compl_lg>true</compl_lg>
                                                <conf_lg>true</conf_lg>
                                                <dai_date>{1}</dai_date>
                                                <day_nr>5</day_nr>
                                                <mast_nr>1</mast_nr>
                                                <on_rt_lg>true</on_rt_lg>
                                                <truck_nr>100</truck_nr>
                                                <s_km>900</s_km>
                                                <e_km>950</e_km>
                                                <t_km>50</t_km>
                                                <p_km>0</p_km>
                                                <a_s_date>{1}</a_s_date>
                                                <a_s_time>093833</a_s_time>
                                                <tib_date>{1}</tib_date>
                                                <tib_time>093831</tib_time>
                                                <a_e_date>{1}</a_e_date>
                                                <a_e_time>094020</a_e_time>
                                                <a_d_time>0002</a_d_time>
                                                <p_e_date>{1}</p_e_date>
                                                <p_d_time>1505</p_d_time>
                                                <p_e_time>2305</p_e_time>
                                                <p_s_date>{1}</p_s_date>
                                                <p_s_time>0800</p_s_time>
                                                <upld_date>{1}</upld_date>
                                                <upld_time>093644</upld_time>
                                                <dwnl_date>{1}</dwnl_date>
                                                <dwnl_time>094130</dwnl_time>
                                                <gen_date>{1}</gen_date>
                                                <gen_time>093510</gen_time>
                                                <mast_cd>LOS1001</mast_cd>
                                                <fixed>false</fixed>
                                            	<citcode>aaa</citcode>
                                              </daily>
                                            </DocumentElement>", branch_cd, date.ToString("yyyy-MM-dd"));

            basicImportHelper.SaveXml(BasicImportEntity.daily, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Daily Route is imported with errors!");
            content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                            <DocumentElement>
                                              <daily act='0'>
                                                <branch_cd>{0}</branch_cd>
                                                <compl_lg>true</compl_lg>
                                                <conf_lg>true</conf_lg>
                                                <dai_date>{1}</dai_date>
                                                <day_nr>4</day_nr>
                                                <mast_nr>1</mast_nr>
                                                <on_rt_lg>true</on_rt_lg>
                                                <truck_nr>100</truck_nr>
                                                <s_km>900</s_km>
                                                <e_km>950</e_km>
                                                <t_km>50</t_km>
                                                <p_km>0</p_km>
                                                <a_s_date>{1}</a_s_date>
                                                <a_s_time>093833</a_s_time>
                                                <tib_date>{1}</tib_date>
                                                <tib_time>093831</tib_time>
                                                <a_e_date>{1}</a_e_date>
                                                <a_e_time>094020</a_e_time>
                                                <a_d_time>0002</a_d_time>
                                                <p_e_date>{1}</p_e_date>
                                                <p_d_time>1505</p_d_time>
                                                <p_e_time>2305</p_e_time>
                                                <p_s_date>{1}</p_s_date>
                                                <p_s_time>0800</p_s_time>
                                                <upld_date>{1}</upld_date>
                                                <upld_time>093644</upld_time>
                                                <dwnl_date>{1}</dwnl_date>
                                                <dwnl_time>094130</dwnl_time>
                                                <gen_date>{1}</gen_date>
                                                <gen_time>093510</gen_time>
                                                <mast_cd>LOS1001</mast_cd>
                                                <fixed>false</fixed>
                                            	<citcode>{2}</citcode>
                                              </daily>
                                            </DocumentElement>", branch_cd, date.AddDays(1).ToString("yyyy-MM-dd"), CitCode);

            basicImportHelper.SaveXml(BasicImportEntity.daily, content);
            basicImportHelper.Import();
            Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Daily Route has not been updated successfully!");
        }

        /// <summary>
        /// Daily Stop can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create Daily Stop with citcode")]

        public void VerifyThatDailyStopCanBeCreatedWithCitCode()
        {
            var replicationParty = DataFacade.ReplicationParty.Take(rp => rp.PartyType == (int)PartyType.Local);
            var role = replicationParty.Build().Role;
            var siteIdentifier = replicationParty.Build().SiteIdentifier;
            var citIdentifier = "test";
            try
            {
                replicationParty.With_Role(ReplicationRole.CIT).With_SiteIdentifier(citIdentifier).SaveToDb();

                var date = DateTime.Now.ToString("yyyy-MM-dd");
                tableName = String.Concat("dbo.", BasicImportEntity.dai_line);
                idColumnData.Add("branch_cd", branch_cd);
                var dataToVerify = new Dictionary<string, object>()
                {
                    { "CitIdentifier", citIdentifier }
                };
                var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                            <DocumentElement>
                                              <dai_line act='0'>
                                                <branch_cd>{0}</branch_cd>
                                                <a_time>1111</a_time>
                                                <a_time_a>111131</a_time_a>
                                                <amt_col_p>0</amt_col_p>
                                                <amt_col_a>0</amt_col_a>
                                                <amt_del>0</amt_del>
                                                <amt_del_a>0</amt_del_a>
                                                <dai_date>{1}</dai_date>
                                                <day_nr>1</day_nr>
                                                <inv_lg>false</inv_lg>
                                                <loc_nr>3000000026</loc_nr>
                                                <loc_cd>ATM</loc_cd>
                                                <lcrep_nr>0</lcrep_nr>
                                                <mast_nr>DR21</mast_nr>
                                                <mast_cd>JHB04</mast_cd>
                                                <ord_nr>0</ord_nr>
                                                <s_time>0005</s_time>
                                                <s_time_a>0000</s_time_a>
                                                <amt_col_o>0</amt_col_o>
                                                <amt_col_c>0</amt_col_c>
                                                <d_time>0805</d_time>
                                                <d_time_a>111133</d_time_a>
                                                <reason_cd>0</reason_cd>
                                                <dai_date_a>{1}</dai_date_a>
                                                <mast_nr_next>0</mast_nr_next>
                                                <action_cd>0</action_cd>
                                                <reason_cd_mc_p>0</reason_cd_mc_p>
                                                <loc_cd_typed>false</loc_cd_typed>
                                                <volg_nr>0</volg_nr>
                                                <phone_lg>false</phone_lg>
                                                <extseqno>0</extseqno>
                                                <citcode>{2}</citcode>
                                              </dai_line>
                                            </DocumentElement>", branch_cd, date, CitCode);

                basicImportHelper.SaveXml(BasicImportEntity.dai_line, content);
                basicImportHelper.Import();
                Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Daily Route is imported with errors!");
            }
            catch
            {
                throw;
            }
            finally
            {
                replicationParty.With_Role(role).With_SiteIdentifier(siteIdentifier).SaveToDb();
            }
        }

        /// <summary>
        /// Package Life Cycle Interface can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create Package Life Cycle Interface with citcode")]

        public void VerifyThatPackageLifeCycleInterfaceCanBeCreatedWithCitCode()
        {
            var replicationParty = DataFacade.ReplicationParty.Take(rp => rp.PartyType == (int)PartyType.Local);
            var role = replicationParty.Build().Role;
            var siteIdentifier = replicationParty.Build().SiteIdentifier;
            var citIdentifier = "test";
            try
            {
                replicationParty.With_Role(ReplicationRole.CIT).With_SiteIdentifier(citIdentifier).SaveToDb();

                var date = DateTime.Now.ToString("yyyy-MM-dd");
                tableName = String.Concat("dbo.", BasicImportEntity.his_pack);
                idColumnData.Add("branch_cd", branch_cd);
                var dataToVerify = new Dictionary<string, object>()
                {
                    { "CitIdentifier", citIdentifier }
                };
                var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                            <DocumentElement>
                                              <his_pack act='0'>
                                                <branch_cd>{0}</branch_cd>
                                                <pack_nr>201080908007</pack_nr>
                                                <a_date>{1}</a_date>
                                                <a_time>084253</a_time>
                                                <a_status>DEP</a_status>
                                                <fr_loc_nr>1999991001</fr_loc_nr>
                                                <fr_loc_cd>1999991001</fr_loc_cd>
                                                <to_loc_nr>800812</to_loc_nr>
                                                <to_loc_cd>800812</to_loc_cd>
                                                <mast_nr>0</mast_nr>
                                                <user_cd>master</user_cd>
                                                <pack_val>0</pack_val>
                                                <real_time>084253</real_time>
                                                <dec_val>0</dec_val>
                                                <bgtyp_nr>0</bgtyp_nr>
                                                <real_date>{1}</real_date>
                                                <stat_desc>Delivered at depot</stat_desc>
                                                <currencyval cur_cd='EUR'>123</currencyval>
                                            	<new_delivery_date>{1}</new_delivery_date>
                                            	<recollected>TRUE</recollected>
                                            	<citcode>{2}</citcode>
                                              </his_pack>
                                            </DocumentElement>", branch_cd, date, CitCode);

                basicImportHelper.SaveXml(BasicImportEntity.his_pack, content);
                basicImportHelper.Import();
                Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Daily Route is imported with errors!");
            }
            catch
            {
                throw;
            }
            finally
            {
                replicationParty.With_Role(role).With_SiteIdentifier(siteIdentifier).SaveToDb();
            }
        }

        /// <summary>
        /// Daily Stop Service Interface can be updated via Basic Import
        /// </summary>
        [Fact(DisplayName = "Basic Import - Create Daily Stop Service Interface with citcode")]

        public void VerifyThatDailyStopServiceInterfaceCanBeCreatedWithCitCode()
        {
            using (var context = new ModelContext())
            {
                var contr_nr = $"{new Random().Next(40000, 99999)}";
                var date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                tableName = String.Concat("dbo.", BasicImportEntity.dai_serv);
                idColumnData.Add("contr_nr", contr_nr);
                var dataToVerify = new Dictionary<string, object>()
                {
                    { "contr_nr", contr_nr }
                };
                var content = String.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes' ?>
                                            <DocumentElement>
                                              <dai_serv act='0'>
                                                <contr_nr>{0}</contr_nr>
                                                <loc_nr>2852381</loc_nr>
                                                <day_nr>2</day_nr>
                                                <bgtyp_nr>1</bgtyp_nr>
                                                <serv_type>Collect</serv_type>
                                                <to_loc_nr>420</to_loc_nr>
                                                <a_time>0915</a_time>
                                                <revenue>0</revenue>
                                                <planned>1</planned>
                                                <executed>0</executed>
                                                <dai_date>{1}</dai_date>
                                                <dai_date_a>{1}</dai_date_a>
                                                <amt_packs_del>0</amt_packs_del>
                                                <total_value_del>0</total_value_del>
                                                <amt_packs_col>2</amt_packs_col>
                                                <total_value_col>0</total_value_col>
                                                <msg_nr>0</msg_nr>
                                                <dai_timew1>0900-1700</dai_timew1>
                                                <dai_timew2></dai_timew2>
                                                <max_val>0</max_val>
                                                <s_time></s_time>
                                                <serv_nr>24383</serv_nr>
                                                <initiator>1</initiator>
                                                <amt>0</amt>
                                                <Orderline_id></Orderline_id>
                                                <branch_cd>ST</branch_cd>
                                                <mast_cd>ST2T03</mast_cd>
                                                <ref_loc_nr></ref_loc_nr>
                                                <to_loc_cd></to_loc_cd>
                                                <citcode>{2}</citcode>
                                              </dai_serv>
                                            </DocumentElement>", contr_nr, date, CitCode);
                try
                {
                    basicImportHelper.SaveXml(BasicImportEntity.dai_serv, content);
                    basicImportHelper.Import();
                    Assert.True(basicImportHelper.VerifyEntity(tableName, idColumnData, dataToVerify), "Daily Route is imported with errors!");
                }
                catch
                {
                    throw;
                }
                finally
                {
                    context.dai_servs.RemoveRange(context.dai_servs.Where(ds=>ds.contr_nr == contr_nr));
                }
            }
        }
    }
}
