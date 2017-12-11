using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Models
{
    public class BackUpFlight {
        public int Id { get; set; }

        public int AviaCompanyId { get; set; }

        public AviaCompany AviaCompany { get; set; }

        public int AirplaneId { get; set; }

        public Airplane Airplane { get; set; }

        public int HomeAirportId { get; set; }

        public Airport HomeAirport { get; set; }

        public int DestinationAirportId { get; set; }

        public Airport DestinationAirport { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }
    }
}
