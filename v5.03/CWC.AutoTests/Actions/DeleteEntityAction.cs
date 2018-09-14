using CWC.AutoTests.DataModel;

namespace CWC.AutoTests.Actions
{
    public class DeleteEntityAction
    {
        private ModelContext context;

        public DeleteEntityAction()
        {
            context = new ModelContext();
        }

        public void DeleteServiceOrder(ServiceOrder serviceorder)
        {
            context.ServiceOrders.Remove(serviceorder);
            context.SaveChanges();
        }

        public void DeleteSoLine(SOline soLine)
        {
            context.SOlines.Remove(soLine);
            context.SaveChanges();
        }

        public void DeleteSoProduct(SOProduct soProduct)
        {
            context.SOProducts.Remove(soProduct);
            context.SaveChanges();
        }
    }
}
