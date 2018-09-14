using Xunit;
using CWC.AutoTests.Helpers;
using CWC.AutoTests.Tests.Fixtures;
using System;
using System.Linq;
using Cwc.BaseData.Classes;
using CWC.AutoTests.Model;
using Cwc.Feedings;
using Cwc.Coin;

namespace CWC.AutoTests.Tests.AutoTests
{
    [Xunit.Collection("MyCollection")] //added because of fixtures(BaseDataFixture, CashPointFixture) conflict
    public class CashPointTransactionsTests : IClassFixture<CashPointFixture>, IDisposable
    {
        CoinDataContext coinContext;
        AutomationFeedingDataContext feedingContext;
        int number;
        CashPointFixture _fixture;

        public CashPointTransactionsTests(CashPointFixture fixture)
        {
            ConfigurationKeySet.Load();
            coinContext = new CoinDataContext();
            feedingContext = new AutomationFeedingDataContext();
            number = int.Parse($"1314{new Random().Next(1000, 2000)}");
            _fixture = fixture;
        }

        public void Dispose()
        {
            feedingContext.Dispose();
            coinContext.Dispose();
        }

        [Fact(DisplayName = "Cash Point Transaction - When used proper data for non mixed stock position Then transaction properly processed")]
        public void VerifyThatCashPointTransactionCreatedForNonMixedStockPosition()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "stock",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no"         //mixed
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            var foundTransaction = coinContext.Transactions.First(tr => tr.Number == number);
            var foundTransactionLine = coinContext.TransactionLines.Where(trl => trl.Transaction_id == foundTransaction.ID);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Result == 0 && m.ObjectID == foundTransaction.ID).Any());
            Assert.True(foundTransactionLine.Any());
        }


        [Fact(DisplayName = "Cash Point Transaction - When used proper data for mixed stock position Then transaction properly processed")]
        public void VerifyThatCashPointTransactionCreatedWhenUsedMixedStockPosition()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "stock",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "",          //material ID
                                "yes"        //mixed
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            var firstAssertreplacement = coinContext.Transactions.First(tr => tr.Number == number);
            var secondAssertreplacement = coinContext.TransactionLines.Where(trl => trl.Transaction_id == firstAssertreplacement.ID);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Result == 0 && m.ObjectID == firstAssertreplacement.ID).Any());
            Assert.True(secondAssertreplacement.Any());
        }

        [Fact(DisplayName = "Cash Point Transaction - When Start Date greater than End Date Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenStartDateGreaterThanEndDate()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss"),
                                "stock",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Start date cannot be greater than End date") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Start date cannot be greater than End date", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Collect transaction and Replenishment = yes Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenCollectAndReplenishment()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "collect",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "yes",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss")
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Replenishment cannot be reported under this transaction type.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
        }

        [Fact(DisplayName = "Cash Point Transaction - When Issue transaction and Replenishment = yes Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenIssueAndReplenishment()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "issue",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "yes",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss")
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Replenishment cannot be reported under this transaction type.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Replenishment cannot be reported under this transaction type.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Change is Negative Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenChangeIsNegative()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
                                    <Change>-1</Change>
                                    <Fee>1</Fee>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "issue",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss")
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Attribute 'Change' cannot have negative value.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
        }

        [Fact(DisplayName = "Cash Point Transaction - When Fee is Negative Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenFeeIsNegative()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
                                    <Change>1</Change>
                                    <Fee>-1</Fee>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "issue",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss")
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Attribute 'Fee' cannot have negative value.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Attribute 'Fee' cannot have negative value.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Transaction Number is not unique Then System doesn't allow to submit it")]
        public void VerifyMessageWhenTransactionNumberIsDuplicated()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
                                    <Change>1</Change>
                                    <Fee>1</Fee>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "issue",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss")
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);
            response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Transaction with this Number and Start Date was already submitted by this Cash Point.") && m.Result == ValidatedFeedingLogResult.Failed).Any(), response.Body.ErrorMessage);
            Assert.Equal("Transaction with this Number and Start Date was already submitted by this Cash Point.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Transaction line Is Mixed = no and Both Material and Product are Empty Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenMaterialAndProductAreEmptyForNonMixedTransactionLine()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
                                    <Change>1</Change>
                                    <Fee>1</Fee>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "issue",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1).ToString("yyyy-MM-dd hh:mm:ss")
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Transaction lines for cassette with non-mixed content should contain either Material or Product.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Transaction lines for cassette with non-mixed content should contain either Material or Product.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Transaction line Is Mixed = yes and Material is not empty Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenMixedTransactionLineAndMaterialIsNotEmpty()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "stock",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",          //material ID
                                "yes"        //mixed
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Transaction lines for mixed cassette should not contain Material or Product. Instead of it, Material Type must be specified.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Transaction lines for mixed cassette should not contain Material or Product. Instead of it, Material Type must be specified.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Transaction line Is Mixed = yes and Product is not empty Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenMixedTransactionLineAndProductIsNotEmpty()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <ProductCode>{8}</ProductCode>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "stock",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "8",         //product code
                                "yes"        //mixed
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Transaction lines for mixed cassette should not contain Material or Product. Instead of it, Material Type must be specified.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Transaction lines for mixed cassette should not contain Material or Product. Instead of it, Material Type must be specified.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Transaction line Is Mixed = yes and Material Type is empty Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenMixedTransactionLineAndMaterialTypeIsEmpty()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{4}</Quantity>
		                                <Value>{5}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType></MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "stock",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",          //material ID
                                "yes"        //mixed
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Transaction lines for mixed cassette should not contain Material or Product. Instead of it, Material Type must be specified.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Transaction lines for mixed cassette should not contain Material or Product. Instead of it, Material Type must be specified.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Quantity is negative Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenQuantityIsNegative()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
                                    <Change>1</Change>
                                    <Fee>1</Fee>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "issue",     //transaction type
                                "100",       //value
                                "-10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1)
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Attribute 'Quantity' cannot have negative value.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Attribute 'Quantity' cannot have negative value.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Value is negative Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenValueIsNegative()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>
                                    <Change>1</Change>
                                    <Fee>1</Fee>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "issue",     //transaction type
                                "-100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1)
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Attribute 'Value' cannot have negative value.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Attribute 'Value' cannot have negative value.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Weight is negative Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenWeightIsNegative()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>	                                
                                    <Change>1</Change>
                                    <Fee>1</Fee>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
                                        <Weight>-1</Weight>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "issue",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1)
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Attribute 'Weight' cannot have negative value.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Attribute 'Weight' cannot have negative value.", response.Body.ErrorMessage);
        }

        [Fact(DisplayName = "Cash Point Transaction - When Residual Quantity is negative Then Validation of Transaction is Failed")]
        public void VerifyMessageWhenResidualQuantityIsNegative()
        {
            string transaction = string.Format(@"<?xml version='1.0' encoding='UTF-8' standalone='yes'?> 
                                <DocumentElement>
	                                <WP_CM_Transaction>
	                                <MachineNumber>{0}</MachineNumber> 
	                                <Type>{3}</Type>
	                                <TransactionNumber>{1}</TransactionNumber>
	                                <StartDate>{2}</StartDate>
                                    <EndDate>{10}</EndDate>
	                                <Currency>EUR</Currency> 	
	                                <Value>{4}</Value>
	                                <Quantity>{5}</Quantity>                                
                                    <Change>1</Change>
                                    <Fee>1</Fee>
	                                <PaymentMethod>pin</PaymentMethod>
	                                <Replenishment>{6}</Replenishment>
	                                <WP_CM_TransactionLine>
		                                <Type>{7}</Type>
		                                <StockPositionDirection>recycle</StockPositionDirection>
		                                <CassetteNumber>1</CassetteNumber>
		                                <Quantity>{5}</Quantity>
		                                <Value>{4}</Value>
		                                <MaterialID>{8}</MaterialID>
		                                <IsMixed>{9}</IsMixed>
		                                <MaterialType>NOTE</MaterialType>
                                        <ResidualQuantity>-1</ResidualQuantity>
	                                </WP_CM_TransactionLine>
	                                </WP_CM_Transaction>
                                </DocumentElement>",
                                _fixture.CashPoint.Number,
                                number,
                                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
                                "issue",     //transaction type
                                "100",       //value
                                "10",        //quantity
                                "no",        //replenishment
                                "collect",   //transaction line type
                                "313",       //material ID
                                "no",         //mixed
                                DateTime.Now.AddDays(1)
                                );

            var response = HelperFacade.FeedingHelper.SendFeeding(transaction);

            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains("Attribute 'ResidualQuantity' cannot have negative value.") && m.Result == ValidatedFeedingLogResult.Failed).Any());
            Assert.Equal("Attribute 'ResidualQuantity' cannot have negative value.", response.Body.ErrorMessage);
        }
    }
}