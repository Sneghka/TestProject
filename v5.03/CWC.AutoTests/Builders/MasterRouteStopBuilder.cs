
using Cwc.BaseData;
using Cwc.Common;
using Cwc.Routes;
using Cwc.Security;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class MasterRouteStopBuilder
    {
        AutomationRoutesDataContext _context;
        DataBaseParams _dbParams;
        MasterRouteStop entity;
        

        public MasterRouteStopBuilder()
        {
            _dbParams = new DataBaseParams();
            _context = new AutomationRoutesDataContext();
        }

        public MasterRouteStopBuilder With_SequenceNumber(Int32? value)
        {
            if (entity != null && value.HasValue)
            {
                entity.SequenceNumber = value.Value;
            }
            return this;
        }
        public MasterRouteStopBuilder Without_SequenceNumber()
        {
            if (entity != null)
            {
                entity.SequenceNumber = default(Int32);
            }
            return this;
        }

        public MasterRouteStopBuilder With_ArrivalTime(TimeSpan value)
        {
            if (entity != null)
            {
                entity.ArrivalTime = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }
       
        public MasterRouteStopBuilder With_OnSiteTime(TimeSpan value)
        {
            if (entity != null)
            {
                entity.OnSiteTime = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteStopBuilder With_DepartureTime(TimeSpan value)
        {
            if (entity != null)
            {
                entity.DepartureTime = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteStopBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.DateCreated = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteStopBuilder With_DateUpdated(DateTime value)
        {
            if (entity != null)
            {
                entity.DateUpdated = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteStopBuilder With_Location_id(Decimal value)
        {
            if (entity != null)
            {
                entity.Location_id = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public MasterRouteStopBuilder With_Location(Location value)
        {
            if (entity != null && value != null)
            {
                entity.Location_id = value.ID;

            }

            return this;
        }

        public MasterRouteStopBuilder Without_Location()
        {
            if (entity != null)
            {
                entity.Location_id = default(decimal);
            }

            return this;
        }
        public MasterRouteStopBuilder With_MasterRoute_id(Int32 value)
        {
            if (entity != null)
            {
                entity.MasterRoute_id = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteStopBuilder With_Author_id(Int32 value = 0)
        {
            if (entity != null)
            {
                var login = SecurityFacade.LoginService.GetAdministratorLogin();
                entity.Author_id = login.UserID;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteStopBuilder With_Editor_id(Int32 value = 0)
        {
            if (entity != null)
            {
                var login = SecurityFacade.LoginService.GetAdministratorLogin();

                entity.Editor_id = login.UserID;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public MasterRouteStopBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }


            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public MasterRouteStopBuilder New()
        {
            entity = new MasterRouteStop();

            return this;
        }


        public static implicit operator MasterRouteStop(MasterRouteStopBuilder ins)
        {
            return ins.Build();
        }

        public MasterRouteStop Build()
        {
            return entity;
        }

        public MasterRouteStopBuilder SaveToDb()
        {
            var temp = entity;

            var res = RoutesFacade.MasterRouteStopService.Save(temp, _dbParams);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Master route saving failed. Reason: {res.GetMessage()}");
            }

            return this;
        }

        public MasterRouteStopBuilder Take(Expression<Func<MasterRouteStop, bool>> expression)
        {
            var found = _context.MasterRouteStops.Where(expression).FirstOrDefault();

            if (found == null)
            {
                throw new ArgumentNullException("Master Rote Stop with provided criteria wasn't found");
            }

            entity = RoutesFacade.MasterRouteStopService.Load(found.ID, _dbParams);

            return this;
        }        

        public void Delete(Func<MasterRouteStop, bool> expression)
        {
            var stops = _context.MasterRouteStops.Where(expression).FirstOrDefault();

            if (stops == null)
            {
                throw new ArgumentNullException("Master Rote Stop with provided criteria wasn't found");
            }

            var res = RoutesFacade.MasterRouteStopService.Delete(new[] { stops.ID }, _dbParams);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Master route Stop deletion failed. Reason: {res.GetMessage()}");
            }
        }

        public bool IsMasterRouteStopExist(Expression<Func<MasterRouteStop, bool>> expression)
        {
            return _context.MasterRouteStops.Where(expression).FirstOrDefault() != null;           
        }
    }
}