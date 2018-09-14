using Cwc.BaseData;
using Cwc.BaseData.Model;
using Cwc.Security;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace CWC.AutoTests.Helpers
{
    public class ConfigurationKeysHelper
    {

        public ConfigurationKey GetKey(Expression<Func<ConfigurationKey, bool>> expression)
        {
            var key = BaseDataFacade.ConfigurationKeyService.LoadBy(expression).FirstOrDefault();
            if (key == null)
            {
                throw new ArgumentNullException("Configuration key with current criteria is not found");
            }

            return key;
        }

        public void Update(ConfigurationKey key, string value)
        {
            var lr = SecurityFacade.LoginService.GetAdministratorLogin();
            key.Value = value;
            var result = BaseDataFacade.ConfigurationKeyService.Save(key, new UserParams(lr));
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Configuration setting updating failed. Reason: {result.GetMessage()}");
            }
        }
    }
}
