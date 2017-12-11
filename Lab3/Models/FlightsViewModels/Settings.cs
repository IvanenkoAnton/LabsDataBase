using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Models.FlightsViewModels
{
    public class Settings {

        [Display(Name = "Using Back up")]
        public bool UseTrigger { get; set; }

        [Display(Name = "To delete flights from this time")]
        public TimeSpan EventTime { get; set; }
    }
}
