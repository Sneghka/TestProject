using Cwc.BaseData;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.Utils;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class SiteBuilder
    {
        Site entity;
        public LocationBuilder locationBuilder;

        public SiteBuilder()
        {
        }

        public SiteBuilder With_ID(Int32 value)
        {
            entity.ID = value;
            return this;
        }

        public SiteBuilder With_Description(String value)
        {
            entity.Description = value;
            return this;
        }

        public SiteBuilder With_Branch_cd(String value)
        {
            entity.Branch_cd = value;
            return this;
        }

        public SiteBuilder With_LocationID(Decimal value)
        {
            entity.Loc_nr = value;
            return this;
        }
        public SiteBuilder With_Location(Location value)
        {
            entity.Location = value;
            entity.Loc_nr = entity.Location.ID;
            return this;
        }

        public SiteBuilder With_BranchType(BranchType value)
        {
            entity.BranchType = value;
            return this;
        }

        public SiteBuilder With_SubType(BranchSubType? value)
        {
            entity.SubType = value;
            return this;
        }

        public SiteBuilder With_WP_IsExternal(Boolean value)
        {
            entity.WP_IsExternal = value;
            return this;
        }

        public SiteBuilder New()
        {
            entity = new Site();
            using (var context = new BaseDataContext())
            {
                var defined = context.Sites.Max(b => b.ID) + 1;
                entity.SetID(defined);
            }            

            return this;
        }

        public static implicit operator Site(SiteBuilder ins)
        {
            return ins.Build();
        }

        public Site Build()
        { 
            return entity;
        }

        public SiteBuilder SaveToDb()
        {
            var loginResult = SecurityFacade.LoginService.GetAdministratorLogin();
            UserParams userParams = new UserParams(loginResult);

            var result = BaseDataFacade.SiteService.Save(entity, userParams, null);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("Site saving failed. Reason: {0}", result.GetMessage()));
            }

            return this;
        }

        public SiteBuilder DeleteMany(Expression<Func<Site, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
               var sites = context.Sites.Where(expression);
                if (sites == null)
                {
                    throw new ArgumentNullException("Sites with given criteria weren't found");
                }

                var res = BaseDataFacade.SiteService.DeleteMultiple(sites.Select(x => x.ID).ToArray());
                if (!res.IsSuccess)
                {
                    throw new InvalidOperationException($"Sites cannot be deleted. Reason: {res.GetMessage()}");
                }
            }
            return this;
        }

        public SiteBuilder DeleteOne(Expression<Func<Site, bool>> expression)
        {
            using (var context = new AutomationBaseDataContext())
            {
                var site = context.Sites.First(expression);
                if(site == null)
                {
                    throw new ArgumentNullException("Site with given criteria wasn't found");
                }
            
                var res = BaseDataFacade.SiteService.Delete(site);
                if (!res.IsSuccess)
                {
                    throw new InvalidOperationException($"Site cannot be deleted. Reason: {res.GetMessage()}");
                }
            }
            return this;
        }

        public SiteBuilder Take(Expression<Func<Site, bool>> expression)
        {
            entity = BaseDataFacade.SiteService.LoadBy(expression).FirstOrDefault();
            if (entity == null)
            {
                throw new ArgumentNullException("Site with provided criteria wasn't found");
            }

            return this;
        }

        public SiteBuilder InitDefault()
        {
            locationBuilder = DataFacade.Location.InitDefault().With_HandlingType("CAS").SaveToDb();
            this.New()
                .With_Branch_cd(ValueGenerator.GenerateString("SP", 6))
                .With_Description(ValueGenerator.GenerateString("SP", 6))
                .With_BranchType(BranchType.CashCenter)
                .With_SubType(BranchSubType.Notes)
                .With_WP_IsExternal(false)
                .With_Location(locationBuilder);

            return this;
        }
    }
}