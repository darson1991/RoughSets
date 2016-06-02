using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using weka.clusterers;
using weka.core;
using Attribute = weka.core.Attribute;

namespace BusinessLogic.Clustering
{
    [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
    public class ClusteringOperations
    {
        private static List<DataObject> _dataObjects; 
        private static List<Attribute> _attributes;
        private static FastVector _fastVector;
        private static List<ArgumentClustersRanges> _argumentsClustersRangeList;

        public static List<ClusteredDataObject> Clustering(RoughSetInformations roughSetInformations, List<DataObject> dataObjects)
        {
            _dataObjects = dataObjects;
            _attributes = roughSetInformations.ArgumentNames.Select(argumentName => new Attribute(argumentName)).ToList();
            _fastVector = PrepareFastVector();

            if (_dataObjects == null)
                return null;

            _argumentsClustersRangeList = PrepareArgumentsClustersRangeList();

            SortArgumentsClustersRangeListItems();

            return PrepareClusteredDataObjects();
        }

        private static FastVector PrepareFastVector()
        {
            var fastVector = new FastVector(_attributes.Count);
            foreach (var attribute in _attributes)
            {
                fastVector.addElement(attribute);
            }
            return fastVector;
        }

        private static List<ArgumentClustersRanges> PrepareArgumentsClustersRangeList()
        {
            var numberOfClusters = (int) Math.Sqrt(_dataObjects.Count / 2);

            var argumentsClustersRangeList = new List<ArgumentClustersRanges>();

            for (var i = 0; i < _attributes.Count; i++)
            {
                var instances = PrepareClusterInstancesForArgument(i);
                var kMeans = PrepareKMeansForArgument(numberOfClusters, instances);
                var clustersRangeList = PrepareClustersRangeListForArgument(numberOfClusters);
                argumentsClustersRangeList.Add(new ArgumentClustersRanges(clustersRangeList));
                UpdateClustersRangeListForArgument(instances, kMeans, i, argumentsClustersRangeList);
            }

            return argumentsClustersRangeList;
        }

        private static void SortArgumentsClustersRangeListItems()
        {
            foreach (var argumentClusterRanges in _argumentsClustersRangeList)
            {
                argumentClusterRanges.ClusterRanges.Sort((x, y) => x.From.CompareTo(y.From));
            }
        }

        private static List<ClusteredDataObject> PrepareClusteredDataObjects()
        {
            var clusteredDataObjects = new List<ClusteredDataObject>();

            foreach (var dataObject in _dataObjects)
            {
                var clusteredDataObject = new ClusteredDataObject
                {
                    Decision = dataObject.Decision
                };
                for (var i = 0; i < dataObject.Arguments.Count; i++)
                {
                    var argumentValue = dataObject.Arguments[i];
                    foreach (var clusterRange in _argumentsClustersRangeList[i].ClusterRanges)
                    {
                        if (argumentValue < clusterRange.From || argumentValue > clusterRange.To)
                            continue;

                        var indexOfClusterRange = _argumentsClustersRangeList[i].ClusterRanges.IndexOf(clusterRange);
                        clusteredDataObject.Arguments.Add(indexOfClusterRange);
                        break;
                    }
                }
                clusteredDataObjects.Add(clusteredDataObject);
            }

            return clusteredDataObjects;
        }

        private static Instances PrepareClusterInstancesForArgument(int attributeIndex)
        {
            var objectsCount = _dataObjects.Count;
            var instances = new Instances("data", _fastVector, objectsCount);
            for (var i = 0; i < objectsCount; i++)
            {
                var instance = new Instance(_fastVector.size());
                instance.setValue(_attributes[attributeIndex], _dataObjects[i].Arguments[attributeIndex]);
                instances.add(instance);
            }
            return instances;
        }

        private static SimpleKMeans PrepareKMeansForArgument(int numberOfClusters, Instances instances)
        {
            var kMeans = new SimpleKMeans();
            kMeans.setNumClusters(numberOfClusters);
            kMeans.buildClusterer(instances);
            return kMeans;
        }

        private static List<ClusterRange> PrepareClustersRangeListForArgument(int numberOfClusters)
        {
            var clustersRangeList = new List<ClusterRange>();
            for (var i = 0; i < numberOfClusters; i++)
            {
                var clusterRange = new ClusterRange();
                clustersRangeList.Add(clusterRange);
            }
            return clustersRangeList;
        }

        private static void UpdateClustersRangeListForArgument(Instances instances, SimpleKMeans kMeans, int i, IReadOnlyList<ArgumentClustersRanges> argumentsClustersRangeList)
        {
            for (var j = 0; j < instances.numInstances(); j++)
            {
                var n = kMeans.clusterInstance(instances.instance(j));
                var value = instances.instance(j).value(_attributes[i]);

                if (value < argumentsClustersRangeList[i].ClusterRanges[n].From)
                    argumentsClustersRangeList[i].ClusterRanges[n].From = value;

                if (value > argumentsClustersRangeList[i].ClusterRanges[n].To)
                    argumentsClustersRangeList[i].ClusterRanges[n].To = value;
            }
        }
    }
}
