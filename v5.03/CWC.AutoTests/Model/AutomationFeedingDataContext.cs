using Cwc.BaseData;
using Cwc.Common;
using Cwc.Feedings;
using System;
using System.Data.Entity;

namespace CWC.AutoTests.Model
{
    /// <summary>
    /// Data context with Feedings entities.
    /// </summary>
    public class AutomationFeedingDataContext : FeedingsDataContext
    {
        #region Constructors
        static AutomationFeedingDataContext()
        {
            Database.SetInitializer<AutomationFeedingDataContext>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationFeedingDataContext"/> class.
        /// </summary>
        public AutomationFeedingDataContext()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationFeedingDataContext"/> class from database parameters.
        /// </summary>
        /// <param name="dbParams">Database parameters.</param>
        protected AutomationFeedingDataContext(DataBaseParams dbParams) : base(dbParams)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationFeedingDataContext"/> class from parent context.
        /// </summary>
        /// <param name="parentContext">Parent data context.</param>
        protected AutomationFeedingDataContext(AutomationFeedingDataContext parentContext): base(parentContext)
        {

        }

        public AutomationFeedingDataContext(bool exportEnabled = false) : base()
        {
            ExportEnabled = exportEnabled;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the <see cref="DbSet{ValidatedFeedingLog}"/>
        /// </summary>
        public DbSet<AutomationValidatedFeedingLog> ValidatedFeedingLogs { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{ValidatedFeeding}"/>
        /// </summary>
        public DbSet<ValidatedFeeding> ValidatedFeedings { get; set; }
        #endregion

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
