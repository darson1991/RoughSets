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
            var attributes = roughSetInformations.ArgumentNames.Select(argumentName => new Attribute(argumentName)).ToList();

            var fastVector = PrepareFastVector(attributes);

            if (dataObjects == null)
                return;

            var objectsCount = dataObjects.Count;
            var numberOfClusters = (int)Math.Sqrt(objectsCount / 2);

            var argumentsClustersRangeList = new List<ArgumentClustersRanges>();

            for (var i = 0; i < attributes.Count; i++)
            {
                var instances = PrepareClusterInstancesForArgument(dataObjects, fastVector, objectsCount, attributes, i);
                var kMeans = ForArgument(numberOfClusters, instances);
                var clustersRangeList = PrepareClustersRangeListForArgument(numberOfClusters);
                argumentsClustersRangeList.Add(new ArgumentClustersRanges(clustersRangeList));
                UpdateClustersRangeListForArgument(instances, kMeans, attributes, i, argumentsClustersRangeList);
            }
        }

        private static void UpdateClustersRangeListForArgument(Instances instances, SimpleKMeans kMeans, List<Attribute> attributes, int i,
            List<ArgumentClustersRanges> argumentsClustersRangeList)
        {
            for (var j = 0; j < instances.numInstances(); j++)
            {
                var n = kMeans.clusterInstance(instances.instance(j));
                var value = instances.instance(j).value(attributes[i]);

                if (value < argumentsClustersRangeList[i].ClusterRanges[n].From)
                    argumentsClustersRangeList[i].ClusterRanges[n].From = value;

                if (value > argumentsClustersRangeList[i].ClusterRanges[n].To)
                    argumentsClustersRangeList[i].ClusterRanges[n].To = value;
            }
        }

        private static List<ClusterRange> PrepareClustersRangeListForArgument(int numberOfClusters)
        {
            var clustersRangeList = new List<ClusterRange>();
            for (var j = 0; j < numberOfClusters; j++)
            {
                var minMaxValue = new ClusterRange(double.MaxValue, double.MinValue);
                clustersRangeList.Add(minMaxValue);
            }
            return clustersRangeList;
        }

        private static SimpleKMeans ForArgument(int numberOfClusters, Instances instances)
        {
            var kMeans = new SimpleKMeans();
            kMeans.setNumClusters(numberOfClusters);
            kMeans.buildClusterer(instances);
            return kMeans;
        }

        private static Instances PrepareClusterInstancesForArgument(List<DataObject> dataObjects, FastVector fastVector, int objectsCount,
            List<Attribute> attributes, int i)
        {
            var instances = new Instances("data", fastVector, objectsCount);
            for (var j = 0; j < objectsCount; j++)
            {
                var instance = new Instance(fastVector.size());
                instance.setValue(attributes[i], dataObjects[j].Arguments[i]);
                instances.add(instance);
            }
            return instances;
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
