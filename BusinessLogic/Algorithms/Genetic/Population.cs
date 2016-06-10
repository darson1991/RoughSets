using System.Collections.Generic;
using BusinessLogic.Algorithms.Common;

namespace BusinessLogic.Algorithms.Genetic
{
    public class Population
    {
        public List<Reduct> Individuals { get; set; } 

        public Population()
        {
            Individuals = new List<Reduct>();
        }

    }
}
