using Cwc.BaseData;
using Cwc.BaseData.Classes;
using Cwc.Jobs;
using Cwc.Sync;
using Cwc.Transport.Jobs;
using Cwc.Transport.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Tests.Transport.TransportInterface
{
    public class TransportOrderJobFixture
    {
        public SyncSettings settings;
        public string folderPath, routeCode;
        public Customer defaultCustomer;
        public Location defaultLocation;
        public CitProcessSetting matchedCitProcessSetting;
        public ServiceType deliveryServiceType, collectServiceType, servicingServiceType, replenishmentServiceType;
        public TransportOrderJobFixture()
        {
            ConfigurationKeySet.Load();
            SyncConfiguration.LoadExportMappings();

            var folder = System.IO.Path.Combine("JobFolders", "TransportOrderImport");
            var basePath = System.IO.Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            var path = System.IO.Path.Combine(basePath, folder);

            var jobInfo = JobsFacade.JobInfoService.LoadAll(new Cwc.Common.DataBaseParams()).FirstOrDefault(job => job.Name == "Transport Order Integration");
            settings = new SyncSettings();

            settings.ExchangeFolder = path;
            settings.TransportServiceType = TransportServiceType.FileSystem;
            settings.JobInstance_id = JobsFacade.JobInstanceService.LoadAll(new Cwc.Common.DataBaseParams()).FirstOrDefault(job => job.JobInfo_id == jobInfo.ID).ID;
            settings.Login = "";
            settings.Password = "";
            folderPath = path;

            defaultCustomer = DataFacade.Customer.Take(c => c.ReferenceNumber == "1101").Build();
            defaultLocation = DataFacade.Location.Take(x => x.Code == "SP02").Build();
            matchedCitProcessSetting = DataFacade.CitProcessSettings.MatchByLocation(defaultLocation).RemoveReplenishmentConfigurationFromSetting();

            deliveryServiceType = DataFacade.ServiceType.Take(x => x.Code == "DELV");
            collectServiceType = DataFacade.ServiceType.Take(x => x.Code == "COLL");
            servicingServiceType = DataFacade.ServiceType.Take(x => x.Code == "SERV");
            replenishmentServiceType = DataFacade.ServiceType.Take(x => x.Code == "REPL");


            routeCode = $"SP{DateTime.Now.Millisecond}";
        }

        public void CreateSettings(ServiceType collect, ServiceType delivery, ServiceType servicing = null)
        {
            var contract = DataFacade.Contract.Take(x => x.CustomerID == defaultCustomer.ID && x.IsLatestRevision).Build();
            var replServType = DataFacade.ServiceType.Take(x => x.Code == "REPL").Build();
            var contractedReplenishment = DataFacade.ContractOrderingSetting.LoadAllOrderingSettings(x => x.Contract_id == contract.ID && x.IsLatestRevision && x.ServiceType_id == replServType.ID).Select(x => x.ServiceType_id);
            var replenishmentConfig = DataFacade.CitProcessSettings.MatchByLocation(defaultLocation)
                .With_ReplenishentConfiguration(contractedReplenishment.First(), addedCollectionType: collect.ID, addedDeliveryType: delivery.ID, addedSeervicingType: servicing.ID);
        }
    }
}
