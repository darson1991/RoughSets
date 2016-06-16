using System;
using System.Linq;
using System.Text;

namespace BusinessLogic.Helpers
{
    public static class BinaryStringHelper
    {
        public static string ConvertIntToBinaryString(long value, int length)
        {
            return Convert.ToString(value, 2).PadLeft(length, '0');
        }

        public static long ConvertBinaryStringToInt(string binaryString)
        {
            return Convert.ToInt32(binaryString, 2);
        }

        public static string GenerateRandomIndividual(int length)
        {
            var random = new Random();

            var individual = new StringBuilder();
            for (var i = 0; i < length; i++)
                individual.Append(random.Next(2));

            Console.WriteLine(individual.ToString());
            return individual.ToString();
        }

        public static string GenerateIndividualWithAllAttributes(int length)
        {
            var individual = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                individual.Append('1');
            }
            return individual.ToString();
        }
    }
}
