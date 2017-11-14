using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class ShowFlightModel{

        public int Id { get; set; }

        public string Company { get; set; }

        public string Plane { get; set; }

        public string HomeAirport { get; set; }

        public string DestinationAirport { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }

    }
}
