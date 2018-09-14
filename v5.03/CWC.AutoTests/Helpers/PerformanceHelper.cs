using System;
using System.IO;

namespace CWC.AutoTests.Helpers
{
    public static class PerformanceHelper
    {
        private static string folderPath = Environment.CurrentDirectory + @"\Logs\";
        private static string filePath = folderPath + @"PerformanceLog.txt";
        private static string exceptionLogPath = folderPath + @"ExceptionLog.txt";

        public static void SaveStartTime()
        {
            Directory.CreateDirectory(folderPath);
            
            using (var sw = new StreamWriter(filePath, true))
            {                
                sw.WriteLine("New Performance test is started at " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));                
            }
        }

        public static void SaveException(Exception e)
        {
            Directory.CreateDirectory(folderPath);

            using (var sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(e.GetType().FullName + " has occured on running test at " + DateTime.Now + ". You can see full details in Exception Log file.");                
                sw.WriteLine(string.Empty);
            }

            using (var sw = new StreamWriter(exceptionLogPath, true))
            {
                sw.WriteLine(e.GetType().FullName + " has occured on running test at " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                sw.WriteLine("Exception details:");
                sw.WriteLine("Message - " + e.ToString());
                sw.WriteLine("Source - " + e.Source);
                sw.WriteLine("Stack trace - " + e.StackTrace);
                sw.WriteLine(string.Empty);
                sw.WriteLine(string.Empty);
            }
        }

        public static void SaveNavigationPerformance(string name, string figure1, string figure2, string figure3, string figure4)
        {
            Directory.CreateDirectory(folderPath);

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine("Results of navigation performance for " + name + " (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")");
                if (figure1 != null)
                {
                    sw.WriteLine("Navigation to next page: " + figure1 + " seconds.");
                }
                if (figure2 != null)
                {
                    sw.WriteLine("Navigation to previous page: " + figure2 + " seconds.");
                }
                if (figure3 != null)
                {
                    sw.WriteLine("Navigation to last page: " + figure3 + " seconds.");
                }
                if (figure4 != null)
                {
                    sw.WriteLine("Navigation to first page: " + figure4 + " seconds.");
                }

                sw.WriteLine(string.Empty);
            }
        }

        public static void SaveFilterPerformance(string name, string figure)
        {
            Directory.CreateDirectory(folderPath);

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine("Results of filter applying performance for " + name + " (" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + ")");                
                sw.WriteLine(name + " has been loaded in " + figure + " seconds.");
                sw.WriteLine(string.Empty);
            }
        }
    }
}
