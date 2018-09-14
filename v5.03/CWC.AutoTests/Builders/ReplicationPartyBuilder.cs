using Cwc.Common;
using Cwc.Replication;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class ReplicationPartyBuilder
	{
        AutomationBaseDataContext _context;
		DataBaseParams _dbParams;
        ReplicationParty entity;

		public ReplicationPartyBuilder()
		{
			 _dbParams = new DataBaseParams();
			 _context = new AutomationBaseDataContext();
		}

		public ReplicationPartyBuilder With_UID(Guid value)
		{
			if (entity != null) 
			{
				entity.UID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_Name(String value)
		{
			if (entity != null) 
			{
				entity.Name = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_PartyType(PartyType value)
		{
			if (entity != null) 
			{
				entity.PartyType = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_CreateDate(DateTime value)
		{
			if (entity != null) 
			{
				entity.CreateDate = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_UpdateDate(DateTime value)
		{
			if (entity != null) 
			{
				entity.UpdateDate = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_IsExportFrom(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsExportFrom = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_IsImportTo(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsImportTo = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_IsSendTicket(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsSendTicket = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_IsWaitForTicket(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsWaitForTicket = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_User_id(Int32? value)
		{
			if (entity != null) 
			{
				entity.User_id = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_ExportSettings(String value)
		{
			if (entity != null) 
			{
				entity.ExportSettings = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_ImportSettings(String value)
		{
			if (entity != null) 
			{
				entity.ImportSettings = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_PackageSize(Int32 value)
		{
			if (entity != null) 
			{
				entity.PackageSize = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_PackageTimeout(Int32 value)
		{
			if (entity != null) 
			{
				entity.PackageTimeout = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_Role(ReplicationRole? value)
		{
			if (entity != null) 
			{
				entity.Role = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_Number(Int32? value)
		{
			if (entity != null) 
			{
				entity.Number = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_PartyVersion(PartyVersion? value)
		{
			if (entity != null) 
			{
				entity.PartyVersion = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_IsDirectExport(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsDirectExport = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_IsActive(Boolean value)
		{
			if (entity != null) 
			{
				entity.IsActive = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_SiteIdentifier(String value)
		{
			if (entity != null) 
			{
				entity.SiteIdentifier = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

		public ReplicationPartyBuilder With_ID(Int32 value)
		{
			if (entity != null) 
			{
				entity.ID = value;
				return this;
			}					
			throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
		}

	
		public ReplicationPartyBuilder New()
		{
			entity = new ReplicationParty();
						
			return this;
		}

		public static implicit operator ReplicationParty(ReplicationPartyBuilder ins)
		{
			return ins.Build();
		}

		public ReplicationParty Build()
		{
			return entity;
		}

		public ReplicationPartyBuilder SaveToDb()
		{
            var result = ReplicationFacade.PartyService.Save(entity, false, null);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Replication Party saving failed. Reason: { result.GetMessage() }");
            }
			return this;
		}

		public ReplicationPartyBuilder Take(Func<ReplicationParty, bool> expression)
		{
            using (var context = new AutomationBaseDataContext())
            {
                var replicationParty = context.ReplicationParties.Where(expression).FirstOrDefault();

                replicationParty = replicationParty ?? throw new ArgumentNullException("ReplicationParty", "Replication Party is not found!");
                
                entity = ReplicationFacade.PartyService.Load(replicationParty.UID, null);
            }
            return this;
		}

		public void Dispose()
		{
			_context.Dispose();
		}
	}
}