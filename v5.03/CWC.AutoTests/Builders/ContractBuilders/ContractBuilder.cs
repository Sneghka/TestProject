using Cwc.BaseData;
using Cwc.Contracts;
using CWC.AutoTests.Model;
using Cwc.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ContractBuilder
    {
        Contract entity;

        public ContractBuilder()
        {
        }
        public ContractBuilder With_IsDefault(Boolean value)
        {
            if (entity != null)
            {
                entity.SetIsDefault(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_Number(String value)
        {
            if (entity != null)
            {
                entity.Number = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_Date(DateTime value)
        {
            if (entity != null)
            {
                entity.Date = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        /// New contract can only have "Draft" status. Status of the finalized contract cannot be changed.
		private ContractBuilder With_Status(ContractStatus value)
        {
            if (entity != null)
            {
                entity.Status = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_StartDate(DateTime value)
        {
            if (entity != null)
            {
                entity.StartDate = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_EndDate(DateTime? value)
        {
            if (entity != null)
            {
                entity.EndDate = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_Customer_id(Decimal? value)
        {
            if (entity != null)
            {
                entity.CustomerID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_Currency_code(String value)
        {
            if (entity != null)
            {
                entity.Currency_code = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_InterestRate(Decimal value)
        {
            if (entity != null)
            {
                entity.InterestRate = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_CustomerType(CustomerType value)
        {
            if (entity != null)
            {
                entity.CustomerType = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_ActualCounterparty_id(Decimal? value)
        {
            if (entity != null)
            {
                entity.ActualCounterparty_id = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_Debtor_id(Decimal? value)
        {
            if (entity != null)
            {
                entity.Debtor_id = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_BankAccount_id(Int32? value)
        {
            if (entity != null)
            {
                entity.BankAccount_id = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_EffectiveDate(DateTime value)
        {
            if (entity != null)
            {
                entity.EffectiveDate = value;
            }
            return this;
        }

        public ContractBuilder With_IsLatestRevision(Boolean value)
        {
            if (entity != null)
            {
                entity.IsLatestRevision = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_RevisionNumber(Int32? value)
        {
            if (entity != null)
            {
                entity.SetRevisionNumber(value);
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_RevisionDate(DateTime? value)
        {
            if (entity != null)
            {
                entity.RevisionDate = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_ReplacedRevisionNumber(Int32? value)
        {
            if (entity != null)
            {
                entity.ReplacedRevisionNumber = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_ReplacedRevisionDate(DateTime? value)
        {
            if (entity != null)
            {
                entity.ReplacedRevisionDate = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_AuthorID(Int32? value)
        {
            if (entity != null)
            {
                entity.AuthorID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_LatestRevisionID(Int32? value)
        {
            if (entity != null)
            {
                entity.LatestRevisionID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_Contract(Contract value)
        {
            if (entity != null)
            {
                entity.Contract = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public ContractBuilder New()
        {
            entity = new Contract();
            With_Status(ContractStatus.Draft);
            return this;
        }

        public static implicit operator Contract(ContractBuilder ins) => ins.Build();

        public Contract Build() => entity;

        public ContractBuilder SaveToDb()
        {
            var loginResult = SecurityFacade.LoginService.GetAdministratorLogin();
            var result = ContractsFacade.ContractService.Save(entity, new UserParams(loginResult), null);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("Contract saving failed. Reason: {0}", result.GetMessage()));
            }
            return this;
        }

        public ContractBuilder Take(Expression<Func<Contract, bool>> expression, AutomationContractDataContext dataContext = null)
        {
            using (var newContext = new AutomationContractDataContext())
            {
                var context = dataContext ?? newContext;
                entity = context.Contracts.FirstOrDefault(expression);

                if (entity == null)
                {
                    throw new InvalidOperationException("Contract is not found!");
                }
            }

            return this;
        }

        /// <summary>
        /// Method will create all contract data that is required for order creation for specific contract and service types
        /// </summary>
        /// <param name="contractId">Id of contract that should be adjusted</param>
        /// <param name="serviceTypeIds">Id's of processed service types</param>
        public void CreateDefaultSettingsForOrder(Contract contract, IEnumerable<int> serviceTypeIds)
        {
            var isClearingRequired = true;

            foreach (var serviceTypeId in serviceTypeIds)
            {
                DataFacade.ContractOrderingSetting.New()
                    .With_Contract_id(contract.ID)
                    .With_ServiceType_id(serviceTypeId)
                    .With_LocationType_code(null)
                    .With_Location_id(null)
                    .With_AllowTotalPreannouncement(true)
                    .With_IsCoins(true)
                    .With_IsConsumables(true)
                    .With_IsLatestRevision(true)
                    .With_IsNotes(true)
                    .With_IsPreAnnouncement(true)
                    .With_IsServicingCodes(true)
                    .With_CCLeadTime(0)
                    .With_CITLeadTime(0)
                    .With_CutOffTime(0.145)
                    .With_LeadTime(0)
                    .With_IsCoinsLooseProductDelivery(false)
                    .With_IsNotesLooseProductDelivery(false)
                    .SaveToDb(isClearingRequired);
                
                DataFacade.ScheduleSetting.New()
                    .With_Contract_id(contract.ID)
                    .With_Location_id(null)
                    .With_ServiceTypeID(serviceTypeId)
                    .With_PeriodStartDate(DateTime.Now)
                    .With_IsLatestRevision(true)
                    .With_ScheduleLines(new List<ScheduleLine> {
                        new ScheduleLine { WeekdayName = WeekdayName.Monday, Contract = contract },
                        new ScheduleLine { WeekdayName = WeekdayName.Tuesday, Contract = contract },
                        new ScheduleLine { WeekdayName = WeekdayName.Wednesday, Contract = contract },
                        new ScheduleLine { WeekdayName = WeekdayName.Thursday, Contract = contract },
                        new ScheduleLine { WeekdayName = WeekdayName.Friday, Contract = contract },
                        new ScheduleLine { WeekdayName = WeekdayName.Saturday, Contract = contract },
                        new ScheduleLine { WeekdayName = WeekdayName.Sunday, Contract = contract }
                    }).SaveToDb(isClearingRequired);

                isClearingRequired = false;
                // this will allow not to clear all data every loop, but just before first
            }
        }
    }
}