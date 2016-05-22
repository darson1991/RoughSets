using System.Collections.Generic;

namespace BusinessLogic
{
    public class RoughSetInformations
    {
        public List<string> ArgumentNames { get; set; }
        public List<string> DecisionClasses { get; set; }

        public RoughSetInformations(List<string> argumentNames, List<string> decisionClasses)
        {
            ArgumentNames = argumentNames;
            DecisionClasses = decisionClasses;
        }
    }
}
