using Cwc.BaseData;
using Cwc.Common;
using Cwc.Routes;
using CWC.AutoTests.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class MasterRouteBuilder
    {
        AutomationRoutesDataContext _context;
        DataBaseParams _dbParams;
        MasterRoute entity;
        public List<MasterRouteStop> linkedStops;

        public MasterRouteBuilder()
        {
            _dbParams = new DataBaseParams();
            _context = new AutomationRoutesDataContext();
            linkedStops = new List<MasterRouteStop>();
        }

        public MasterRouteBuilder With_Number(String value)
        {
            if (entity != null)
            {
                entity.Number = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_Code(String value)
        {
            if (entity != null)
            {
                entity.Code = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_ReferenceNumber(String value)
        {
            if (entity != null)
            {
                entity.ReferenceNumber = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_WeekdayNumber(WeekdayNumber? value)
        {
            if (entity != null && value.HasValue)
            {
                entity.WeekdayNumber = value.Value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_WeekdayName(Weekday value)
        {
            if (entity != null)
            {
                entity.WeekdayName = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_DistancePlanned(Decimal? value)
        {
            if (entity != null)
            {
                entity.DistancePlanned = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_CrewRequired(Int32? value)
        {
            if (entity != null)
            {
                entity.CrewRequired = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_Description(String value)
        {
            if (entity != null)
            {
                entity.Description = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.DateCreated = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_DateUpdated(DateTime value)
        {
            if (entity != null)
            {
                entity.DateUpdated = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_Branch_id(Int32 value)
        {
            if (entity != null)
            {
                entity.SetBranchID(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_Branch(Site value)
        {
            if (entity != null)
            {
                entity.Branch = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_Branch_id(Site value)
        {
            if (entity != null)
            {                
                entity.SetBranchID(value.Branch_nr);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_Branch_Code(string branchCode)
        {
            if (entity != null)
            {
                entity.Branch = DataFacade.Site.Take(x => x.Branch_cd == branchCode); 
                entity.SetBranchID(DataFacade.Site.Take(x => x.Branch_cd == branchCode).Build().Branch_nr);               
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder Without_Branch()
        {
            if (entity != null)
            {
                entity.ClearPropertyValue("Branch_id");
                entity.Branch = null;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public MasterRouteBuilder With_AuthorId(Int32? value)
        {
            if (entity != null)
            {
                entity.AuthorId = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_EditorId(Int32? value)
        {
            if (entity != null)
            {
                entity.EditorId = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_MasterRouteStop(MasterRouteStopBuilder value)
        {
            if (entity != null)
            {
                this.linkedStops.Add(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_MasterRouteStops(List<MasterRouteStopBuilder> stops)
        {
            if (entity != null)
            {
                foreach (var stop in stops)
                {                   
                    this.linkedStops.Add(stop);                    
                }
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_MasterRouteStops(params MasterRouteStopBuilder[] stopsList)
        {
            if (entity != null)
            {
                this.linkedStops.Clear();
                for (int i = 0; i < stopsList.Length; i++)
                {
                    this.linkedStops.Add(stopsList[i]);
                }
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder Remove_MasterRouteStops()
        {
            if (entity != null)
            {
                this.linkedStops.Clear();
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public MasterRouteBuilder New()
        {
            entity = new MasterRoute();
            return this;
        }


        public static implicit operator MasterRoute(MasterRouteBuilder ins)
        {
            return ins.Build();
        }

        public MasterRoute Build()
        {
            return entity;
        }

        public MasterRouteBuilder SaveToDb()
        {
            var temp = entity;
            temp.DateCreated = DateTime.Now;
            temp.DateUpdated = DateTime.Now;
            var res = RoutesFacade.MasterRouteService.Save(temp, _dbParams);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Master Route saving failed. Reason: {res.GetMessage()}");
            }

            if (linkedStops.Any())
            {
                var user = Cwc.Security.SecurityFacade.LoginService.GetAdministratorLogin();

                foreach (var item in linkedStops)
                {
                    item.MasterRoute_id = temp.ID;
                    item.DateCreated = DateTime.Now;
                    item.DateUpdated = DateTime.Now;
                    item.Author_id = user.UserID;
                    item.Editor_id = user.UserID;

                    var stopSaveResult = RoutesFacade.MasterRouteStopService.Save(item, _dbParams);

                    if (!stopSaveResult.IsSuccess)
                    {
                        throw new InvalidOperationException($"Master Route saving failed. Reason: {stopSaveResult.GetMessage()}");
                    }
                }
            }
            return this;
        }

        public MasterRouteBuilder Take(Expression<Func<MasterRoute, bool>> expression)
        {
            var found = _context.MasterRoutes.Where(expression).FirstOrDefault();

            if (found == null)
            {
                throw new ArgumentNullException("Master Route with provided criteria is not found");
            }

            entity = RoutesFacade.MasterRouteService.Load(found.ID, _dbParams);
            return this;
        }

        public void Delete(Expression<Func<MasterRoute, bool>> expression)
        {
            var found = _context.MasterRoutes.FirstOrDefault(expression);

            if (found == null)
            {
                throw new ArgumentNullException("Master Route with provided criteria is not found");
            }

            var foundStops = _context.MasterRouteStops.Where(mrs => mrs.MasterRoute_id == found.ID);

            if (foundStops.Any())
            {
                var stopsDeleteResult = RoutesFacade.MasterRouteStopService.Delete(foundStops.Select(x => x.ID).ToArray(), _dbParams);

                if (!stopsDeleteResult.IsSuccess)
                {
                    throw new InvalidOperationException($"Master Route Stops deletion failed. Reason: {stopsDeleteResult.GetMessage()}");
                }
            }

            var res = RoutesFacade.MasterRouteService.Delete(new[] { found.ID }, _dbParams);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Master Route deletion failed. Reason: {res.GetMessage()}");
            }
        }
        public bool IsMasterRouteExist(Expression<Func<MasterRoute, bool>> expression)
        {
            return _context.MasterRoutes.Any(expression);
        }
    }
}