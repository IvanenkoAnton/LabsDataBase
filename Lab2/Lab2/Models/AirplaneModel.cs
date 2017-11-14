using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class AirplaneModel {

        public int Id { get; set; }

        public string Model { get; set; }

        public int Seats { get; set; }

        public DateTime LastCheckUp { get; set; }

    }
}
