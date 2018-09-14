using Cwc.Common;
using Cwc.Feedings;
using System;

namespace CWC.AutoTests.Model
{
    /// <summary>
    /// Order Line Product.
    /// </summary>
    [Serializable]
    [ClassDescription("Cwc_Feedings_ValidatedFeedingLog")]
    public class AutomationValidatedFeedingLog : BaseEntity
    {

        public AutomationValidatedFeedingLog()
        {
        }

        #region Property 'ValidatedFeedingID'

        /// <summary>
        /// Gets or sets ValidatedFeedingLog identifier
        /// </summary>
        [PropertyDescription("ValidatedFeedingID", IsNullable = false)]
        public int ValidatedFeedingID
        {
            get { return this.GetPropertyValue<int>("ValidatedFeedingID"); }
            set { this.SetPropertyValue(value, "ValidatedFeedingID"); }
        }
        #endregion

        #region Property 'ObjectID'
        /// <summary>
        /// Gets or sets ObjectID identifier.
        /// </summary>
        [PropertyDescription("ObjectID", ColumnAlias = "ObjectID", IsNullable = true)]
        public long? ObjectID
        {
            get { return this.GetPropertyValue<long>("ObjectID"); }
            set { this.SetPropertyValue(value, "ObjectID"); }
        }
        #endregion

        #region Property 'ObjectClassID'

        [PropertyDescription("ObjectClassID", IsNullable = true)]
        public int? ObjectClassID
        {
            get { return this.GetPropertyValue<int>("ObjectClassID"); }
            set { this.SetPropertyValue(value, "ObjectClassID"); }
        }
        #endregion

        #region Property 'DateCreated'

        [PropertyDescription("DateCreated")]
        public DateTime DateCreated
        {
            get { return this.GetPropertyValue<DateTime>("DateCreated"); }
            set { this.SetPropertyValue(value, "DateCreated"); }
        }
        #endregion

        #region Property 'Result'

        [PropertyDescription("Result", IsNullable = true)]
        public ValidatedFeedingLogResult Result
        {
            get { return this.GetPropertyValue<ValidatedFeedingLogResult>("Result"); }
            set { this.SetPropertyValue(value, "Result"); }
        }
        #endregion

        #region Property 'Action'

        [PropertyDescription("Action")]
        public ValidatedFeedingActionType? Action
        {
            get { return this.GetPropertyValue<ValidatedFeedingActionType>("Action"); }
            set { this.SetPropertyValue(value, "Action"); }
        }
        #endregion

        #region Property 'Message'

        [PropertyDescription("Message", IsNullable = true)]
        public string Message
        {
            get { return this.GetPropertyValue<string>("Message"); }
            set { this.SetPropertyValue(value, "Message"); }
        }
        #endregion

    }
}