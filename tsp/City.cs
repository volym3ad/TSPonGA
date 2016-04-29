using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Tsp
{
    /// An individual City in our tour.
    public class City
    {
        /// Constructor that provides the city location.
        /// "x" - X position of the city.
        /// "y" - Y position of the city.
        public City(int x, int y)
        {
            Location = new Point(x, y);
        }

        /// Private copy of the location of this city.
        private Point location;
        /// The location of this city.
        public Point Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        /// Private copy of the distance from this city to every other city.
        /// The index in this array is the number of the city linked to.
        private List<double> distances = new List<double>();
        /// The distance from this city to every other city.
        public List<double> Distances
        {
            get
            {
                return distances;
            }
            set
            {
                distances = value;
            }
        }

        /// Private copy of the list of the cities that are closest to this one.
        private List<int> closeCities = new List<int>();
        /// A list of the cities that are closest to this one.
        public List<int> CloseCities
        {
            get
            {
                return closeCities;
            }
        }

        /// Find the cities that are closest to this one.
        /// "numberOfCloseCities" - When creating the initial population of tours, this is a greater chance
        /// that a nearby city will be chosen for a link. This is the number of nearby cities that will be considered close.
        public void FindClosestCities( int numberOfCloseCities )
        {
            double shortestDistance;
            int shortestCity = 0;
            double[] dist = new double[Distances.Count];
            Distances.CopyTo(dist);

            if (numberOfCloseCities > Distances.Count - 1)
            {
                numberOfCloseCities = Distances.Count - 1;
            }

            closeCities.Clear();

            for (int i = 0; i < numberOfCloseCities; i++)
            {
                shortestDistance = Double.MaxValue;
                for (int cityNum = 0; cityNum < Distances.Count; cityNum++)
                {
                    if (dist[cityNum] < shortestDistance)
                    {
                        shortestDistance = dist[cityNum];
                        shortestCity = cityNum;
                    }
                }
                closeCities.Add(shortestCity);
                dist[shortestCity] = Double.MaxValue;
            }
        }
    }
}
