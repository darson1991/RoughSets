using System.Collections.Generic;

namespace BusinessLogic
{
    public class RoughSetObject
    {
        public List<double> Arguments { get; set; }
        public double Decision { get; set; }

        public RoughSetObject()
        {
            Arguments = new List<double>();
        } 
    }
}
