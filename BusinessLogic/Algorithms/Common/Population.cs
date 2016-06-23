using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BusinessLogic.Algorithms.Common
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class Population
    {
        public List<Reduct> Individuals { get; set; }

        public Reduct FittestReduct => GetFittest();

        public Population()
        {
            Individuals = new List<Reduct>();
        }

        private Reduct GetFittest()
        {
            Reduct fittestIndividual = null;
            foreach (var reduct in Individuals)
            {
                if (ShouldChangeFittest(fittestIndividual, reduct))
                    fittestIndividual = reduct;
            }
            return fittestIndividual;
        }

        private static bool ShouldChangeFittest(Reduct fittestIndividual, Reduct reduct)
        {
            return fittestIndividual == null || reduct.Approximation > fittestIndividual.Approximation
                   ||
                   (reduct.Approximation == fittestIndividual.Approximation &&
                    reduct.Subset.Count > fittestIndividual.Subset.Count);
        }
    }
}
