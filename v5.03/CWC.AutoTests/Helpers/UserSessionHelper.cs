using CWC.AutoTests.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CWC.AutoTests.Helpers
{
    public class UserSessionHelper
    {
        public static List<UserSessionSetting> GetUserSessionSettings()
        {
            var userSessions = new List<UserSessionSetting>();
            var userConfig = new XmlDocument();
            userConfig.Load("UserConfig.xml");
            foreach (XmlNode node in userConfig.DocumentElement.ChildNodes)
            {
                var login = node.Attributes["login"].Value;
                var pwd = node.Attributes["password"].Value;
                var ws = node.Attributes["workstation"].Value;
                var preanSource = node.Attributes["preanSource"].Value;
                if (string.IsNullOrEmpty(preanSource))
                {
                    Console.WriteLine("Skip user " + login + " because no prean source!");
                    continue;
                }

                var feedingsFolder = node.Attributes["feedingsFolder"] != null ? node.Attributes["feedingsFolder"].Value : null;
                var userSession =
                    new UserSessionSetting { Login = login, Password = pwd, Workstation = ws, FeedingsFolder = feedingsFolder, PreannouncementSource = preanSource };
                userSessions.Add(userSession);
            }

            return userSessions;
        }
    }
}
