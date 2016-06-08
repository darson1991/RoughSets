using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Algorithms.Common
{
    public class Reduct
    {
        public string Chromosome { get; private set; }
        public List<int> Subset { get; private set; } 
        public List<AbstractClass> AbstractClasses { get; private set; }
        public List<ClusteredDataObject> ReductDataObjects { get; private set; }
        public double Approximation { get; private set; }

        public Reduct(string chromosome, List<ClusteredDataObject> clusteredDataObjects)
        {
            Chromosome = chromosome;
            GenerateSubset();
            GenerateDataObjectsForReduct(clusteredDataObjects);
            GenerateAbstractClasses();
            CalculateApproximation();
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

        private void GenerateDataObjectsForReduct(List<ClusteredDataObject> clusteredDataObjects)
        {
            ReductDataObjects = new List<ClusteredDataObject>();
            if (Subset.Count == 0)
                return;

            foreach (var clusteredDataObject in clusteredDataObjects)
            {
                var reductDataObject = new ClusteredDataObject
                {
                    Decision = clusteredDataObject.Decision
                };

                for (var i = 0; i < clusteredDataObject.Arguments.Count; i++)
                {
                    if (Subset.Contains(i))
                        reductDataObject.Arguments.Add(clusteredDataObject.Arguments[i]);
                }

                ReductDataObjects.Add(reductDataObject);
            }
        }

        private void GenerateAbstractClasses()
        {
            AbstractClasses = new List<AbstractClass>();
            if (Subset.Count == 0)
                return;

            for (var i = 0; i < ReductDataObjects.Count; i++)
            {
                var reductDataObject = ReductDataObjects[i];
                var arguments = Subset.Select(s => reductDataObject.Arguments[s]).ToList();

                var isNewAbstractClass = true;

                foreach (var abstractClass in AbstractClasses)
                {
                    if (!abstractClass.ArgumentsValues.SequenceEqual(arguments))
                        continue;

                    if (abstractClass.Decision != reductDataObject.Decision)
                        abstractClass.IsClear = false;
                    else
                    {
                        isNewAbstractClass = false;
                        abstractClass.ObjectsIndexes.Add(i);
                    }
                }

                if (!isNewAbstractClass)
                    continue;
                
                var newAbstractClass = new AbstractClass
                {
                    ArgumentsValues = arguments,
                    Decision = reductDataObject.Decision
                };
                newAbstractClass.ObjectsIndexes.Add(i);

                AbstractClasses.Add(newAbstractClass);
            }
        }

        private void CalculateApproximation()
        {
            throw new System.NotImplementedException();
        }
    }
}
