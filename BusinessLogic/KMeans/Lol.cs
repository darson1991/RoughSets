using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using weka.clusterers;
using weka.core;
using Attribute = weka.core.Attribute;

namespace BusinessLogic.KMeans
{
    [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
    public class Lol
    {
        public static void Clustering(RoughSetInformations roughSetInformations, List<DataObject> dataObjects)
        {
            List<Attribute> a = new List<Attribute>();
            foreach (var argumentName in roughSetInformations.ArgumentNames)
            {
                a.Add(new Attribute(argumentName));
            }
            var attributes = roughSetInformations.ArgumentNames.Select(argumentName => new Attribute(argumentName)).ToList();

            var fastVector = PrepareFastVector(attributes);

            if (dataObjects == null)
                return;

            var objectsCount = dataObjects.Count;
            var numberOfClusters = (int)Math.Sqrt(objectsCount / 2);

            var listOfMinMaxLists = new List<List<Pair<double, double>>>();

            for (var i = 0; i < attributes.Count; i++)
            {
                var instances = new Instances("data", fastVector, objectsCount);
                for (var j = 0; j < objectsCount; j++)
                {
                    var instance = new Instance(fastVector.size());
                    instance.setValue(attributes[i], dataObjects[j].Arguments[i]);
                    instances.add(instance);
                }

                var kMeans = new SimpleKMeans();
                kMeans.setNumClusters(numberOfClusters);
                kMeans.buildClusterer(instances);

                var minMaxList = new List<Pair<double, double>>();
                for (var j = 0; j < numberOfClusters; j++)
                {
                    var minMaxValue = new Pair<double, double>(double.MaxValue, double.MinValue);
                    minMaxList.Add(minMaxValue);
                }

                listOfMinMaxLists.Add(minMaxList);

                var centroids = kMeans.getClusterCentroids();

                for (var j = 0; j < instances.numInstances(); j++)
                {
                    var n = kMeans.clusterInstance(instances.instance(j));
                    var val = instances.instance(j).value(attributes[i]);

                    if (val < listOfMinMaxLists[i][n].First)
                        listOfMinMaxLists[i][n].First = val;

                    if (val > listOfMinMaxLists[i][n].Second)
                        listOfMinMaxLists[i][n].Second = val;
                }

                var clusterSizes = kMeans.getClusterSizes();
            }
        }

        private static FastVector PrepareFastVector(List<Attribute> attributes)
        {
            var fastVector = new FastVector(attributes.Count);
            foreach (var attribute in attributes)
            {
                fastVector.addElement(attribute);
            }
            return fastVector;
        }
    }
}
