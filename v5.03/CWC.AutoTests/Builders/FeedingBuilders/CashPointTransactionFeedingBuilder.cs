using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace CWC.AutoTests.Builders.FeedingBuilders
{
    class CashPointTransactionFeedingBuilder
    {
        XDocument documentElement;
        XElement transaction;
        XElement transactionLine;

        public CashPointTransactionFeedingBuilder()
        {
            documentElement = new XDocument(new XElement("DocumentElement"));
        }

        public CashPointTransactionFeedingBuilder New()
        {
            transaction = new XElement("WP_CM_Transaction");
            documentElement.Root.Add(transaction);
            return this;
        }

        public CashPointTransactionFeedingBuilder With_MachineNumber(string number)
        {
            if (number != null)
            {
                transaction.Add(new XElement("Number", number));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_LocationCode(string locationCode)
        {
            if (locationCode != null)
            {
                transaction.Add(new XElement("LocationCode", locationCode));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Type(string type)
        {
            if (type != null)
            {
                transaction.Add(new XElement("Type", type));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_TransactionNumber(string transactionNumber)
        {
            if (transactionNumber != null)
            {
                transaction.Add(new XElement("TransactionNumber", transactionNumber));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_StartDate(string startDate)
        {
            if (startDate != null)
            {
                transaction.Add(new XElement("StartDate", startDate));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_EndDate(string endDate)
        {
            if (endDate != null)
            {
                transaction.Add(new XElement("EndDate", endDate));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_BankNumber(string bankNumber)
        {
            if (bankNumber != null)
            {
                transaction.Add(new XElement("BankNumber", bankNumber));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_SealNumber(string sealNumber)
        {
            if (sealNumber != null)
            {
                transaction.Add(new XElement("SealNumber", sealNumber));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Bic(string bic)
        {
            if (bic != null)
            {
                transaction.Add(new XElement("Bic", bic));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_AccountNumber(string accountNumber)
        {
            if (accountNumber != null)
            {
                transaction.Add(new XElement("AccountNumber", accountNumber));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Currency(string currency)
        {
            if (currency != null)
            {
                transaction.Add(new XElement("Currency", currency));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Value(string value)
        {
            if (value != null)
            {
                transaction.Add(new XElement("Value", value));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Change(string change)
        {
            if (change != null)
            {
                transaction.Add(new XElement("Change", change));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Fee(string fee)
        {
            if (fee != null)
            {
                transaction.Add(new XElement("Fee", fee));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Weight(string weight)
        {
            if (weight != null)
            {
                transaction.Add(new XElement("Weight", weight));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Quantity(string quantity)
        {
            if (quantity != null)
            {
                transaction.Add(new XElement("Quantity", quantity));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_PaymentMethod(string paymentMethod)
        {
            if (paymentMethod != null)
            {
                transaction.Add(new XElement("PaymentMethod", paymentMethod));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Replenishment(string replenishment)
        {
            if (replenishment != null)
            {
                transaction.Add(new XElement("Replenishment", replenishment));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_StaffNumber(string staffNumber)
        {
            if (staffNumber != null)
            {
                transaction.Add(new XElement("StaffNumber", staffNumber));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_SecondStaffNumber(string secondStaffNumber)
        {
            if (secondStaffNumber != null)
            {
                transaction.Add(new XElement("SecondStaffNumber", secondStaffNumber));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_Comment(string comment)
        {
            if (comment != null)
            {
                transaction.Add(new XElement("Comment", comment));
            }

            return this;
        }

        public CashPointTransactionFeedingBuilder With_StockPositionList()
        {
            transactionLine = new XElement("WP_CM_TransactionLine");
            transaction.Add(transactionLine);

            return this;
        }


        public CashPointTransactionFeedingBuilder With_TransactionLine(string Type, string StockPositionDirection = null, string CassetteNumber = null, string CassetteExternalNumber = null, string SealNumber = null)
        {


            return this;
        }

        public XDocument Build()
        {
            return documentElement;
        }
    }
}




