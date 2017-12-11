using Lab3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Data
{
    public class FlightContext : DbContext { 

        public FlightContext(DbContextOptions<FlightContext> options) : base(options) {

        }

        public DbSet<Airport> Airports { get; set; }

        public DbSet<Airplane> Airplanes { get; set; }

        public DbSet<AviaCompany> AviaCompanies { get; set; }

        public DbSet<Flight> Flights { get; set; }

        public DbSet<BackUpFlight> BackUpFlights { get; set; }

    }
}
