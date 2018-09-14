using Cwc.BaseData;
using Cwc.BaseData.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace CWC.AutoTests.Tests.Import.OrderImportFormatB
{
    public class PostCodesImportTests : IDisposable
    {
        BaseDataContext context;
        //TransactionScope scope;
        public PostCodesImportTests()
        {
            context = new BaseDataContext();
            //scope = new TransactionScope();
        }

        [Fact(DisplayName = "When post code action is insert Then System creates new post code")]
        public void VerifyThatNewPostCodeCanBeCreated()
        {
            var postCode = new PostCode { LetterCode = "1", ReferenceCode = 1, AreaCode = 1 };

            var result = BaseDataFacade.PostCodeService.Save(postCode);

            var foundPostCode = context.PostCodes.FirstOrDefault(x=>x.LetterCode == postCode.LetterCode && x.ReferenceCode == postCode.ReferenceCode && x.AreaCode == postCode.AreaCode);

            Assert.True(result.IsSuccess);
            Assert.NotNull(foundPostCode);
        }

        [Fact(DisplayName = "When post code action is update Then System updates post code")]
        public void VerifyThatPostCodeCanBeEdited()
        {

        }

        [Fact(DisplayName = "When post code action is delete Then System deletes post code")]
        public void VerifyThatPostCodeCanBeDEleted()
        {

        }

        void ImportFile()
        {

        }

        public void Dispose()
        {
            context.Dispose();
            //scope.Dispose();
        }
    }
}
