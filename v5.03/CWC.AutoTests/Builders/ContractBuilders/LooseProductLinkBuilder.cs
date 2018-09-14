using Cwc.Contracts;
using Cwc.Contracts.Model;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class LooseProductLinkBuilder
    {

        OrderingSettingLooseProductsLink entity;
        AutomationContractDataContext context;

        public LooseProductLinkBuilder()
        {
            context = new AutomationContractDataContext();
        }

        public LooseProductLinkBuilder With_ContractOrderingSettingId(int id)
        {
            if (entity != null)
            {
                entity.SetContractOrderingSettingID(id);
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public LooseProductLinkBuilder With_ProductId(int id)
        {
            if (entity != null)
            {
                entity.ProductID = id;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public LooseProductLinkBuilder With_IsLatestRevisison(bool value)
        {
            if (entity != null)
            {
                entity.IsLatestRevision = value;
                return this;
            }

            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public LooseProductLinkBuilder New()
        {
            entity = new OrderingSettingLooseProductsLink();
            return this;
        }

        public static implicit operator OrderingSettingLooseProductsLink(LooseProductLinkBuilder ins)
        {
            return ins.Build();
        }

        public OrderingSettingLooseProductsLink Build()
        {
            return entity;
        }


        public LooseProductLinkBuilder SaveToDb(bool isClearingRequired = false)
        {
            if (isClearingRequired)
            {
                HelperFacade.ContractHelper.ClearLooseProductLink(entity.ContractOrderingSettingID);
            }

            var result = ContractsFacade.OrderingSettingLooseProductsLinkService.Save(entity);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Ordering setting loose product link saving failed. Reason: { result.GetMessage() }");
            }

            return this;
        }


        public LooseProductLinkBuilder Take(Expression<Func<OrderingSettingLooseProductsLink, bool>> expression, AutomationContractDataContext modelContext = null)
        {
            using (var newContext = new AutomationContractDataContext())
            {
                var context = modelContext ?? newContext;
                var found = context.OrderingSettingLooseProductsLinks.FirstOrDefault(expression);
                if (found == null)
                {
                    throw new InvalidOperationException("Ordering settings loose product link is not found!");
                }

                entity = ContractsFacade.OrderingSettingLooseProductsLinkService.Load(found.ID, null);
            }

            return this;
        }
    }
}
