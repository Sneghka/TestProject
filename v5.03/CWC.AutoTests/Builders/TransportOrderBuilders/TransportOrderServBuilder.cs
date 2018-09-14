using Cwc.Common;
using Cwc.Transport;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class TransportOrderServBuilder
    {
        DataBaseParams _dbParams;
        TransportOrderServ entity;
        AutomationTransportDataContext context;

        public TransportOrderServBuilder()
        {
            _dbParams = new DataBaseParams();
            context = new AutomationTransportDataContext();
        }

        public TransportOrderServBuilder With_IsPlanned(Boolean value)
        {
            if (entity != null)
            {
                entity.SetIsPlanned(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_IsPerformed(Boolean value)
        {
            if (entity != null)
            {
                entity.SetIsPerformed(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_Service(String value)
        {
            if (entity != null)
            {
                entity.SetServiceID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_Service(Cwc.BaseData.Model.ServicingCode value)
        {
            if (entity != null)
            {
                entity.SetServiceID(value.Code);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_TransportOrderID(Int32 value)
        {
            if (entity != null)
            {
                entity.SetTransportOrderID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_TransportOrderID(TransportOrder value)
        {
            if (entity != null)
            {
                entity.SetTransportOrderID(value.ID);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateCreated(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_DateUpdated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateUpdated(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_AuthorID(Int32? value)
        {
            if (entity != null)
            {
                entity.SetAuthorID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_EditorID(Int32? value)
        {
            if (entity != null)
            {
                entity.SetEditorID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderServBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public TransportOrderServBuilder New()
        {
            entity = new TransportOrderServ();

            return this;
        }

        public static implicit operator TransportOrderServ(TransportOrderServBuilder ins)
        {
            return ins.Build();
        }

        public TransportOrderServ Build()
        {
            return entity;
        }

        public TransportOrderServBuilder SaveToDb()
        {
            var temp = entity;

            var res = TransportFacade.TransportOrderServService.Save(temp);
            // was modified from TransportFacade.TransportOrderServService.Save(temp, _dbParams) that was made bulk sabe into db

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Transport Order Serv saving failed. Reason: {res.GetMessage()}");
            }

            return this;
        }

        public TransportOrderServBuilder Take(Func<TransportOrderServ, bool> expression)
        {
            entity = context.TransportOrderServs.FirstOrDefault(expression);

            if (entity == null)
            {
                throw new ArgumentNullException("Transport order service with provided criteria wasn't found");
            }

            return this;
        }

        public TransportOrderServBuilder Delete(TransportOrderServ tos)
        {
            var result = TransportFacade.TransportOrderServService.Delete(tos, isExport: true);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Transport Order Service deletion failed. Reason: {result.GetMessage()}");
            }

            return this;
        }

        public TransportOrderServBuilder InitDefault(int transportOrderId)
        {
            var service = DataFacade.ServicingCode.Take(x => x.Code != null).Build();

            entity = this.New().With_TransportOrderID(transportOrderId).With_Service(service.Code).With_IsPerformed(false).With_IsPlanned(true);

            return this;
        }
    }
}