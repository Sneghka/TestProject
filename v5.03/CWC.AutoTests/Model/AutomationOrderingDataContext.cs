using Cwc.Common;
using Cwc.Ordering;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace CWC.AutoTests.Model
{
    public class AutomationOrderingDataContext : OrderingDataContext
    {

        static AutomationOrderingDataContext()
        {
            Database.SetInitializer<OrderingDataContext>(null);
        }
        public AutomationOrderingDataContext()
        {
        }

        public AutomationOrderingDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }

        public AutomationOrderingDataContext(DataBaseParams dbParams) : base(dbParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationOrderingDataContext"/> class.
        /// </summary>
        /// <param name="parentContext">Parent data context.</param>
        protected AutomationOrderingDataContext(AutomationOrderingDataContext parentContext)
            : base(parentContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractsDataContext"/> class from database parameters.
        /// </summary>
        /// <param name="dbCompiledModel">The <see cref="DbCompiledModel"/> .</param>
        /// <param name="dbParams">Database parameters</param>
        /// <param name="contextOwnsConnections">Default(false) If set to true the connection is disposed when the context is disposed, otherwise the caller must dispose the connection.</param>
        protected AutomationOrderingDataContext(DbCompiledModel dbCompiledModel, DataBaseParams dbParams, bool contextOwnsConnections = false)
            : base(dbCompiledModel, dbParams, contextOwnsConnections)
        {
        }

        public new void SaveChanges()
        {
            var result = base.SaveChanges();
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Unexpected error on saving an entity. Reason: {result.GetMessage()}");
            }
        }
    }
}
