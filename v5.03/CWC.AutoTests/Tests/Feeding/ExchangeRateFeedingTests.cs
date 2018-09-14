using CWC.AutoTests.Helpers;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using CWC.AutoTests.ObjectBuilder.FeedingBuilder;
using System;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.Feeding
{
    public class ExchangeRateFeedingTests
    {

        private const string rate = "3";
        private const string incorrectRate = "3,3";
        private const string currencyFromCode = "GBP";
        private const string incorrectCurrencyFromCode = "SFAS";
        private const string newRate = "3.3";
        private static DateTime dateNow = DateTime.Now;
        private static string date = dateNow.ToString("yyyy-MM-dd HH:mm");
        private static string incorrectDate = dateNow.ToString("yyyyMMddHHmm");

        AutomationBaseDataContext exchangeRateContext;
        AutomationFeedingDataContext feedingContext;
        public ExchangeRateFeedingTests()
        {
            exchangeRateContext = new AutomationBaseDataContext();
            feedingContext = new AutomationFeedingDataContext();
        }

        public void Dispose()
        {
            exchangeRateContext.ExchangeRates.RemoveRange(exchangeRateContext.ExchangeRates.Where(er => er.CurrencyFromID == currencyFromCode && er.ExchangeDate == dateNow));
            exchangeRateContext.SaveChanges();
            exchangeRateContext.Dispose();
            feedingContext.ValidatedFeedingLogs.RemoveRange(feedingContext.ValidatedFeedingLogs);
            feedingContext.SaveChanges();
            feedingContext.Dispose();
        }


        [Fact(DisplayName = "Exchange Rates Feeding - Verify that exchange rate can be created")]
        public void CheckIfExchangeRatesCanBeCreated()
        {
            try
            {
                var exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                    With_ExchangeRate().
                        With_Rate(rate).
                        With_ExchangeDate(date).
                        With_CurrencyFromCode(currencyFromCode).
                        Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

                var message = $"Exchange rate with rate ‘{rate}’ on exchange date ‘{dateNow.ToString("dd.MM.yyyy HH:mm:00")}’ has been successfully created.";

                var exchangeRateActual = exchangeRateContext.ExchangeRates.Where(er => er.CurrencyFromID == currencyFromCode).ToList().OrderByDescending(er => er.ID).First();

                Assert.Equal(rate, ((double)exchangeRateActual.Rate).ToString());
                Assert.Equal(date, exchangeRateActual.ExchangeDate.ToString("yyyy-MM-dd HH:mm"));
                Assert.Equal(currencyFromCode, exchangeRateActual.CurrencyFromID.ToString());
                Assert.Equal("OK", response.Body.ValidationResult);
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Exchange Rates Feeding - Verify that exchange rate can be updated")]
        public void CheckIfExchangeRatesCanBeUpdated()
        {
            try
            {
                var exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                    With_ExchangeRate().
                        With_Rate(rate).
                        With_ExchangeDate(date).
                        With_CurrencyFromCode(currencyFromCode).
                        Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

                exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                    With_ExchangeRate().
                        With_Rate(newRate).
                        With_ExchangeDate(date).
                        With_CurrencyFromCode(currencyFromCode).
                        Build();

                response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

                var message = $"Exchange rate with rate ‘{newRate}’ on exchange date ‘{dateNow.ToString("dd.MM.yyyy HH:mm:00")}’ has been successfully updated.";

                var exchangeRateActual = exchangeRateContext.ExchangeRates.Where(er => er.CurrencyFromID == currencyFromCode).ToList().OrderByDescending(er => er.ID).First();

                Assert.Equal(newRate, ((double)exchangeRateActual.Rate).ToString());
                Assert.Equal(date, exchangeRateActual.ExchangeDate.ToString("yyyy-MM-dd HH:mm"));
                Assert.Equal(currencyFromCode, exchangeRateActual.CurrencyFromID.ToString());
                Assert.Equal("OK", response.Body.ValidationResult);
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Exchange Rates Feeding - Verify that exchange rate can not be created without rate")]
        public void CheckIfExchangeRatesCannotBeCreatedWithoutRate()
        {
            try
            {
                var exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                    With_ExchangeRate().
                        With_ExchangeDate(date).
                        With_CurrencyFromCode(currencyFromCode).
                        Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

                var message = $"Mandatory attribute 'Rate' is not submitted.";
                Assert.Equal("OK", response.Body.ValidationResult);
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Exchange Rates Feeding - Verify that exchange rate can not be created without exchange date")]
        public void CheckIfExchangeRatesCannotBeCreatedWithoutExchangeDate()
        {
            try
            {
                var exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                    With_ExchangeRate().
                        With_Rate(rate).
                        With_CurrencyFromCode(currencyFromCode).
                        Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

                var message = $"Mandatory attribute 'ExchangeDate' is not submitted.";
                Assert.Equal("OK", response.Body.ValidationResult);
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Exchange Rates Feeding - Verify that exchange rate can not be created without currency from code")]
        public void CheckIfExchangeRatesCannotBeCreatedWithoutCurrencyFromCode()
        {
            try
            {
                var exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                    With_ExchangeRate().
                        With_Rate(rate).
                        With_ExchangeDate(date).
                        Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

                var message = $"Mandatory attribute 'CurrencyFromCode' is not submitted.";
                Assert.Equal("OK", response.Body.ValidationResult);
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Exchange Rates Feeding - Verify that exchange rate can not be created with incorrect rate")]
        public void CheckIfExchangeRatesCannotBeCreatedWithIncorrectRate()
        {
            try
            {
                var exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                    With_ExchangeRate().
                        With_Rate(incorrectRate).
                        With_ExchangeDate(date).
                        With_CurrencyFromCode(currencyFromCode).
                        Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

                var message = $"Attribute 'Rate' format is broken, unable to parse value '{incorrectRate}'.";
                Assert.Equal("OK", response.Body.ValidationResult);
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Exchange Rates Feeding - Verify that exchange rate can not be created with incorrect Date")]
        public void CheckIfExchangeRatesCannotBeCreatedWithIncorrectExchangeRate()
        {
            try
            {
                var exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                    With_ExchangeRate().
                        With_Rate(rate).
                        With_ExchangeDate(incorrectDate).
                        With_CurrencyFromCode(currencyFromCode).
                        Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

                var message = $"Attribute 'ExchangeDate' format is broken, unable to parse value '{incorrectDate}'.";
                Assert.Equal("OK", response.Body.ValidationResult);
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Exchange Rates Feeding - Verify that exchange rate can not be created with incorrect CurrencyFromCode")]
        public void CheckIfExchangeRatesCannotBeCreatedWithIncorrectCurrencyFromCode()
        {
            try
            {
                var exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                    With_ExchangeRate().
                        With_Rate(rate).
                        With_ExchangeDate(date).
                        With_CurrencyFromCode(incorrectCurrencyFromCode).
                        Build();

                var response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

                var message = $"Currency with provided Code ‘{incorrectCurrencyFromCode}’ does not exist.";
                Assert.Equal("OK", response.Body.ValidationResult);
                Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
            }
            catch
            {
                throw;
            }
        }

        [Fact(DisplayName = "Exchange Rates Feeding - Verify that Replaced by sets correctly")]
        public void VerifyThatReplacedBySetsCorrectly()
        {
            var exchangeRate = FeedingBuilderFacade.ExchangeRateFeeding.New().
                With_ExchangeRate().
                    With_Rate(rate).
                    With_ExchangeDate(dateNow.AddMinutes(1).ToString("yyyy-MM-dd HH:mm")).
                    With_CurrencyFromCode(currencyFromCode).
                    Build();

            var response = HelperFacade.FeedingHelper.SendFeeding(exchangeRate.ToString());

            var exchangeRateActual = exchangeRateContext.ExchangeRates.Where(er => er.CurrencyFromID == currencyFromCode).ToList().OrderByDescending(er => er.ID).FirstOrDefault();

            var exchangeRateSecond = FeedingBuilderFacade.ExchangeRateFeeding.New().
                With_ExchangeRate().
                    With_Rate(newRate).
                    With_ExchangeDate(date).
                    With_CurrencyFromCode(currencyFromCode).
                    Build();

            response = HelperFacade.FeedingHelper.SendFeeding(exchangeRateSecond.ToString());
 
            var exchangeRateReplaced = exchangeRateContext.ExchangeRates.Where(er => er.CurrencyFromID == currencyFromCode).ToList().OrderByDescending(er => er.ID).FirstOrDefault();

            Assert.Equal(exchangeRateActual.ID, exchangeRateReplaced.ReplacedByID);

            var message = $"Currency with provided Code ‘{incorrectCurrencyFromCode}’ does not exist.";
            Assert.Equal("OK", response.Body.ValidationResult);
            Assert.True(feedingContext.ValidatedFeedingLogs.Where(m => m.Message.Contains(message)).Any());
        }
    }
}
