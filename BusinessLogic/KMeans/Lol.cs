using System.Collections.Generic;
using weka.core;

namespace BusinessLogic.KMeans
{
    public class Lol
    {
        private static void SecondClasify()
        {
            List<List<double>> objects = new List<List<double>>();
            List<List<double[]>> minMaxList = new List<List<double[]>>();
            List<string> attributes = new List<string> {"1", "2", "3", "4"};
            List<weka.core.Attribute> attributesList = new List<weka.core.Attribute>();
            foreach (var attribute in attributes)
            {
                attributesList.Add(new weka.core.Attribute(attribute));
            }
            //do weki potrzebne
            FastVector fastVector = new FastVector(attributesList.Count);

            foreach (var attribute in attributesList)
            {
                fastVector.addElement(attribute);
            }
        }
    }
}
