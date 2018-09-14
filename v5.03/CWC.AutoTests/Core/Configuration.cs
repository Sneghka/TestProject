using System;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CWC.AutoTests.Core
{
    public static class Configuration
    {   
        public static string Browser
        {
            get
            {
                return ConfigurationManager.AppSettings["browser"];
            }
        }

        public static string Portal
        {
            get
            {
                return ConfigurationManager.AppSettings["portal"];
            }
        }

        public static string Username
        {
            get
            {
                return ConfigurationManager.AppSettings["username"];
            }
        }

        public static string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["password"];
            }
        }

        public static string Workstation
        {
            get
            {
                return ConfigurationManager.AppSettings["workstation"];
            }
        }

        public static string StockLocationName
        {
            get
            {
                return ConfigurationManager.AppSettings["stockLocationName"];
            }
        }

        public static string StockLocationDescription
        {
            get
            {
                return ConfigurationManager.AppSettings["stockLocationDescription"];
            }
        }

        public static string LocationCode
        {
            get
            {
                return ConfigurationManager.AppSettings["locationCode"];
            }
        }

        public static string CompanyCode
        {
            get
            {
                return ConfigurationManager.AppSettings["companyCode"];
            }
        }

        public static string CompanyName
        {
            get
            {
                return ConfigurationManager.AppSettings["companyName"];
            }
        }

        public static string SiteCode
        {
            get
            {
                return ConfigurationManager.AppSettings["siteCode"];
            }
        }

        public static string OperatorInfo
        {
            get
            {
                return ConfigurationManager.AppSettings["operatorInfo"];
            }
        }

        public static string LocationFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["locationFrom"];
            }
        }                
    }
}