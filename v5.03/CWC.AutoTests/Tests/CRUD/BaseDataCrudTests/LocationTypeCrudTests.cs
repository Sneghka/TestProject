using Cwc.BaseData;
using Cwc.Security;
using CWC.AutoTests.Model;
using CWC.AutoTests.ObjectBuilder;
using System;
using System.Data;
using System.Linq;
using Xunit;

namespace CWC.AutoTests.Tests.CRUD.BaseDataCrudTests
{
    [Xunit.Collection("Mycollection")]
    public class LocationTypeCrudTests : IDisposable
    {
        string name, code;

        public LocationTypeCrudTests()
        {
            name = "AutoTestManagement";
            code = $"1101{new Random().Next(4000, 9999)}";
        }

        public void Dispose()
        {
            using (var context = new AutomationBaseDataContext())
            {
                context.LocationTypes.RemoveRange(context.LocationTypes.Where(lt => lt.ltCode.StartsWith(code)));
                context.SaveChanges();
            }
        }

        [Fact(DisplayName = "LocationType CRUD - LocationType was Created successfully")]

        public void VerifyThatLocationTypeWasCreatedSuccessfully()
        {
            var locationType = DataFacade.LocationType.New().
                With_ltCode(code).
                With_ltDesc(name).
                SaveToDb().
                Build();

            var locationTypeCreated = DataFacade.LocationType.Take(lt => lt.ltCode == code).Build();

            Assert.True(locationType.ltCode == locationTypeCreated.ltCode, "LocationType wasn't created");
            Assert.True(locationType.ltDesc == locationTypeCreated.ltDesc, "LocationType wasn't created");
        }

        [Fact(DisplayName = "LocationType CRUD - LocationType was Deleted successfully")]

        public void VerifyThatHandlingTypeWasDeletedSuccessfully()
        {
            var locationType = DataFacade.LocationType.New().
                With_ltCode(code).
                With_ltDesc(name).
                SaveToDb();

            DataFacade.LocationType.DeleteMany(lt => lt.ltCode == code);

            using (var context = new AutomationBaseDataContext())
            {
                var result = context.LocationTypes.FirstOrDefault(lt => lt.ltCode == code);
                Assert.True(result == null, "LocationType wasn't deleted");
            }
        }

        [Fact(DisplayName = "LocationType CRUD - LocationType was Updated successfully")]

        public void VerifyThatLocationTypeWasUpdatedSuccessfully()
        {
            var locationType = DataFacade.LocationType.New().
                With_ltCode(code).
                With_ltDesc(name).
                SaveToDb().
                Build();

            var locationTypeCreated = DataFacade.LocationType.Take(lt => lt.ltCode == code);
            locationTypeCreated.With_ltCode(code + "1").
                With_ltDesc(name + "1").
                SaveToDb();

            var locationTypeUpdated = DataFacade.LocationType.Take(lt => lt.ltCode == (code +"1")).Build();

            Assert.False(locationType.ltCode == locationTypeUpdated.ltCode, "Code wasn't updated");
            Assert.False(locationType.ltDesc == locationTypeUpdated.ltDesc, "Description wasn't updated");
        }

        [Fact(DisplayName = "LocationType CRUD - When LocationType was created without Code Then system shows error message")]

        public void VerifyThatLocationCannotBeCreatedWithoutCode()
        {
            var locationType = DataFacade.LocationType.New().
                With_ltCode(string.Empty).
                With_ltDesc(name);

            var result = BaseDataFacade.LocationTypeService.Save(locationType, null);
            Assert.False(result.IsSuccess, "LocationType saving should be unsuccessful");
            Assert.Equal("Value of property 'Code' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "LocationType CRUD - When LocationType was created without Description Then system shows error message")]

        public void VerifyThatLocationCannotBeCreatedWithoutDescription()
        {
            var locationType = DataFacade.LocationType.New().
                With_ltCode(code).
                With_ltDesc(string.Empty);

            var result = BaseDataFacade.LocationTypeService.Save(locationType, null);
            Assert.False(result.IsSuccess, "LocationType saving should be unsuccessful");
            Assert.Equal("Value of property 'Description' is not specified.", result.Messages.First());
        }

        [Fact(DisplayName = "LocationType CRUD - When LocationType was created with existed Code Then system shows error message")]

        public void VerifyThatDblicateLocationCannotBeCreated()
        {
            var locationTypeFirst = DataFacade.LocationType.New().
                With_ltCode(code).
                With_ltDesc(name).
                SaveToDb();                

            var locationTypeSecond = DataFacade.LocationType.New().
                With_ltCode(code).
                With_ltDesc(name);

            var userID = SecurityFacade.LoginService.GetAdministratorLogin().UserID;
            var result = BaseDataFacade.LocationTypeService.UserSave(locationTypeSecond.Build(), userID, null);
            Assert.False(result.IsSuccess, "Location Type saving should be unsuccessful");
            Assert.Equal("Location Type with specified Code value already exists. Please, specify other value.", result.Messages.First());
        }
    }
}
