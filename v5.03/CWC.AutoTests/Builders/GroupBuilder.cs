using System;
using System.Linq;
using Cwc.Common;
using Cwc.Security;
using CWC.AutoTests.DataModel;

namespace CWC.AutoTests.ObjectBuilder
{
    public class GroupBuilder : IDisposable
    {
        SecurityDataContext _context;
        DataBaseParams _dbParams;
        Group entity;

        public GroupBuilder()
        {
            _dbParams = new DataBaseParams();
            _context = new SecurityDataContext();
        }

        public GroupBuilder With_Comments(String value)
        {
            if (entity != null)
            {
                entity.Comments = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public GroupBuilder With_Name(String value)
        {
            if (entity != null)
            {
                entity.Name = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public GroupBuilder With_Status(GroupStatus value)
        {
            if (entity != null)
            {
                entity.Status = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public GroupBuilder With_DepartmentType(DepartmentType value)
        {
            if (entity != null)
            {
                entity.DepartmentType = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public GroupBuilder With_Type(GroupType value)
        {
            if (entity != null)
            {
                entity.Type = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public GroupBuilder With_ContactName(String value)
        {
            if (entity != null)
            {
                entity.ContactName = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public GroupBuilder With_ContactEmail(String value)
        {
            if (entity != null)
            {
                entity.ContactEmail = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public GroupBuilder With_ContactPhoneNumber(String value)
        {
            if (entity != null)
            {
                entity.ContactPhoneNumber = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public GroupBuilder With_ContactFaxNumber(String value)
        {
            if (entity != null)
            {
                entity.ContactFaxNumber = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public GroupBuilder With_DepartmentsLocationID(Decimal? value)
        {
            if (entity != null)
            {
                entity.DepartmentsLocationID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public GroupBuilder With_ID(Int32 value)
        {
            if (entity != null)
            {
                entity.ID = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }


        public GroupBuilder New()
        {
            entity = new Group();
            return this;
        }

        public static implicit operator Group(GroupBuilder ins)
        {
            return ins.Build();
        }

        public Group Build()
        {
            return entity;
        }

        public GroupBuilder SaveToDb()
        {
            var result = SecurityFacade.GroupService.Save(entity, _dbParams);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(string.Format("Group saving failed. Reason: {0}", result.GetMessage()));
            }
            return this;
        }

        public GroupBuilder Take(Func<Group, bool> expression)
        {
            entity = _context.Groups.Where(expression).FirstOrDefault();
            if (entity == null)
            {
                throw new ArgumentNullException("Group with provided criteria is not found.");
            }            
            return this;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}