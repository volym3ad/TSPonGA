using System;
using System.Collections.Generic;
using System.Text;

namespace Tsp
{
    /// An individual link between 2 cities in a tour.
    /// This city connects to 2 other cities.
    public class Link
    {
        /// Connection to the first city.
        private int connection1;
        /// Connection to the first city.
        public int Connection1
        {
            get
            {
                return connection1;
            }
            set
            {
                connection1 = value;
            }
        }

        /// Connection to the second city.
        private int connection2;
        /// Connection to the second city.
        public int Connection2
        {
            get
            {
                return connection2;
            }
            set
            {
                connection2 = value;
            }
        }
    }
}
