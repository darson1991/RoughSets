using System;
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
        private int _iterationWithoutImprovementCount;

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

            if (ShouldChangeBestSolution())
                BestSolution = ActualPopulation.FittestReduct;

            while (++_iterationWithoutImprovementCount != _inputValues.IterationWithoutImprovement)
            {
                var newPopulation = new Population();
                newPopulation.Individuals.Add(ActualPopulation.FittestReduct);

                for (var i = 1; i < _inputValues.PopulationSize; i++)
                    newPopulation.Individuals.Add(TournamentSelection());

                for (var i = 0; i < _inputValues.PopulationSize - 1; i += 2)
                {
                    if (!ShouldCrossingOver())
                        continue;

                    var newIndividuals = Crossover(new Tuple<string, string>(newPopulation.Individuals[i].Individual,
                        newPopulation.Individuals[i + 1].Individual));
                    newPopulation.Individuals[i] = newIndividuals.Item1;
                    newPopulation.Individuals[i + 1] = newIndividuals.Item2;
                }

                for (var i = 0; i < _inputValues.PopulationSize; i++)
                {
                    if (!ShouldMutate())
                        continue;

                    var newIndividual = Mutate(newPopulation.Individuals[i]);
                    newPopulation.Individuals[i] = newIndividual;
                }

                ActualPopulation = newPopulation;

                if (!ShouldChangeBestSolution())
                    continue;

                BestSolution = ActualPopulation.FittestReduct;
                _iterationWithoutImprovementCount = 0;
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

        private bool ShouldMutate()
        {
            var random = new Random();
            return random.NextDouble() < _inputValues.MutationPossibility;
        }

        private Reduct Mutate(Reduct reduct)
        {
            var individual = reduct.Individual;
            var random = new Random();
            var mutationIndex = random.Next(_individualLength);
            var newGeneValue = individual[mutationIndex] == '1' ? '0' : '1';
            var newIndividual = individual.Substring(0, mutationIndex) + newGeneValue + individual.Substring(mutationIndex + 1);
            return new Reduct(newIndividual, _clusteredDataObjects);
        }

        private bool ShouldCrossingOver()
        {
            var random = new Random();
            return random.NextDouble() < _inputValues.CrossingOverPossibility;
        }

        private Tuple<Reduct, Reduct> Crossover(Tuple<string, string> individualsTuple)
        {
            var random = new Random();
            var placeOfCross = random.Next(_individualLength);

            var newIndividual1 = individualsTuple.Item1.Substring(0, placeOfCross) + individualsTuple.Item2.Substring(placeOfCross);
            var newIndividual2 = individualsTuple.Item2.Substring(0, placeOfCross) + individualsTuple.Item1.Substring(placeOfCross);

            var newReduct1 = new Reduct(newIndividual1, _clusteredDataObjects);
            var newReduct2 = new Reduct(newIndividual2, _clusteredDataObjects);

            return new Tuple<Reduct, Reduct>(newReduct1, newReduct2);
        }

        private Reduct TournamentSelection()
        {
            var tournament = GenerateTournament();
            return tournament.FittestReduct;
        }

        private Population GenerateTournament()
        {
            var tournament = new Population();
            for (var i = 0; i < _inputValues.TournamentSize; i++)
            {
                var random = new Random();
                var indexOfIndividual = random.Next(_inputValues.PopulationSize);
                tournament.Individuals.Add(ActualPopulation.Individuals[indexOfIndividual]);
            }
            return tournament;
        }
    }
}
