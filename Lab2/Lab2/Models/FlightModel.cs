using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class FlightModel {

        public int Id { get; set; }

        public int AviaCompanyId { get; set; }

        public int AirplaneId { get; set; }

        public int HomeAirportId { get; set; }

        public int DestinationAirportId { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }
    }
}
