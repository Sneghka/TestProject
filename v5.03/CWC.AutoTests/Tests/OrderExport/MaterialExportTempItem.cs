using Cwc.Ordering.Classes.OrderExport;

namespace CWC.AutoTests.Tests.OrderExport
{
    public class MaterialExportTempItem : MaterialExportItem
    {
        private int id;

        public int ID
        {
            get { return this.id; }
            set { this.id = value; }
        }
    }
}
