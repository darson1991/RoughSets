using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Algorithms.Common
{
    public class Population
    {
        public List<Reduct> Individuals { get; set; }

        public List<Reduct> SortedIndividuals => Individuals.OrderByDescending(i => i.Approximation).ThenBy(i => i.Subset.Count).ToList();
        public Reduct FittestReduct => SortedIndividuals.FirstOrDefault();

        public Population()
        {
            Individuals = new List<Reduct>();
        }
    }
}
