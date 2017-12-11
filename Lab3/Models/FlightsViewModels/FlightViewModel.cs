using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Models.FlightsViewModels
{
    public class FlightViewModel {

        public int Id { get; set; }

        [Display(Name = "Company")]
        public int AviaCompanyId { get; set; }

        [Display(Name="Airplane")]
        public int AirplaneId { get; set; }

        [Display(Name = "From")]
        public int HomeAirportId { get; set; }

        [Display(Name = "To")]
        public int DestinationAirportId { get; set; }

        public DateTime Departure { get; set; }

        public DateTime Arrival { get; set; }
    }
}
