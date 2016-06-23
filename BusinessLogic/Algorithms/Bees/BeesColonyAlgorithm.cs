using System.Collections.Generic;
using System.Linq;
using BusinessLogic.Algorithms.Common;
using BusinessLogic.Helpers;

namespace BusinessLogic.Algorithms.Bees
{
    public class BeesColonyAlgorithm : BaseAlgorithm
    {
        private readonly BeesColonyAlgorithmInputValues _inputValues;
        private int _iterationWithoutImprovementCount;

        public Population ActualPopulation { get; private set; }

        public BeesColonyAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects, BaseAlgorithmInputValues inputValues) 
            : base(individualLength, clusteredDataObjects)
        {
            _inputValues = (BeesColonyAlgorithmInputValues)inputValues;
        }

        public override void Calculate()
        {
            CalculateApproximationForAllAttributes();
            SetInitialPopulation();

            BestSolution = ActualPopulation.FittestReduct;

            while (++_iterationWithoutImprovementCount != _inputValues.IterationWithoutImprovement)
            {
                //TODO: add logic to algorithm

                if (!ShouldChangeBestSolution(ActualPopulation.FittestReduct))
                    continue;

                BestSolution = ActualPopulation.FittestReduct;
                _iterationWithoutImprovementCount = 0;
            }
        }

        private void SetInitialPopulation()
        {
            ActualPopulation = new Population();

            for (var i = 0; i < _inputValues.PopulationSize; i++)
            {
                var randomIndividual = BinaryStringHelper.GenerateRandomIndividual(IndividualLength);
                TryAddReductToCheckedReductsList(randomIndividual);
                ActualPopulation.Individuals.Add(CheckedReducts.FirstOrDefault(r => r.Individual == randomIndividual));
            }
        }
    }
}
