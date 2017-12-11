using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Models.FlightsViewModels
{
    public class LookAttrViewModel {

        [Display(Name = "AviaCompanies' countries")]
        public string CompanyCountry { get; set; }

        [Display(Name = "AviaCompanies' names")]
        public string CompanyName { get; set; }

        [Display(Name = "Planes' number of seats From")]
        public int PlaneSeatsFrom { get; set; }

        [Display(Name = "Planes' number of seats To")]
        public int PlaneSeatsTo { get; set; }

        [Display(Name = "Planes' models ")]
        public string PlaneModel { get; set; }

        [Display(Name = "Airports' From Countries")]
        public string AirportCountryFrom { get; set; }

        [Display(Name = "Airports' To Countries")]
        public string AirportCountryTo { get; set; }

        [Display(Name = "Departure Date")]
        public DateTime FlightDate { get; set; }

    }
}
