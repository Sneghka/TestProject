using Cwc.BaseData;
using Cwc.CallManagement;
using Cwc.Coin;
using Cwc.Common;
using Cwc.Contracts;
using System;
using System.Linq.Expressions;

namespace CWC.AutoTests.ObjectBuilder
{
    public class CallBuilder
	{
		DataBaseParams _dbParams;
		Call entity;

		public CallBuilder()
		{
			 _dbParams = new DataBaseParams();
		}

		public CallBuilder With_Status(CallStatus value)
		{
			if (entity != null) 
			{
				entity.Status = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Priority(CallPriority value)
		{
			if (entity != null) 
			{
				entity.Priority = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Subject(String value)
		{
			if (entity != null) 
			{
				entity.Subject = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_DateOccurred(DateTime value)
		{
			if (entity != null) 
			{
				entity.DateOccurred = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_ReportedNumber(String value)
		{
			if (entity != null) 
			{
				entity.ReportedNumber = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_ExternalNumber(String value)
		{
			if (entity != null) 
			{
				entity.ExternalNumber = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_FailureDescription(String value)
		{
			if (entity != null) 
			{
				entity.FailureDescription = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_SolutionDescription(String value)
		{
			if (entity != null) 
			{
				entity.SolutionDescription = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Amount(Decimal? value)
		{
			if (entity != null) 
			{
				entity.Amount = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_AccountType(AccountType? value)
		{
			if (entity != null) 
			{
				entity.AccountType = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_AccountNumber(String value)
		{
			if (entity != null) 
			{
				entity.AccountNumber = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_PaymentMethod(PaymentMethod? value)
		{
			if (entity != null) 
			{
				entity.PaymentMethod = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_SealbagNumber(String value)
		{
			if (entity != null) 
			{
				entity.SealbagNumber = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_CashPointNumber(String value)
		{
			if (entity != null) 
			{
				entity.CashPointNumber = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Order_id(String value)
		{
			if (entity != null) 
			{
				entity.Order_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Operator_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.Operator_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_CallCategory_id(Int32 value)
		{
			if (entity != null) 
			{
				entity.CallCategory_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Requestor_id(Int32 value)
		{
			if (entity != null) 
			{
				entity.Requestor_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Company_id(Decimal value)
		{
			if (entity != null) 
			{
				entity.Company_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Location_id(Decimal value)
		{
			if (entity != null) 
			{
				entity.Location_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Client_id(Decimal? value)
		{
			if (entity != null) 
			{
				entity.Client_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_FailureCode_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.FailureCode_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_SolutionCode_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.SolutionCode_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_ServiceType_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.ServiceType_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Currency_code(String value)
		{
			if (entity != null) 
			{
				entity.Currency_code = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_Call_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.Call_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

        public CallBuilder With_DateResponded(DateTime? value)
		{
			if (entity != null) 
			{
				entity.DateResponded = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_DateFixStart(DateTime? value)
		{
			if (entity != null) 
			{
				entity.DateFixStart = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_DateFixEnd(DateTime? value)
		{
			if (entity != null) 
			{
				entity.DateFixEnd = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_DateCompleted(DateTime? value)
		{
			if (entity != null) 
			{
				entity.DateCompleted = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}


		public CallBuilder With_TimeViolated(Double? value)
		{
			if (entity != null) 
			{
				entity.TimeViolated = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_ExternalOrder(String value)
		{
			if (entity != null) 
			{
				entity.ExternalOrder = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_NewRequestor(Requestor value)
		{
			if (entity != null) 
			{
				entity.NewRequestor = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_AddNewRequestor(Boolean value)
		{
			if (entity != null) 
			{
				entity.AddNewRequestor = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public CallBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public CallBuilder New()
		{
			entity = new Call();
						
			return this;
		}

		public static implicit operator Call(CallBuilder ins)
		{
			return ins.Build();
		}

		public Call Build()
		{
			return entity;
		}

		public CallBuilder SaveToDb()
		{
            entity.SetNumber("-");
            var res = CallManagementFacade.CallService.SaveCall(entity, false, null, false, null, null, string.Empty, null);
            if (!res.IsSuccess)
            {
                throw new InvalidOperationException($"Call saving failed. Reason: {res.GetMessage()}");
            }

            return this;
		}

		public CallBuilder Take(Expression<Func<Call, bool>> expression)
		{
			throw new NotImplementedException("Take method should implemented manually");
			return this;
		}

	}
}