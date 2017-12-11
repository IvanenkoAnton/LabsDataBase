using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Models
{
    public class Airplane {

        [Key]
        public int Id { get; set; }

        public string Model { get; set; }

        public int Seats { get; set; }

        public DateTime LastCheckUp { get; set; }

    }
}
