using Lab3.Models;
using Lab3.Models.FlightsViewModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Data
{
    public class DbInitializer {

        string _dataPath;
        FlightContext _dbContext;

        public DbInitializer(FlightContext dbContext, string dataPath) {
            _dataPath = dataPath;
            _dbContext = dbContext;
        }

        public void CheckDb() {
            if (!CheckDataExistance()) {
                RecreateDb();
                var sql = @"SET @@global.event_scheduler = 1; DROP EVENT IF EXISTS delete_every_day; CREATE EVENT delete_every_day
    ON SCHEDULE EVERY 1 DAY STARTS '" + DateTime.Now.ToString(@"yyyy-MM-dd") + @" 00:00:00' - INTERVAL 1 DAY   
    COMMENT 'Clears out flights every day.'
    DO
      DELETE FROM flights WHERE departure < (SELECT CURDATE()) ;";
                var connection = _dbContext.Database.GetDbConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
                var settings = new Settings {
                    UseTrigger = false,
                    EventTime = TimeSpan.FromMilliseconds(0)
                };
                var settingsJson = JsonConvert.SerializeObject(settings);
                File.WriteAllText(Path.GetFullPath(Path.Combine("App_data", "settings.json")), settingsJson);
                connection.Close();
            }
        }

        private bool CheckDataExistance() {
            try {
                var testRecord = _dbContext.Flights.FirstOrDefault();

                return testRecord != null;
            }
            catch {
                return false;
            }

        }

        private void RecreateDb() {
            try {
                _dbContext.Database.EnsureDeleted();
            }
            catch {

            }

            var created = _dbContext.Database.EnsureCreated();
            if (created) {
                Seed();
            }
        }

        public void Seed() {
            LoadAviaCompanies();
            LoadAirplanes();
            LoadAirports();
            LoadFlights();
        }


        private void LoadAviaCompanies() {
            var Lines = File.ReadAllLines(Path.Combine(_dataPath, "companies.csv"));

            foreach (var line in Lines) {
                _dbContext.AviaCompanies.Add(GetCompanyFromString(Parse(line)));
            }

            _dbContext.SaveChanges();
        }

        private void LoadAirplanes() {
            var Lines = File.ReadAllLines(Path.Combine(_dataPath, "airplanes.csv"));

            foreach (var line in Lines) {
                _dbContext.Airplanes.Add(GetAirplaneFromString(Parse(line)));
            }

            _dbContext.SaveChanges();
        }

        private void LoadAirports() {
            var Lines = File.ReadAllLines(Path.Combine(_dataPath, "airports.csv"));

            foreach (var line in Lines) {
                _dbContext.Airports.Add(GetAirportFromString(Parse(line)));
            }

            _dbContext.SaveChanges();
        }

        private void LoadFlights() {
            var Lines = File.ReadAllLines(Path.Combine(_dataPath, "flights_info.csv"));

            foreach (var line in Lines) {
                _dbContext.Flights.Add(GetFlightFromString(Parse(line)));
            }

            _dbContext.SaveChanges();

        }

        private AviaCompany GetCompanyFromString(string[] param) {
            return new AviaCompany {
                Id = Int32.Parse(param[0]),
                CompanyName = param[1],
                Country = param[2]
            };
        }
        private Airport GetAirportFromString(string[] param) {

            return new Airport {
                Id = Int32.Parse(param[0]),
                Name = param[1],
                City = param[2],
                Country = param[3]
            };
        }
        private Airplane GetAirplaneFromString(string[] param) {

            return new Airplane {
                Id = Int32.Parse(param[0]),
                Model = param[1],
                Seats = Int32.Parse(param[2]),
                LastCheckUp = DateTime.Parse(param[3])
            };
        }
        private Flight GetFlightFromString(string[] param) {
            return new Flight {
                Id = Int32.Parse(param[0]),
                Airplane = _dbContext.Airplanes.Find(Int32.Parse(param[1])),
                AviaCompany = _dbContext.AviaCompanies.Find(Int32.Parse(param[2])),
                HomeAirport = _dbContext.Airports.Find(Int32.Parse(param[3])),
                DestinationAirport = _dbContext.Airports.Find(Int32.Parse(param[4])),
                Departure = DateTime.Parse(param[5]),
                Arrival = DateTime.Parse(param[6])
            };
        }

        private string[] Parse(string value) {
            return value.Split(",");
        }

    }
}
