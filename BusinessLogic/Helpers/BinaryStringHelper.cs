using System;
using System.Linq;

namespace BusinessLogic.Helpers
{
    public static class BinaryStringHelper
    {
        public static string ConvertIntToBinaryString(int value, int length)
        {
            return Convert.ToString(value, 2).PadLeft(length, '0');
        }

        public static int ConvertBinaryStringToInt(string binaryString)
        {
            return Convert.ToInt32(binaryString, 2);
        }

        public static string GenerateRandomIndividual(int length)
        {
            const string chars = "01";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
