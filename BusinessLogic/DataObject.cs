using System.Collections.Generic;

namespace BusinessLogic
{
    public class DataObject
    {
        public List<double> Arguments { get; set; }
        public double Decision { get; set; }

        public DataObject()
        {
            Arguments = new List<double>();
        }
    }
}
