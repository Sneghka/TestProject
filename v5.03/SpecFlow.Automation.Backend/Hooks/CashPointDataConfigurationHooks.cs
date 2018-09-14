using Cwc.BaseData;
using Cwc.BaseData.Classes;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.ObjectBuilder;
using System.Linq;
using TechTalk.SpecFlow;
using Cwc.BaseData.Enums;
using Cwc.Security;
using System;
using System.Collections.Generic;
using CWC.AutoTests.Model;
using Cwc.Coin;
using Cwc.BaseData.Model;

namespace Specflow.Automation.Backend.Hooks
{
    [Binding, Scope(Tag = "cashpoint-generation-required")]
    public class CashPointDataConfigurationHooks
    {

        #region Configuration strings
        //Cash Point Name
        private string СashPointAddCashName = "CP AddCash";
        private string СashPointSwapCashName = "CP SwapCash";


        #endregion

        #region Configuration codes
        //Cash Point locations
        private string CashPointAddCashLocationCode = "";
        private string CashPointSwapCashLocationCode = "";

        // Cash Point Numbers
        private string СashPointAddCashNumber = "001";
        private string СashPointSwapCashNumber = "002";

        #endregion



        #region Configuration entities

        private static CoinMachine cashPointAddCash;
        private static CoinMachine cashPointSwapCash;
  
        private static Location сashPointAddCashLocation;
        private static Location сashPointSwapCashLocation;
        private static MachineModel machineModel;


        private CashPointType cashPointTypeAtm;

        public static Location CashPointAddCashLocation { get; private set; }
        public static Location CashPointSwapCashLocation { get; private set; }
        public static CoinMachine CashPointAddCash { get; private set; }
        public static CoinMachine CashPointSwapCash { get; private set; }
        public static CashPointType CashPointTypeAtm { get; private set; }

        public static MachineModel MachineModel { get; private set; }


        #endregion


        [BeforeFeature(Order = 1)]
        public static void Init()
        {
            if (!BaseDataConfigurationHooks.IsBaseDataConfigured)
            {
                BaseDataConfigurationHooks.ConfigureBaseData();
            }
        }

        [BeforeFeature(Order = 2)]
        public static void Init()
        {
            if (!BaseDataConfigurationHooks.IsBaseDataConfigured)
            {
                BaseDataConfigurationHooks.ConfigureBaseData();
            }
        }
    }
}
