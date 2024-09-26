using System;
using System.Collections.Generic;
using System.Linq;

namespace NewBlazorApp.Algorithms
{
    public class KMeans
    {
        public List<int> Cluster(List<double[]> data, int k, int maxIterations = 100)
        {
            int n = data.Count;
            int dimensions = data[0].Length;

            // Step 1: Randomly initialize centroids
            var centroids = InitializeCentroids(data, k, dimensions);
            var assignments = new List<int>(new int[n]);  // Track which cluster each point belongs to

            bool changed = true;
            int iterations = 0;

            // Step 2: Iterate until convergence or maximum iterations
            while (changed && iterations < maxIterations)
            {
                changed = false;
                iterations++;

                // Step 3: Assign each data point to the closest centroid
                for (int i = 0; i < n; i++)
                {
                    int closest = GetClosestCentroid(data[i], centroids);
                    if (assignments[i] != closest)
                    {
                        assignments[i] = closest;
                        changed = true;  // Points changed their cluster
                    }
                }

                // Step 4: Recalculate centroids
                centroids = UpdateCentroids(data, assignments, k, dimensions);
            }

            return assignments;  // Return the cluster assignments
        }

        // Initialize centroids by randomly selecting k points from the dataset
        private List<double[]> InitializeCentroids(List<double[]> data, int k, int dimensions)
        {
            var rand = new Random();
            var centroids = new List<double[]>();

            // Pick k random points as the initial centroids
            var chosenIndices = new HashSet<int>();
            while (centroids.Count < k)
            {
                int index = rand.Next(data.Count);
                if (chosenIndices.Add(index))  // Make sure not to pick the same point twice
                {
                    centroids.Add(data[index].ToArray());  // Copy the array to avoid reference issues
                }
            }

            return centroids;
        }

        // Find the closest centroid for a given data point
        private int GetClosestCentroid(double[] point, List<double[]> centroids)
        {
            double minDistance = double.MaxValue;
            int closest = -1;

            for (int i = 0; i < centroids.Count; i++)
            {
                double distance = EuclideanDistance(point, centroids[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = i;
                }
            }

            return closest;
        }

        // Update centroids based on current assignments
        private List<double[]> UpdateCentroids(List<double[]> data, List<int> assignments, int k, int dimensions)
        {
            var newCentroids = new List<double[]>();
            var clusterSizes = new int[k];
            var clusterSums = new List<double[]>(k);

            // Initialize cluster sums to 0
            for (int i = 0; i < k; i++)
            {
                clusterSums.Add(new double[dimensions]);
            }

            // Sum all points in each cluster
            for (int i = 0; i < data.Count; i++)
            {
                int cluster = assignments[i];
                clusterSizes[cluster]++;
                for (int d = 0; d < dimensions; d++)
                {
                    clusterSums[cluster][d] += data[i][d];
                }
            }

            // Calculate the new centroids by averaging the sums
            for (int i = 0; i < k; i++)
            {
                var centroid = new double[dimensions];
                if (clusterSizes[i] > 0)  // Avoid division by zero
                {
                    for (int d = 0; d < dimensions; d++)
                    {
                        centroid[d] = clusterSums[i][d] / clusterSizes[i];
                    }
                }
                newCentroids.Add(centroid);
            }

            return newCentroids;
        }

        // Euclidean distance between two points
        private double EuclideanDistance(double[] point1, double[] point2)
        {
            double sum = 0;
            for (int i = 0; i < point1.Length; i++)
            {
                sum += Math.Pow(point1[i] - point2[i], 2);
            }
            return Math.Sqrt(sum);
        }
    }
}
