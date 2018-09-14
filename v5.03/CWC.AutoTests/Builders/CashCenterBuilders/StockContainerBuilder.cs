using Cwc.BaseData;
using Cwc.CashCenter;
using Cwc.Common;
using Cwc.Security;
using CWC.AutoTests.Model;
using System;
using System.Linq;

namespace CWC.AutoTests.ObjectBuilder
{
    public class StockContainerBuilder : IDisposable
    {
        AutomationCashCenterDataContext _context;
        StockContainer entity;
        DataBaseParams _dbParams;
        DateTime? _created, _updated;


        public StockContainerBuilder()
        {   
            _dbParams = new DataBaseParams();
            _context = new AutomationCashCenterDataContext();
        }

        #region DataInit
        public StockContainerBuilder WithNumber(string number)
        {
            entity.SetNumber(number);

            return this;
        }

        public StockContainerBuilder WithPermanentNumber(string permNumber)
        {
            entity.PermanentNumber = permNumber;

            return this;
        }

        public StockContainerBuilder WithSecondNumber(string secNumber)
        {
            entity.SecondNumber = secNumber;

            return this;
        }

        public StockContainerBuilder WithLocationFrom(decimal location)
        {
            entity.LocationFrom_id = location;

            return this;
        }

        public StockContainerBuilder WithPreanType(PreannouncementType? type)
        {
            entity.SetPreannouncementType(type);

            return this;
        }

        public StockContainerBuilder WithType(StockContainerType type)
        {
            entity.SetType(type);

            return this;
        }

        public StockContainerBuilder WithTotalValue(int totalValue)
        {
            entity.TotalValue = totalValue;

            return this;
        }

        public StockContainerBuilder WithStockLocation(int location)
        {
            entity.StockLocation_id = location;

            return this;
        }

        public StockContainerBuilder WithLocationTo(decimal location)
        {
            entity.LocationTo_id = location;

            return this;
        }

        public StockContainerBuilder WithDateCollected(DateTime dateCollected)
        {
            entity.DateCollected = dateCollected;

            return this;
        }

        public StockContainerBuilder WithDateCreated(DateTime dateCreated)
        {
            _created = dateCreated;

            return this;
        }

        public StockContainerBuilder WithDateUpdated(DateTime dateUpdated)
        {
            _updated = dateUpdated;

            return this;
        }

        public StockContainerBuilder WithTransactiondate(DateTime? transactiondate)
        {
            entity.TransactionDate = transactiondate;

            return this;
        }

        public StockContainerBuilder WithStatus(SealbagStatus status)
        {
            entity.SetStatus(status);

            return this;
        }

        public StockContainerBuilder WithParentContainer(StockContainer parent)
        {
            entity.ParentContainer_id = parent.ID;

            return this;
        }

        public StockContainerBuilder WithTotalQuantity(int quantity)
        {
            entity.TotalQuantity = quantity;

            return this;
        }

        public StockContainerBuilder With_StockPositions(System.Collections.Generic.List<StockPosition> value)
        {
            if (entity != null)
            {
                entity.StockPositions = value;
                return this;
            }
            throw new InvalidOperationException("Make sure that New() or Take() methods are invoked first");
        }

        public StockContainer Build()
        {
            return entity;
        }

        /// <summary>
        /// Upon implicit casting Entity to EntityBuilder on this step, System will provide required actions of creating / updating
        /// </summary>
        /// <param name="ins"></param>
        public static implicit operator StockContainer(StockContainerBuilder ins)
        {
            return ins.Build();
        }
        #endregion

        #region Actions

        /// <summary>
        /// Method to save entity in DB. On any error will return an exception
        /// </summary>
        /// <param name="dbParams"></param>
        public StockContainerBuilder SaveToDb()
        {
            var container = entity;

            var userId = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
            UserParams userParams = new UserParams(userId);

            var res = CashCenterFacade.StockContainerService.Save(container, null, string.Empty, false, userParams, false, null, _dbParams);

            if (!res.IsSuccess)
            {
                throw new InvalidOperationException(res.GetMessage());
            }

            if (_created.HasValue)
            {
                this.ChangeDateCreated(_created.Value);
            }

            if (_updated.HasValue)
            {
                this.ChangeDateUpdated(_updated.Value);
            }


            return this;
        }

        /// <summary>
        /// Because of upon CWC->SaveEntity method, System sets DateCreated, DateUpdated by itself, this fields should be updated in another way (current implemetation is using EF) 
        /// </summary>
        /// <param name="date"></param>
        private void ChangeDateCreated(DateTime date)
        {
            var change = _context.StockContainers.FirstOrDefault(s => s.Number == entity.Number && s.PreannouncementType == entity.PreannouncementType);

            change.SetDateCreated(date);

            _context.SaveChanges();
        }

        /// <summary>
        /// The sane for ChangeDateCreated
        /// </summary>
        /// <param name="date"></param>
        private void ChangeDateUpdated(DateTime date)
        {
            var change = _context.StockContainers.FirstOrDefault(s => s.Number == entity.Number && s.PreannouncementType == entity.PreannouncementType);

            change.SetDateUpdated(date);

            _context.SaveChanges();
        }

        /// <summary>
        /// Method instatiates new entity. Upon entity builder operations one of New() or Take() should be used. If not, System'll throw an exception.
        /// </summary>
        /// <returns></returns>
        public StockContainerBuilder New()
        {
            entity = new StockContainer();

            return this;
        }

        /// <summary>
        /// Method to load an entity by some condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public StockContainerBuilder Take(Func<StockContainer, bool> expression)
        {
            entity = _context.StockContainers.FirstOrDefault(expression);

            if (entity == null)
            {
                throw new InvalidOperationException("Container wasn't found");
            }

            return this;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion

    }
}
