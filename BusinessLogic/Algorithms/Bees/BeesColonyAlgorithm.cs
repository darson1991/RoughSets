﻿using System.Collections.Generic;
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
                var eliteReducts = ActualPopulation.SortedIndividuals.GetRange(0, _inputValues.NumberOfEliteSolutions);
                var bestReducts = ActualPopulation.SortedIndividuals.GetRange(_inputValues.NumberOfEliteSolutions, _inputValues.NumberOfBestSolutions);

                ActualPopulation.Individuals = new List<Reduct>();

                PrepareNewEliteIndividuals(eliteReducts);
                PrepareNewBestIndividuals(bestReducts);
                PrepareRestIndividuals(eliteReducts, bestReducts);

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

        private void PrepareNewBestIndividuals(IEnumerable<Reduct> bestReducts)
        {
            foreach (var bestReducr in bestReducts)
                GenerateNewSolutionFromNeighbors(bestReducr, _inputValues.BestNeighborhoodSize);
        }

        private void PrepareNewEliteIndividuals(IEnumerable<Reduct> eliteReducts)
        {
            foreach (var eliteReduct in eliteReducts)
                GenerateNewSolutionFromNeighbors(eliteReduct, _inputValues.EliteNeighborhoodSize);
        }

        private void PrepareRestIndividuals(IReadOnlyCollection<Reduct> eliteReducts, IReadOnlyCollection<Reduct> bestReducts)
        {
            var restReductsCount = ActualPopulation.Individuals.Count - eliteReducts.Count - bestReducts.Count;
            for (var i = 0; i < restReductsCount; i++)
                AddRandomIndividualToPopulation();
        }

        private void GenerateNewSolutionFromNeighbors(Reduct eliteReduct, int neighborhoodSize)
        {
            var neighborhood = new List<Reduct>();
            for (var i = 0; i < neighborhoodSize; i++)
            {
                var neighborIndividual = BinaryStringHelper.GenerateNeighborSolution(eliteReduct.Individual);
                TryAddReductToCheckedReductsList(neighborIndividual);
                neighborhood.Add(CheckedReducts.FirstOrDefault(r => r.Individual == neighborIndividual));
            }
            ActualPopulation.Individuals.Add(neighborhood.OrderByDescending(r => r.Approximation).ThenBy(i => i.Subset.Count).FirstOrDefault());
        }

        private void AddRandomIndividualToPopulation()
        {
            var randomIndividual = BinaryStringHelper.GenerateRandomIndividual(IndividualLength);
            TryAddReductToCheckedReductsList(randomIndividual);
            ActualPopulation.Individuals.Add(CheckedReducts.FirstOrDefault(r => r.Individual == randomIndividual));
        }
    }
}
