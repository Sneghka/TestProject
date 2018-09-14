using Cwc.Common;
using Cwc.Transport;
using Cwc.Transport.Model;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class TransportOrderProductBuilder
    {
        DataBaseParams _dbParams;
        TransportOrderProduct entity;
        AutomationTransportDataContext context;
        public ProductBuilder product;

        public TransportOrderProductBuilder()
        {
            _dbParams = new DataBaseParams();
            context = new AutomationTransportDataContext();
        }

        public TransportOrderProductBuilder With_OrderedQuantity(Int32 value)
        {
            if (entity != null)
            {
                entity.OrderedQuantity = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_OrderedValue(int value)
        {
            if (entity != null)
            {
                entity.OrderedValue = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_Product(Int32 value)
        {
            if (entity != null)
            {
                entity.SetProductID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_Product(Cwc.BaseData.Product value)
        {
            if (entity != null)
            {
                entity.SetProductID(value.ID);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_TransportOrder(Int32 value)
        {
            if (entity != null)
            {
                entity.SetTransportOrderID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_TransportOrder(TransportOrder value)
        {
            if (entity != null)
            {
                entity.SetTransportOrderID(value.ID);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_CurrencyID(String value)
        {
            if (entity != null)
            {
                entity.CurrencyID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_DateCreated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateCreated(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_DateUpdated(DateTime value)
        {
            if (entity != null)
            {
                entity.SetDateUpdated(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_AuthorID(Int32? value)
        {
            if (entity != null)
            {
                entity.SetAuthorID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public TransportOrderProductBuilder With_EditorID(Int32? value)
        {
            if (entity != null)
            {
                entity.SetEditorID(value);

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        

        public TransportOrderProductBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;

                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public TransportOrderProductBuilder New()
        {
            entity = new TransportOrderProduct();

            return this;
        }

        public TransportOrderProductBuilder InitDefault(int transportOrderId)
        {
            product = DataFacade.Product.Take(x => x.ProductCode != null);
            entity = this.New().With_TransportOrder(transportOrderId).With_Product(product).With_OrderedQuantity(1).With_OrderedValue((int)product.Build().Value * 1).With_CurrencyID("EUR");

            return this;
        }

        public static implicit operator TransportOrderProduct(TransportOrderProductBuilder ins)
        {
            return ins.Build();
        }

        public TransportOrderProduct Build()
        {
            return entity;
        }

        public TransportOrderProductBuilder SaveToDb()
        {
            var temp = entity;

            var res = TransportFacade.TransportOrderProductService.Save(temp);
            //was modiied from TransportFacade.TransportOrderProductService.Save(temp, _dbParams) that was made bulk save to db

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Transport Order Product saving is failed. Reason: {res.GetMessage()}");
            }

            return this;
        }

        public TransportOrderProductBuilder Take(Func<TransportOrderProduct, bool> expression)
        {
            entity = context.TransportOrderProducts.FirstOrDefault(expression);

            if (entity == null)
            {
                throw new ArgumentNullException("Transport Order product with specified criteria is not found");
            }

            return this;
        }

        public TransportOrderProductBuilder Delete(TransportOrderProduct top)
        {
            var result = TransportFacade.TransportOrderProductService.Delete(top, isExport: true);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Transport Order Product deletion failed. Reason: {result.GetMessage()}");
            }

            return this;
        }

        
    }
}