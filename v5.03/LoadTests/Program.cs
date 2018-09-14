using LoadTests.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = Console.ReadLine();
            if (command.StartsWith("run", StringComparison.OrdinalIgnoreCase))
            {
                var data = command.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                var testClass = data[1];
                var testMethod = data[2];
                TestRunner.Run(testClass, testMethod);
            }
        }
    }
}
