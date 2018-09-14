using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CWC.AutoTests.Utils
{
    public static class ValueGenerator
    {
        public static string GenerateString(string prefix, int length)
        {
            var tempLength = length - prefix.Length;
            var secondPart = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, tempLength);

            return prefix + secondPart;
        }

        public static string GenerateNumber()
        {
            var r = new Random().Next(999999);
            return r.ToString();
        }
    }
}
