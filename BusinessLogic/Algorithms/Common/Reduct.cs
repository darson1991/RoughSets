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
                var reductDataObject = CreateReductDataObject(clusteredDataObject);

                ReductDataObjects.Add(reductDataObject);
            }
        }

        private ClusteredDataObject CreateReductDataObject(ClusteredDataObject clusteredDataObject)
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
            return reductDataObject;
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

                bool isNewAbstractClass;
                bool isClearAbstractClass;
                UpdateExistingAbstractClassesWithTheSameArguments(arguments, reductDataObject, i, out isNewAbstractClass, out isClearAbstractClass);

                if (!isNewAbstractClass)
                    continue;
                
                var newAbstractClass = CreateNewAbstractClass(arguments, reductDataObject, i, isClearAbstractClass);

                AbstractClasses.Add(newAbstractClass);
            }
        }

        private void UpdateExistingAbstractClassesWithTheSameArguments(List<int> arguments, ClusteredDataObject reductDataObject,
            int indexOfReductDataObject, out bool isNewAbstractClass, out bool isClearAbstractClass)
        {
            isClearAbstractClass = true;
            isNewAbstractClass = true;
            foreach (var abstractClass in AbstractClasses)
            {
                if (!abstractClass.ArgumentsValues.SequenceEqual(arguments))
                    continue;

                isClearAbstractClass = false;
                if (abstractClass.Decision != reductDataObject.Decision)
                    abstractClass.IsClear = false;
                else
                {
                    isNewAbstractClass = false;
                    abstractClass.ObjectsIndexes.Add(indexOfReductDataObject);
                }
            }
        }

        private static AbstractClass CreateNewAbstractClass(List<int> arguments, ClusteredDataObject reductDataObject, 
            int indexOfReductDataObject, bool isClearAbstractClass)
        {
            var newAbstractClass = new AbstractClass
            {
                ArgumentsValues = arguments,
                Decision = reductDataObject.Decision,
                IsClear = isClearAbstractClass
            };
            newAbstractClass.ObjectsIndexes.Add(indexOfReductDataObject);
            return newAbstractClass;
        }

        private void CalculateApproximation()
        {
            var lowerApproximation = 0;
            var upperApproximation = 0;

            foreach (var abstractClass in AbstractClasses)
            {
                upperApproximation += abstractClass.ObjectsIndexes.Count;
                if (abstractClass.IsClear)
                    lowerApproximation += abstractClass.ObjectsIndexes.Count;
            }

            Approximation = (double)lowerApproximation/upperApproximation;
        }
    }
}
