using System.Collections.Generic;

namespace BusinessLogic
{
    public class ClusteredDataObject
    {
        public List<int> Arguments { get; set; }
        public double Decision { get; set; }

        public ClusteredDataObject()
        {
            Arguments = new List<int>();
        }
    }
}
