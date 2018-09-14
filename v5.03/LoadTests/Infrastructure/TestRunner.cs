using Edsson.WebPortal.AutoTests.Tests;
using System;

namespace LoadTests.Infrastructure
{
    public static class  TestRunner
    {
        public static void Run(string testClass, string testMethod)
        {
            var type = GetTestType(testClass);
            using (var test = (LoadTest)Activator.CreateInstance(type, null))
            {
                test.Run(testMethod);
                return;
            }

            throw new Exception(testClass + " is not a load test class");           
        }

        public static Type GetTestType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type == null)
            {
                type = Type.GetType("LoadTests.Test." + typeName);
            }

            if (type == null)
            {
                throw new Exception("No test class " + typeName + " found.");
            }

            return type;
        }
    }
}
