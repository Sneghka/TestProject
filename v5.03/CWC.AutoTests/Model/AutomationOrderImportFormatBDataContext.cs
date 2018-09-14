using Cwc.Common;
using Cwc.Integration.OrderImportFormatB;
using System;
using System.Data.Entity.Infrastructure;

namespace CWC.AutoTests.Model
{
    public class AutomationOrderImportFormatBDataContext : OrderImportFormatBDataContext
    {
        public AutomationOrderImportFormatBDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }
        public AutomationOrderImportFormatBDataContext()
        {
        }

        public AutomationOrderImportFormatBDataContext(DataBaseParams dbParams) : base(dbParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationOrderingDataContext"/> class.
        /// </summary>
        /// <param name="parentContext">Parent data context.</param>
        protected AutomationOrderImportFormatBDataContext(AutomationOrderImportFormatBDataContext parentContext)
            : base(parentContext)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractsDataContext"/> class from database parameters.
        /// </summary>
        /// <param name="dbCompiledModel">The <see cref="DbCompiledModel"/> .</param>
        /// <param name="dbParams">Database parameters</param>
        /// <param name="contextOwnsConnections">Default(false) If set to true the connection is disposed when the context is disposed, otherwise the caller must dispose the connection.</param>
        protected AutomationOrderImportFormatBDataContext(DbCompiledModel dbCompiledModel, DataBaseParams dbParams, bool contextOwnsConnections = false)
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
