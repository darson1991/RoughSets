using System.Collections.Generic;

namespace BusinessLogic.Algorithms.Common
{
    public class Reduct
    {
        public string Chromosome { get; private set; }
        public List<int> Subset { get; private set; } 
        public List<AbstractClass> AbstractClasses { get; private set; }
        public List<DataObject> DataObjects { get; private set; }
        public double Approximation { get; private set; }

        public Reduct(string chromosome)
        {
            Chromosome = chromosome;
            GenerateSubset();
            GenerateDataObjectsForReduct();
            GenerateAbstractClasses();
        }


        public char GetChromosomeCharacter(int index)
        {
            return Chromosome[index];
        }

        public void ChangeChromosomeCharacter(int index, char character)
        {
            var charChromosomeArray = Chromosome.ToCharArray();
            charChromosomeArray[index] = character;
            Chromosome = new string(charChromosomeArray);
        }

        private void GenerateSubset()
        {
            Subset = new List<int>();

            for (var i = 0; i < Subset.Count; i++)
            {
                if (Subset[i] == '1')
                    Subset.Add(i);
            }
        }

        private void GenerateDataObjectsForReduct()
        {
            DataObjects = new List<DataObject>();
            if (Subset.Count == 0)
                return;

            //TODO: from clustered objects generate new data objects
        }

        private void GenerateAbstractClasses()
        {
            AbstractClasses = new List<AbstractClass>();
            if (Subset.Count == 0)
                return;


        }
    }
}
