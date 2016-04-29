using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace Tsp
{
    /// Event arguments when the TSP class wants the GUI to draw a tour.
    public class TspEventArgs : EventArgs
    {
        public TspEventArgs()
        {

        }

        /// cityList - The list of cities to draw.
        /// bestTour - The tour that connects all the cities.
        /// generation - Which generation is this.
        /// complete - Is this the last update before we are done.
        public TspEventArgs(Cities cityList, Tour bestTour, int generation, bool complete)
        {
            this.cityList = cityList;
            this.bestTour = bestTour;
            this.generation = generation;
            this.complete = complete;
        }

        /// Private copy of the list of cities.
        private Cities cityList;
        /// Public property for list of cities.
        public Cities CityList
        {
            get
            {
                return cityList;
            }
        }

        /// Private copy of the tour of the cities.
        private Tour bestTour;
        /// Public property for the tour of the cities.
        public Tour BestTour
        {
            get
            {
                return bestTour;
            }
        }

        /// Private copy for which generation this tour came from.
        private int generation;
        /// Public property for which generation this tour came from.
        public int Generation
        {
            get
            {
                return generation;
            }
            set
            {
                generation = value;
            }
        }

        /// Private copy indicating if the TSP algorithm is complete.
        private bool complete = false;
        /// Public property indicating if the TSP algorithm is complete.
        public bool Complete
        {
            get
            {
                return complete;
            }
            set
            {
                complete = value;
            }
        }
    }
}