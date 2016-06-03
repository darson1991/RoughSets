using System.Collections.Generic;

namespace BusinessLogic.RoughSetsHelperObjects
{
    public class Reduct
    {
        public List<int> Subset { get; set; } 
        public List<AbstractClass> AbstractClasses { get; set; }
        public List<DataObject> DataObjects { get; set; }
        public double Approximation { get; set; }

        public Reduct()
        {
            Subset = new List<int>();
            AbstractClasses = new List<AbstractClass>();
            DataObjects = new List<DataObject>();
        } 
    }
}
