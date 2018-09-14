using Cwc.BaseData.Classes;
using Cwc.BaseData.Enums;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.Tests.BasicExport
{
    public class BasicExportCleanUpFixture : IDisposable
    {
        public BasicExportCleanUpFixture()
        {
            ConfigurationKeySet.Load();
            Cwc.Sync.SyncConfiguration.LoadExportMappings();
            var key = HelperFacade.ConfigurationKeysHelper.GetKey(x => x.Name == Cwc.BaseData.Enums.ConfigurationKeyName.UseCwcCSMasterDataValidation);
            HelperFacade.ConfigurationKeysHelper.Update(key, "False");

            using (var context = new AutomationBaseDataContext())
            {
                var es = context.ExportEntitySettings.Where(x => x.Alias != null && (x.IsExcludeImportedRecords || !x.IsExportAvaible));

                foreach (var item in es)
                {
                    item.IsExcludeImportedRecords = false;
                    item.IsExportAvaible = true;
                }

                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            object locker = new object();
            lock (locker)
            {
                using (var context = new AutomationBaseDataContext())
                {
                    var locationIdList = context.Locations.Where(l => l.Name == "AutoTestManagement").Select(x => x.ID).ToList();
                    var visitAddressIdList = context.Locations.Where(l => locationIdList.Contains(l.ID) && l.Level == (int)LocationLevel.VisitAddress).Select(x => x.ID).ToList();
                    var locationsWithVisitAddressList = context.Locations.Where(l => visitAddressIdList.Contains((int)l.VisitAddressID)).Select(x => x.ID).ToList();
                    using (var transportContext = new AutomationTransportDataContext())
                    {
                        transportContext.CitProcessSettingLinks.RemoveRange(transportContext.CitProcessSettingLinks.Where(x => locationIdList.Contains(x.LocationID)
                            || locationsWithVisitAddressList.Contains(x.LocationID)));
                        transportContext.SaveChanges();
                    }
                    context.ExportItems.RemoveRange(context.ExportItems);
                    context.BaseAddresses.RemoveRange(context.BaseAddresses);
                    context.SaveChanges();
                    context.Locations.RemoveRange(context.Locations.Where(l => ((locationIdList.Contains(l.ID)) && l.Level == LocationLevel.ServicePoint)
                        || locationsWithVisitAddressList.Contains(l.ID)));
                    context.SaveChanges();

                    context.Locations.RemoveRange(context.Locations.Where(l => (locationIdList.Contains(l.ID) && l.Level == (int)LocationLevel.VisitAddress)));
                    context.Customers.RemoveRange(context.Customers.Where(c => c.Name == "AutoTestManagement" || c.Abbrev == "AutoTestManagement"));
                    context.Materials.RemoveRange(context.Materials.Where(m => m.Description == "AutoTestManagement"));
                    context.Products.RemoveRange(context.Products.Where(p => p.Description == "AutoTestManagement"));
                    using (var routesContext = new AutomationRoutesDataContext())
                    {
                        routesContext.MasterRoutes.RemoveRange(routesContext.MasterRoutes.Where(m => m.Description == "AutoTestManagement"));
                        routesContext.SaveChanges();
                    }
                }
            }                  
        }
    }
}
