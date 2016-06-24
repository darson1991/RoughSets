using System;
using System.Text;

namespace BusinessLogic.Helpers
{
    public static class BinaryStringHelper
    {
        public static string ConvertIntToBinaryString(long value, int length)
        {
            return Convert.ToString(value, 2).PadLeft(length, '0');
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

        public static string GenerateNeighborSolution(string individual)
        {
            var random = new Random();
            var index = random.Next(individual.Length);
            var indexValue = individual[index] == '0' ? '1' : '0';
            return individual.Substring(0, index) + indexValue + individual.Substring(index + 1);
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
