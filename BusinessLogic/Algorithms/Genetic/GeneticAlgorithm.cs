using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
        
        public GeneticAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects, BaseAlgorithmInputValues inputValues) 
            : base(individualLength, clusteredDataObjects)
        {
            _inputValues = (GeneticAlgorithmInputValues)inputValues;
        }

        public override void Calculate()
        {
            CalculateApproximationForAllAttributes();
            SetInitialPopulation();

            BestSolution = ActualPopulation.FittestReduct;

            while (++_iterationWithoutImprovementCount != _inputValues.IterationWithoutImprovement)
            {
                var newPopulation = CreateNewPopulation();

                ActualPopulation = newPopulation;

                if (!ShouldChangeBestSolution(ActualPopulation.FittestReduct))
                    continue;

                BestSolution = ActualPopulation.FittestReduct;
                _iterationWithoutImprovementCount = 0;
            }
        }

        private Population CreateNewPopulation()
        {
            var newPopulation = InitializeNewPopulation();

            CrossoverPopulation(newPopulation);
            MutatePopulation(newPopulation);

            return newPopulation;
        }

        private Population InitializeNewPopulation()
        {
            var newPopulation = new Population();
            newPopulation.Individuals.Add(ActualPopulation.FittestReduct);

            for (var i = 1; i < _inputValues.PopulationSize; i++)
                newPopulation.Individuals.Add(TournamentSelection());
            return newPopulation;
        }

        private void CrossoverPopulation(Population newPopulation)
        {
            for (var i = 0; i < _inputValues.PopulationSize - 1; i += 2)
            {
                if (!ShouldCrossover())
                    continue;

                var newIndividuals = CrossoverIndividuals(new Tuple<string, string>(newPopulation.Individuals[i].Individual,
                    newPopulation.Individuals[i + 1].Individual));
                newPopulation.Individuals[i] = newIndividuals.Item1;
                newPopulation.Individuals[i + 1] = newIndividuals.Item2;
            }
        }

        private void MutatePopulation(Population newPopulation)
        {
            for (var i = 0; i < _inputValues.PopulationSize; i++)
            {
                if (!ShouldMutate())
                    continue;

                var newIndividual = MutateIndividual(newPopulation.Individuals[i]);
                newPopulation.Individuals[i] = newIndividual;
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

        private bool ShouldMutate()
        {
            var random = new Random();
            return random.NextDouble() < _inputValues.MutationPossibility;
        }

        private Reduct MutateIndividual(Reduct reduct)
        {
            var individual = reduct.Individual;
            var random = new Random();
            var mutationIndex = random.Next(IndividualLength);
            var newGeneValue = individual[mutationIndex] == '1' ? '0' : '1';
            var newIndividual = individual.Substring(0, mutationIndex) + newGeneValue + individual.Substring(mutationIndex + 1);
            return new Reduct(newIndividual, ClusteredDataObjects);
        }

        private bool ShouldCrossover()
        {
            var random = new Random();
            return random.NextDouble() < _inputValues.CrossingOverPossibility;
        }

        private Tuple<Reduct, Reduct> CrossoverIndividuals(Tuple<string, string> individualsTuple)
        {
            var random = new Random();
            var placeOfCross = random.Next(IndividualLength);

            var newIndividual1 = individualsTuple.Item1.Substring(0, placeOfCross) + individualsTuple.Item2.Substring(placeOfCross);
            var newIndividual2 = individualsTuple.Item2.Substring(0, placeOfCross) + individualsTuple.Item1.Substring(placeOfCross);

            TryAddReductToCheckedReductsList(newIndividual1);
            TryAddReductToCheckedReductsList(newIndividual2);
            
            var newReduct1 = CheckedReducts.FirstOrDefault(r => r.Individual == newIndividual1);
            var newReduct2 = CheckedReducts.FirstOrDefault(r => r.Individual == newIndividual2);

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
