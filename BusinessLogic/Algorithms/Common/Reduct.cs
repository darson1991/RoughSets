using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Algorithms.Common
{
    public class Reduct
    {
        public static double Gamma { get; set; }
        public string Individual { get; }
        public List<int> Subset { get; private set; } 
        public List<AbstractClass> AbstractClasses { get; private set; }
        public List<ClusteredDataObject> ReductDataObjects { get; private set; }
        public double Approximation { get; private set; }

        public double FitnessFunction => 1 - Approximation + Gamma * Subset.Count;

        public Reduct(string individual, IEnumerable<ClusteredDataObject> clusteredDataObjects)
        {
            Individual = individual;
            GenerateSubset();
            GenerateDataObjectsForReduct(clusteredDataObjects);
            GenerateAbstractClasses();
            CalculateApproximation();
        }

        private void GenerateSubset()
        {
            Subset = new List<int>();

            for (var i = 0; i < Individual.Length; i++)
            {
                if (Individual[i] == '1')
                    Subset.Add(i);
            }
        }

        private void GenerateDataObjectsForReduct(IEnumerable<ClusteredDataObject> clusteredDataObjects)
        {
            ReductDataObjects = new List<ClusteredDataObject>();
            if (Subset.Count == 0)
                return;

            foreach (var reductDataObject in clusteredDataObjects.Select(CreateReductDataObject))
            {
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
                var arguments = reductDataObject.Arguments;

                bool isNewAbstractClass;
                UpdateExistingAbstractClassesWithTheSameArguments(arguments, reductDataObject, i, out isNewAbstractClass);

                if (!isNewAbstractClass)
                    continue;
                
                var newAbstractClass = CreateNewAbstractClass(arguments, reductDataObject, i);

                AbstractClasses.Add(newAbstractClass);
            }
        }

        private void UpdateExistingAbstractClassesWithTheSameArguments(List<int> arguments, ClusteredDataObject reductDataObject,
            int indexOfReductDataObject, out bool isNewAbstractClass)
        {
            isNewAbstractClass = true;
            foreach (var abstractClass in AbstractClasses.Where(abstractClass => abstractClass.ArgumentsValues.SequenceEqual(arguments)))
            {
                isNewAbstractClass = false;
                abstractClass.ObjectsIndexes.Add(indexOfReductDataObject);
                if (abstractClass.Decision == reductDataObject.Decision)
                    continue;

                abstractClass.IsClear = false;
                abstractClass.Decision = null;
            }
        }

        private static AbstractClass CreateNewAbstractClass(List<int> arguments, ClusteredDataObject reductDataObject, 
            int indexOfReductDataObject)
        {
            var newAbstractClass = new AbstractClass
            {
                ArgumentsValues = arguments,
                Decision = reductDataObject.Decision,
                IsClear = true
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
