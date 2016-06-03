using System.Collections.Generic;

namespace BusinessLogic.RoughSetsHelperObjects
{
    public class AbstractClass
    {
        public List<int> ObjectsIndexes { get; set; }
        public List<double> ArgumentsValues { get; set; }
        public bool IsClear { get; set; }
        public double? Decision { get; set; }

        public AbstractClass()
        {
            ObjectsIndexes = new List<int>();
            ArgumentsValues = new List<double>();
            IsClear = true;
        }
    }
}
