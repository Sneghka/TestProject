using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CWC.AutoTests.Model;

namespace CWC.AutoTests.Tests.BasicExport
{
    public class ServiceOrderCleanUpFixture : IDisposable
    {
        public void Dispose()
        {
            var locker = new object();
            lock (locker)
            {
                using (var _context = new AutomationOrderingDataContext())
                {
                    var ordersToRemove = _context.Orders.Where(s => s.ID.StartsWith("1101") || s.ReferenceID.StartsWith("1101")).ToArray();
                    var orderLinesToRemove = _context.OrderLines.ToArray().Where(s => ordersToRemove.Select(o => o.ID).Contains(s.OrderID));
                    var productsToRemove = _context.SOProduct.ToArray().Where(s => orderLinesToRemove.Select(l => l.ID).Contains(s.OrderLine_ID));

                    _context.SOProduct.RemoveRange(productsToRemove);
                    _context.OrderLines.RemoveRange(orderLinesToRemove);
                    _context.Orders.RemoveRange(ordersToRemove);
                    _context.SaveChanges();
                }
            }
        }
    }
}
