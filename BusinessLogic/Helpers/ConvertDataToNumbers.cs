using System.Collections.Generic;

namespace BusinessLogic.Helpers
{
    public class ConvertDataToNumbers
    {
        private List<string> _stringColumnsNumber;

        public void PrepareListOfStringColumns(string line)
        {
            _stringColumnsNumber = new List<string>();
            var dataList = line.Split(',');
            foreach (var data in dataList)
            {
                
            }
        }
    }
}
