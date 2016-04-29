using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Data;
using System.Drawing;

namespace Tsp
{
    /// This class performs the Travelling Salesman Problem algorithm.
    class Tsp
    {
        /// Delegate used to raise an event when a new best tour is found.
        public delegate void NewBestTourEventHandler(Object sender, TspEventArgs e);

        /// Event fired when a new best tour is found.
        public event NewBestTourEventHandler foundNewBestTour;

        /// Random number generator object.
        /// We allow the GUI to set the seed for the random number generator to assist in debugging.
        /// This allows errors to be easily reproduced.
        Random rand;

        /// The list of cities. This is only used to calculate the distances between the cities.
        Cities cityList;

        /// The complete list of all the tours.
        Population population;

        /// Private copy of a flag that will stop the TSP from calculating any more generations.
        private bool halt = false;
        /// The GUI sets this flag to true to stop the TSP algorithm and allow the Begin() function to return.
        public bool Halt
        {
            get
            {
                return halt;
            }
            set
            {
                halt = value;
            }
        }

        public Tsp()
        {
        }

        /// Starts the TSP algorithm.
        /// To stop before all generations are calculated, set <see cref="Halt"/> to true.
        /// <param name="populationSize">Number of random tours to create before starting the algorithm.</param>
        /// <param name="maxGenerations">Number of times to perform the crossover operation before stopping.</param>
        /// <param name="groupSize">Number of tours to examine in each generation. Top 2 are chosen as the parent tours whose children replace the worst 2 tours in the group.</param>
        /// <param name="mutation">Odds that a child tour will be mutated..</param>
        /// <param name="seed">Seed for the random number generator.</param>
        /// <param name="chanceToUseCloseCity">The odds (out of 100) that a city that is known to be close will be used in any given link.</param>
        /// <param name="cityList">List of cities in the tour.</param>
        public void Begin(int populationSize, int maxGenerations, int groupSize, int mutation, int seed, int chanceToUseCloseCity, Cities cityList)
        {
            rand = new Random(seed);

            this.cityList = cityList;

            population = new Population();
            population.CreateRandomPopulation(populationSize, cityList, rand, chanceToUseCloseCity);

            displayTour(population.BestTour, 0, false);

            bool foundNewBestTour = false;
            int generation;
            for (generation = 0; generation < maxGenerations; generation++)
            {
                if (Halt)
                {
                    break;  // GUI has requested we exit.
                }
                foundNewBestTour = makeChildren(groupSize, mutation);

                if (foundNewBestTour)
                {
                    displayTour(population.BestTour, generation, false);
                }
            }

            displayTour(population.BestTour, generation, true);
        }

        /// Randomly select a group of tours from the population. 
        /// The top 2 are chosen as the parent tours.
        /// Crossover is performed on these 2 tours.
        /// The childred tours from this process replace the worst 2 tours in the group.
        /// <param name="groupSize">Number of tours in this group.</param>
        /// <param name="mutation">Odds that a child will be mutated.</param>
        bool makeChildren(int groupSize, int mutation)
        {
            int[] tourGroup = new int[groupSize];
            int tourCount, i, topTour, childPosition, tempTour;
        	
            // pick random tours to be in the neighborhood city group
            // we allow for the same tour to be included twice
            for (tourCount = 0; tourCount < groupSize; tourCount++)
            {
                tourGroup[tourCount] = rand.Next(population.Count);
            }

            // bubble sort on the neighborhood city group
            for (tourCount = 0; tourCount < groupSize - 1; tourCount++)
            {
                topTour = tourCount;
                for (i = topTour + 1; i < groupSize; i++)
                {
                    if (population[tourGroup[i]].Fitness < population[tourGroup[topTour]].Fitness)
                    {
                        topTour = i;
                    }
                }

                if (topTour != tourCount)
                {
                    tempTour = tourGroup[tourCount];
                    tourGroup[tourCount] = tourGroup[topTour];
                    tourGroup[topTour] = tempTour;
                }
            }

            bool foundNewBestTour = false;

            // take the best 2 tours, do crossover, and replace the worst tour with it
            childPosition = tourGroup[groupSize - 1];
            population[childPosition] = Tour.Crossover(population[tourGroup[0]], population[tourGroup[1]], cityList, rand);
            if (rand.Next(100) < mutation)
            {
                population[childPosition].Mutate(rand);
            }
            population[childPosition].DetermineFitness(cityList);

            // now see if the first new tour has the best fitness
            if (population[childPosition].Fitness < population.BestTour.Fitness)
            {
                population.BestTour = population[childPosition];
                foundNewBestTour = true;
            }

            // take the best 2 tours (opposite order), do crossover, and replace the 2nd worst tour with it
            childPosition = tourGroup[groupSize - 2];
            population[childPosition] = Tour.Crossover(population[tourGroup[1]], population[tourGroup[0]], cityList, rand);
            if (rand.Next(100) < mutation)
            {
                population[childPosition].Mutate(rand);
            }
            population[childPosition].DetermineFitness(cityList);

            // now see if the second new tour has the best fitness
            if (population[childPosition].Fitness < population.BestTour.Fitness)
            {
                population.BestTour = population[childPosition];
                foundNewBestTour = true;
            }

            return foundNewBestTour;
        }

        /// Raise an event to the GUI listener to display a tour.
        /// <param name="bestTour">The best tour the algorithm has found so far.</param>
        /// <param name="generationNumber">How many generations have been performed.</param>
        /// <param name="complete">Is the TSP algorithm complete.</param>
        void displayTour(Tour bestTour, int generationNumber, bool complete)
        {
            if (foundNewBestTour != null)
            {
                this.foundNewBestTour(this, new TspEventArgs(cityList, bestTour, generationNumber, complete));
            }
        }
    }
}
