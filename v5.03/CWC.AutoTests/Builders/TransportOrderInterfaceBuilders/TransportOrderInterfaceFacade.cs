using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.ObjectBuilder.TransportOrderInterfaceBuilders
{
    public class TransportOrderInterfaceFacade
    {
        readonly static object locker = new object();

        static TransportOrderInterfaceBuilder.TransportOrderInterfaceBuilder transportOrderInterface;
        static TransportOrderInterfaceBuilder.TransportOrderLineInterfaceBuilder transportOrderLineInterface;
        static TransportOrderInterfaceBuilder.TransportOrderProductInterfaceBuilder transportOrderProductInterface;
        static TransportOrderInterfaceBuilder.TransportOrderServiceInterfaceBuilder transportOrderServiceInterface;
        static TransportOrderInterfaceBuilder.TransportOrderUpdateInterfaceBuilder transportOrderUpdateInterface;

        public static TransportOrderInterfaceBuilder.TransportOrderUpdateInterfaceBuilder TransportOrderUpdate
        {
            get
            {
                lock (locker)
                {
                    transportOrderUpdateInterface = new TransportOrderInterfaceBuilder.TransportOrderUpdateInterfaceBuilder();
                }

                return transportOrderUpdateInterface;
            }
        }

        public static TransportOrderInterfaceBuilder.TransportOrderServiceInterfaceBuilder TransportOrderService
        {
            get
            {
                lock (locker)
                {
                    transportOrderServiceInterface = new TransportOrderInterfaceBuilder.TransportOrderServiceInterfaceBuilder();
                }

                return transportOrderServiceInterface;
            }
        }

        public static TransportOrderInterfaceBuilder.TransportOrderProductInterfaceBuilder TransportOrderProduct
        {
            get
            {
                lock (locker)
                {
                    transportOrderProductInterface = new TransportOrderInterfaceBuilder.TransportOrderProductInterfaceBuilder();
                }

                return transportOrderProductInterface;
            }
        }

        public static TransportOrderInterfaceBuilder.TransportOrderInterfaceBuilder TransportOrder
        {
            get
            {
                lock (locker)
                {
                    transportOrderInterface = new TransportOrderInterfaceBuilder.TransportOrderInterfaceBuilder();
                }

                return transportOrderInterface;
            }
        }

        public static TransportOrderInterfaceBuilder.TransportOrderLineInterfaceBuilder TransportOrderLine
        {
            get
            {
                lock (locker)
                {
                    transportOrderLineInterface = new TransportOrderInterfaceBuilder.TransportOrderLineInterfaceBuilder();
                }

                return transportOrderLineInterface;
            }
        }
    }
}
