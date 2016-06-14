using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using BusinessLogic.Algorithms.Common;
using BusinessLogic.Helpers;

namespace BusinessLogic.Algorithms.Genetic
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class GeneticAlgorithm: BaseAlgorithm
    {
        private readonly GeneticAlgorithmInputValues _inputValues;
        private int _iterationWithoutImprovementCount = 1;

        public Population ActualPopulation { get; private set; }
        
        public GeneticAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects, GeneticAlgorithmInputValues inputValues) 
            :base(individualLength, clusteredDataObjects)
        {
            _inputValues = inputValues;
        }

        public override void Calculate()
        {
            ActualPopulation = new Population();
            SetInitialPopulation();
            while (_iterationWithoutImprovementCount != _inputValues.IterationWithoutImprovement)
            {
                var newPopulation = new Population();
                newPopulation.Individuals.Add(ActualPopulation.FittestReduct);

                if (ShouldChangeBestSolution())
                    BestSolution = ActualPopulation.FittestReduct;


            }
        }

        private bool ShouldChangeBestSolution()
        {
            return BestSolution == null || ActualPopulation.FittestReduct.Approximation > BestSolution.Approximation
                   ||
                   (ActualPopulation.FittestReduct.Approximation == BestSolution.Approximation &&
                    ActualPopulation.FittestReduct.Subset.Count < BestSolution.Subset.Count);
        }

        private void SetInitialPopulation()
        {
            for (var i = 0; i < _inputValues.PopulationSize; i++)
            {
                var randomIndividual = BinaryStringHelper.GenerateRandomIndividual(_individualLength);
                ActualPopulation.Individuals.Add(new Reduct(randomIndividual, _clusteredDataObjects));
            }
        }
    }
}
