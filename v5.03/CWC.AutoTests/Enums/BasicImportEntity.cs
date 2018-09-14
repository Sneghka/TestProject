using System.ComponentModel;

namespace CWC.AutoTests.Enums
{
    public enum BasicImportEntity
    {
        [Description("dbo.ServiceOrder")]
        ServiceOrder = 0,

        ServiceOrderUpdate = 1,

        [Description("dbo.SOline")]
        SOline = 2,

        [Description("dbo.SOProduct")]
        SOProduct = 3,

        [Description("dbo.SOService")]
        SOService = 4,

        [Description("dbo.customer")]
        Customer = 5,

        [Description("dbo.location")]
        Location = 6,

        [Description("dbo.employee")]
        Employee = 7,

        [Description("dbo.Material")]
        Material = 8,

        [Description("dbo.Product")]
        Product = 9,

        [Description("dbo.his_pack")]
        his_pack = 10,

        [Description("dbo.dai_coin")]
        dai_coin = 11,

        [Description("dbo.ServicingCode")]
        ServicingCode = 12,

        [Description("dbo.branch")]
        Branch = 13,

        [Description("dbo.daily")]
        daily = 14,

        [Description("dbo.dai_line")]
        dai_line = 15,

        [Description("dbo.dai_serv")]
        dai_serv = 16
    }
}