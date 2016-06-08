using System.Collections.Generic;

namespace BusinessLogic.Algorithms.Common
{
    public class AbstractClass
    {
        public List<int> ObjectsIndexes { get; private set; }
        public List<int> ArgumentsValues { get; set; }
        public bool IsClear { get; set; }
        public double? Decision { get; set; }

        public AbstractClass()
        {
            ObjectsIndexes = new List<int>();
            ArgumentsValues = new List<int>();
            IsClear = true;
        }
    }
}
