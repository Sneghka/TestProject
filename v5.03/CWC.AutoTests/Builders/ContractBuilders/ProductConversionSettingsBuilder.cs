using Cwc.Contracts;
using Cwc.Contracts.Model;
using System;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ProductConversionSettingsBuilder : IDisposable
    {
        //ContractOrderingSetting orderingSettings;
        OrderingSettingProductConversionSetting entity;
        ContractsDataContext context;
    

        public void Dispose()
        {
            context.Dispose();
        }
        public ProductConversionSettingsBuilder()
        {
            context = ContractsDataContext.Create<ContractsDataContext>();
        }

        public ProductConversionSettingsBuilder New()
        {
            entity = new OrderingSettingProductConversionSetting();
            return this;
        }
        public ProductConversionSettingsBuilder With_OrderedProductId(int id)
        {
            if (entity != null)
            {
                entity.OrderedProductID = id;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductConversionSettingsBuilder With_ConvertedProductId(int id)
        {
            if (entity != null)
            {
                entity.ConvertedProductID = id;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ProductConversionSettingsBuilder With_ContractOrderingSettingId(int id)
        {
            if (entity != null)
            {
                entity.SetContractOrderingSettingID(id);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public static implicit operator OrderingSettingProductConversionSetting(ProductConversionSettingsBuilder ins)
        {
            return ins.Build();
        }

        public OrderingSettingProductConversionSetting Build()
        {
            return entity;
        }
    }
}
