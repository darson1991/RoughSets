using System;

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
    }
}
