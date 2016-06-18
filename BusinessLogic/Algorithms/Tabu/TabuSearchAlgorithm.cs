using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using BusinessLogic.Algorithms.Common;
using BusinessLogic.Helpers;

namespace BusinessLogic.Algorithms.Tabu
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    [SuppressMessage("ReSharper", "LoopCanBeConvertedToQuery")]
    public class TabuSearchAlgorithm: BaseAlgorithm
    {
        private readonly TabuSearchAlgorithmInputValues _inputValues;
        private int _iterationWithoutImprovementCount;
        private int[] _tabuList; 

        public Reduct ActualSolution { get; set; }

        public TabuSearchAlgorithm(int individualLength, List<ClusteredDataObject> clusteredDataObjects, BaseAlgorithmInputValues inputValues)
            : base(individualLength, clusteredDataObjects)
        {
            _inputValues = (TabuSearchAlgorithmInputValues)inputValues;
            _tabuList = new int[individualLength];
        }

        public override void Calculate()
        {
            CalculateApproximationForAllAttributes();
            SetInitialSolution();

            BestSolution = ActualSolution;

            while (++_iterationWithoutImprovementCount != _inputValues.IterationWithoutImprovement)
            {
                var neighborsList = GenerateSortedNeighborhoodForActualSolution();

                var bestNeighbor = ChooseNextNeighbor(neighborsList); 

                var indexOfIndividualChange = GetIndexOfIndividualStringChange(bestNeighbor);
                if (indexOfIndividualChange == null)
                    continue;

                ActualSolution = bestNeighbor;

                TabuListActualization((int)indexOfIndividualChange);

                if (!ShouldChangeBestSolution())
                    continue;

                BestSolution = ActualSolution;
                _iterationWithoutImprovementCount = 0;
            }
        }

        private Reduct ChooseNextNeighbor(IReadOnlyList<Reduct> neighborsList)
        {
            //TODO: add checking situation where  circular changes of best neighbor is (maybe frequence will be nice to add)
            Reduct bestNeighbor = null;
            for (var i = 0; i < neighborsList.Count; i++)
            {
                // ReSharper disable once InvertIf
                if (neighborsList[i].Approximation > BestSolution.Approximation || _tabuList[i] == 0)
                {
                    bestNeighbor = neighborsList[i];
                    break;
                }
            }

            return bestNeighbor;
        }

        private void TabuListActualization(int indexOfIndividualChange)
        {
            for (var i = 0; i < _tabuList.Length; i++)
            {
                if (_tabuList[i] > 0)
                    _tabuList[i]--;
            }
            _tabuList[indexOfIndividualChange] = _inputValues.TabuListLength;
        }

        private int? GetIndexOfIndividualStringChange(Reduct bestNeighbor)
        {
            for (var i = 0; i < IndividualLength; i++)
            {
                if (ActualSolution == null || bestNeighbor == null || ActualSolution.Individual[i] == bestNeighbor.Individual[i])
                    continue;
                return i;
            }
            return null;
        }

        private List<Reduct> GenerateSortedNeighborhoodForActualSolution()
        {
            var neighborsList = new List<Reduct>();

            for (var i = 0; i < IndividualLength; i++)
            {
                var neighborIndividual = GenerateNeighborIndividualString(i);
                var neighbor = new Reduct(neighborIndividual, ClusteredDataObjects);
                neighborsList.Add(neighbor);
            }

            return neighborsList.OrderBy(n => n.Approximation).Reverse().ToList();
        }

        private string GenerateNeighborIndividualString(int index)
        {
            var neighborIndividualStringBuilder = new StringBuilder(ActualSolution.Individual);
            neighborIndividualStringBuilder[index] = neighborIndividualStringBuilder[index] == '0' ? '1' : '0';
            return neighborIndividualStringBuilder.ToString();
        }

        private void SetInitialSolution()
        {
            var individual = BinaryStringHelper.GenerateRandomIndividual(IndividualLength);
            ActualSolution = new Reduct(individual, ClusteredDataObjects);
        }

        private bool ShouldChangeBestSolution()
        {
            return BestSolution == null || ActualSolution.Approximation > BestSolution.Approximation
                   ||
                   (ActualSolution.Approximation == BestSolution.Approximation &&
                    ActualSolution.Subset.Count < BestSolution.Subset.Count);
        }
    }
}
