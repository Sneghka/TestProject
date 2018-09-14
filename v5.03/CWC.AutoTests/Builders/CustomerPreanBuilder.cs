using Cwc.Common;
using Cwc.Jobs;
using Cwc.MasterDataImport;
using Cwc.Security;
using Edsson.WebPortal.AutoTests.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Edsson.WebPortal.AutoTests.Helpers
{
    public class CustomerPreanBuilder
    { 

        #region Xml Tags
        const string CinNumber = "CINumber";
        const string CustomerCode = "CustomerCode";
        const string Barcode = "Barcode";
        const string TransactionDate = "TransactionDate";
        const string PreparationDate = "PreparationDate";
        const string CollectionDate = "CollectionDate";
        const string TransactionType = "TransactionType";
        const string TransportType = "TransportType";
        const string TitType = "TIType";
        const string PermanentKey = "PermanentKey";
        const string Amount = "Amount";
        const string Weight = "Weight";
        const string AccountNumber = "AccountNumber";
        const string Currency = "Currency";
        const string CurrencyType = "CurrencyType";
        const string ParentBarcode = "ParentBarcode";
        const string Seal = "Seal";
        const string Remark = "Remark";
        #endregion

        #region ctor
        ModelContext _context;
        XDocument _prean;
        XElement _announcementElement;
        string _fileName;
        string _directoty;
        public CustomerPreanBuilder()
        {
            _prean = new XDocument();
            _prean.Document.Add(new XElement(@"CWCAnnouncement", new XAttribute(XNamespace.Xmlns + "ns0", @"http://CWC.SL.IN.Announcement")));
            _fileName = $"CWCPreannouncement-{DateTime.Now.ToString("yyyyMMddhhmmssfff")}.xml";
            _directoty = @"D:\MasterDataImport\";
            _context = new ModelContext();
        } 
        #endregion

        #region Data Builders

        /// <summary>
        /// Init full prean structure with some default values
        /// </summary>
        /// <returns></returns>
        public CustomerPreanBuilder WithPreannouncement()
        {
            _announcementElement = new XElement("Announcement");
            _announcementElement.Add(new XElement(CinNumber));
            _announcementElement.Add(new XElement(CustomerCode));
            _announcementElement.Add(new XElement(Barcode));
            _announcementElement.Add(new XElement(TransactionDate));
            _announcementElement.Add(new XElement(PreparationDate));
            _announcementElement.Add(new XElement(CollectionDate));
            _announcementElement.Add(new XElement(TransactionType));
            _announcementElement.Add(new XElement(TransportType));
            _announcementElement.Add(new XElement(TitType));
            _announcementElement.Add(new XElement(PermanentKey));
            _announcementElement.Add(new XElement(Amount, 0));
            _announcementElement.Add(new XElement(Weight));
            _announcementElement.Add(new XElement(AccountNumber));
            _announcementElement.Add(new XElement(Currency, "EUR"));
            _announcementElement.Add(new XElement(CurrencyType, "Notes"));

            for (int i = 1; i <= 10; i++)
            {
                _announcementElement.Add(new XElement($"Quantity{i}", 0));
            }

            _announcementElement.Add(new XElement(ParentBarcode));
            _announcementElement.Add(new XElement(Seal));
            _announcementElement.Add(new XElement(Remark));

            _prean.Root.Add(_announcementElement);
            return this;
        }

        public CustomerPreanBuilder WithCinNumber(string cinNumber = "")
        {
            _announcementElement.Element(CinNumber).SetValue(cinNumber);

            return this;
        }

        public CustomerPreanBuilder WithCustomerCode(string customerCode = "")
        {
            _announcementElement.Element(CustomerCode).SetValue(customerCode);

            return this;
        }

        public CustomerPreanBuilder WithBarcode(string barcode = "")
        {
            _announcementElement.Element(Barcode).SetValue(barcode);

            return this;
        }

        public CustomerPreanBuilder WithTransactionDate(DateTime? transactionDate = null)
        {
            if (transactionDate.HasValue)
            {
                var date = transactionDate.Value;

                _announcementElement.Element(TransactionDate).SetValue(date.ToString("yyyy-MM-dd hh:mm:ss"));

                return this;
            }
            _announcementElement.Element(TransactionDate).SetValue(string.Empty);

            return this;
        }

        public CustomerPreanBuilder WithPreparationDate(DateTime preparationDate)
        {
            _announcementElement.Element(PreparationDate).SetValue(preparationDate.ToString("yyyy-MM-dd"));

            return this;
        }

        public CustomerPreanBuilder WithCollectionDate(DateTime? collectionDate = null)
        {
            if (collectionDate.HasValue)
            {
                var date = collectionDate.Value;

                _announcementElement.Element(CollectionDate).SetValue(date.ToString("yyyy-MM-dd"));

                return this;
            }

            _announcementElement.Element(CollectionDate).SetValue(string.Empty);

            return this;
        }

        public CustomerPreanBuilder WithTransactionType(string transactionType = "")
        {
            _announcementElement.Element(TransactionType).SetValue(transactionType);

            return this;
        }

        public CustomerPreanBuilder WithTransportType(string transportType = "")
        {
            _announcementElement.Element(TransportType).SetValue(transportType);

            return this;
        }

        public CustomerPreanBuilder WithTitType(string titType = "")
        {
            _announcementElement.Element(TitType).SetValue(titType);

            return this;
        }

        public CustomerPreanBuilder WithPermanentKey(string permanentKey = "")
        {
            _announcementElement.Element(PermanentKey).SetValue(permanentKey);

            return this;
        }

        public CustomerPreanBuilder WithAmount(int amount = 0)
        {
            _announcementElement.Element(Amount).SetValue(amount);

            return this;
        }

        public CustomerPreanBuilder WithWeight(decimal weight = 0)
        {
            _announcementElement.Element(Weight).SetValue(weight);

            return this;
        }

        public CustomerPreanBuilder WithAccountNumnber(string accountNumber = "")
        {
            _announcementElement.Element(AccountNumber).SetValue(accountNumber);

            return this;
        }

        public CustomerPreanBuilder WithCurrency(string currency = "")
        {
            _announcementElement.Element(Currency).SetValue(currency);

            return this;
        }

        public CustomerPreanBuilder WithCurrencyType(string currencyType = "")
        {
            _announcementElement.Element(CurrencyType).SetValue(currencyType);

            return this;
        }

        public CustomerPreanBuilder WithQuantity(int n, int quantity)
        {
            _announcementElement.Element($"Quantity{n}").SetValue(quantity);

            return this;
        }

        public CustomerPreanBuilder WithParentBarcode(string parentBarcode = "")
        {
            _announcementElement.Element(ParentBarcode).SetValue(parentBarcode);

            return this;
        }

        public CustomerPreanBuilder WithSealNumber(string sealNumber = "")
        {
            _announcementElement.Element(Seal).SetValue(sealNumber);

            return this;
        }

        public CustomerPreanBuilder WithRemark(string remark = "")
        {
            _announcementElement.Element(Remark).SetValue(remark);

            return this;
        }
        #endregion

        #region Actions
        /// <summary>
        /// 1. Create settings for the job
        /// 2. Run Master Data Import job if it's stopped
        /// 3. Puts the XML file to the specified folder
        /// </summary>
        /// <param name="fileName">parameter is required to store the file name. This is used to find logs in the DB upon assertion</param>
        /// <returns></returns>
        public CustomerPreanBuilder Import(out string fileName)
        {
            
            fileName = _fileName;

            var job = new MasterDataImportIntegrationJob();
            //var settings = MasterDataImportIntegrationFacade.SettingsService.LoadSettings(new Cwc.Common.DataBaseParams());

            var sttgs = job.LoadSettings(JobsFacade.JobInstanceService.Load(49, new DataBaseParams()), new DataBaseParams()).First();

            if (sttgs == null)
            {
                throw new Exception("settings");
            }

            if (!Directory.Exists(_directoty))
            {
                Directory.CreateDirectory(_directoty);
            }

            sttgs.ExchangeFolder = _directoty;

            try
            {
                StartJob();
                RunIfStopped();

                WriteFile(this._prean, $"{sttgs.ExchangeFolder}{_fileName}");

                return this;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Wait until file will be loaded. This method should be used ONLY after Import() method, 
        /// because Wait doesn't take into account wheter Master Data Import job is running (yet)
        /// </summary>
        /// <returns></returns>
        public CustomerPreanBuilder Wait()
        {
            while (File.Exists($"{_directoty}\\{_fileName}"))
            {
            }

            return this;
        }
        #endregion

        #region Private Methods
        private void WriteFile(XDocument doc, string path)
        {
            if (doc == null)
            {
                throw new Exception("File cannot be null");
            }

            if (string.IsNullOrEmpty(path))
            {
                throw new Exception("Invalid path specified");
            }

            doc.Save(path);
        }

        private void RunIfStopped()
        {
            var res = (from job in _context.WP_Jobs_JobInstance
                       join info in _context.WP_Jobs_JobInfo on job.JobInfo_id equals info.id
                       where info.Name.Contains("Master Data Import")
                       select job).First();

            if (res.Status == 0)
            {
                res.Status = 1;
                _context.SaveChanges();
            }
        }
        #endregion

        private void StartJob()
        {

            //var dbParams = new DataBaseParams();

            //var jobInfo = _context.WP_Jobs_JobInfo.Where(x => x.Name.Contains("Master Data Import")).First();
            //var mdiJob = JobsFacade.JobInstanceService.LoadAll(dbParams).Where(x => x.JobInfo_id == jobInfo.id).First();

            //var servDB = _context.WP_Jobs_Server.First();
            //var serverDB = JobsFacade.ServerService.Load(servDB.id, dbParams);

            //JobsFacade.ServerService.SetServerIsAvailable(serverDB.ID, dbParams);
            //JobsFacade.ServerService.UpdateStatus(serverDB.ID, ServerStatus.Available, dbParams);

            //var stat = JobsFacade.JobInstanceService.GetCurrentStatusOfJob(jobInfo.id, serverDB.Name, dbParams);

            //var res = JobsFacade.JobInstanceService.StartJobInstance(mdiJob.ID, true, SecurityFacade.LoginService.GetAdministratorLogin(), dbParams);

            //stat = JobsFacade.JobInstanceService.GetCurrentStatusOfJob(jobInfo.id, serverDB.Name, dbParams);

            //JobsFacade.JobInstanceService.UpdateStatus(mdiJob.ID, JobInstanceStatus.Running, dbParams);

            //stat = JobsFacade.JobInstanceService.GetCurrentStatusOfJob(jobInfo.id, serverDB.Name, dbParams);
        }
    }
}
