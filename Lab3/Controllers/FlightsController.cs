using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;

using Pomelo.EntityFrameworkCore.MySql;

using Lab3.Models;
using Lab3.Models.FlightsViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab3.Data;
using Lab3.Additional;
using Newtonsoft.Json;
using System.IO;

namespace Lab2.Controllers
{
    public class FlightsController : Controller {

        private FlightContext _dbContext;
        private FlightExtentions _flights;

        public FlightsController(
                FlightContext dbContext,
                FlightExtentions flights
            ) {
            _dbContext = dbContext;
            _flights = flights;
        }

        [HttpGet]
        public IActionResult Index() {
            var result = _flights.GetAllView();
            ViewBag.models = result;
            return View();
        }

        public IActionResult Index(LookAttrViewModel model) {
            var sql = GenerateSql(model);
            if (string.IsNullOrEmpty(sql))
                return RedirectToAction("Index");
            var result = _flights.GetAllView(sql);
            ViewBag.models = result;
            return View(model);
        }

        [HttpGet]
        public IActionResult Add(string returnUrtl = null) {
            var airports = _dbContext.Airports.ToList();
            var companies = _dbContext.AviaCompanies.ToList();
            var planes = _dbContext.Airplanes.ToList();
            ViewBag.Airports = new SelectList(airports, "Id", "Name");
            ViewBag.Companies = new SelectList(companies, "Id", "CompanyName");
            ViewBag.Airplanes = new SelectList(planes, "Id", "Model");
            return View();
        }

        [HttpPost]
        public IActionResult Add(FlightViewModel model, string returnUrtl = null) {
            var flight = new Flight{
                Id = model.Id,
                Airplane = _dbContext.Airplanes.Find(model.AirplaneId),
                AviaCompany = _dbContext.AviaCompanies.Find(model.AviaCompanyId),
                HomeAirport = _dbContext.Airports.Find(model.HomeAirportId),
                DestinationAirport = _dbContext.Airports.Find(model.DestinationAirportId),
                Departure = model.Departure,
                Arrival = model.Arrival
            };
            var curFlights = _dbContext.Flights.ToList().OrderByDescending(item => item.Id);
            if (curFlights == null)
                flight.Id = 1;
            else
                flight.Id = curFlights.First().Id + 1;
            _dbContext.Flights.Add(flight);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id, string returnUrtl = null) {
            var flight = _dbContext.Flights.Find(id);
            var model = new FlightViewModel {
                Id = flight.Id,
                AirplaneId = flight.AirplaneId,
                AviaCompanyId = flight.AviaCompanyId,
                HomeAirportId = flight.HomeAirportId,
                DestinationAirportId = flight.DestinationAirportId,
                Departure = flight.Departure,
                Arrival = flight.Arrival
            };
            var airports = _dbContext.Airports.ToList();
            var companies = _dbContext.AviaCompanies.ToList();
            var planes = _dbContext.Airplanes.ToList();
            ViewBag.Airports = new SelectList(airports, "Id", "Name");
            ViewBag.Companies = new SelectList(companies, "Id", "CompanyName");
            ViewBag.Airplanes = new SelectList(planes, "Id", "Model");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(FlightViewModel model, string returnUrtl = null) {
            var flight = new Flight {
                Id = model.Id,
                Airplane = _dbContext.Airplanes.Find(model.AirplaneId),
                AviaCompany = _dbContext.AviaCompanies.Find(model.AviaCompanyId),
                HomeAirport = _dbContext.Airports.Find(model.HomeAirportId),
                DestinationAirport = _dbContext.Airports.Find(model.DestinationAirportId),
                Departure = model.Departure,
                Arrival = model.Arrival
            };
            _dbContext.Flights.Update(flight);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ReturnBackUp() {
            var flights = _dbContext.BackUpFlights.ToList();
            foreach (var flight in flights) {
                _dbContext.Flights.Add(new Flight {
                    Id = flight.Id,
                    HomeAirportId = flight.HomeAirportId,
                    DestinationAirportId = flight.DestinationAirportId,
                    Departure = flight.Departure,
                    Arrival = flight.Arrival,
                    AirplaneId = flight.AirplaneId,
                    AviaCompanyId = flight.AviaCompanyId
                });
            }
            _dbContext.BackUpFlights.RemoveRange(flights);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Settings() {
            var settings = JsonConvert.DeserializeObject<Settings>(System.IO.File.ReadAllText(Path.GetFullPath(Path.Combine("App_data", "Settings.json"))));
            return View(settings);
        }

        [HttpPost]
        public IActionResult Settings(Settings NewSettings) {
            var settings = JsonConvert.DeserializeObject<Settings>(System.IO.File.ReadAllText(Path.GetFullPath(Path.Combine("App_data", "Settings.json"))));
            if (settings.UseTrigger != NewSettings.UseTrigger) {
                if (NewSettings.UseTrigger) {
                    var sql = System.IO.File.ReadAllText(Path.GetFullPath(Path.Combine("App_data", "trigger.sql")));
                    var connection = _dbContext.Database.GetDbConnection();
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                else {
                    var sql = "DROP TRIGGER `delete_flight`;";
                    var connection = _dbContext.Database.GetDbConnection();
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            if (settings.EventTime != NewSettings.EventTime) {
                var sql = @"DROP EVENT IF EXISTS delete_every_day; CREATE EVENT delete_every_day
    ON SCHEDULE EVERY 1 DAY STARTS '" + DateTime.Now.ToString(@"yyyy-MM-dd") + " " 
    + NewSettings.EventTime.ToString()+ "' - INTERVAL 1 DAY" +
    @" COMMENT 'Clears out flights every day.'
    DO
      DELETE FROM flights WHERE departure < (SELECT CURDATE());";
                var connection = _dbContext.Database.GetDbConnection();
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
                connection.Close();
            }
            var settingsJson = JsonConvert.SerializeObject(NewSettings);
            System.IO.File.WriteAllText(Path.GetFullPath(Path.Combine("App_data", "settings.json")), settingsJson);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id, string returnUrtl = null) {
            var flight = _dbContext.Flights.Find(id);
            var model = new ShowFlightModel {
                Id = flight.Id,
                Plane = _dbContext.Airplanes.Find(flight.AirplaneId).Model,
                Company = _dbContext.AviaCompanies.Find(flight.AviaCompanyId).CompanyName,
                HomeAirport = _dbContext.Airports.Find(flight.HomeAirportId).Name,
                DestinationAirport = _dbContext.Airports.Find(flight.DestinationAirportId).Name,
                Departure = flight.Departure,
                Arrival = flight.Arrival
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(ShowFlightModel model, string returnUrtl = null) {
            _dbContext.Flights.Remove(_dbContext.Flights.Find(model.Id));
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        private string GenerateSql(LookAttrViewModel model) {
            var sql = "";
            if (!string.IsNullOrEmpty(model.CompanyCountry)) {
                sql += "AND ac.country IN (" + MakeForMySql(model.CompanyCountry) + ") ";
            }
            if (!string.IsNullOrEmpty(model.CompanyName)) {
                sql += "AND ac.companyname IN (" + MakeForMySql(model.CompanyName) + ") ";
            }
            if (!string.IsNullOrEmpty(model.AirportCountryFrom)) {
                sql += "AND ha.country IN (" + MakeForMySql(model.AirportCountryFrom) + ") ";
            }
            if (!string.IsNullOrEmpty(model.AirportCountryTo)) {
                sql += "AND da.country IN (" + MakeForMySql(model.AirportCountryTo) + ") ";
            }
            if (!string.IsNullOrEmpty(model.PlaneModel)) {
                sql += "AND p.model IN (" + MakeForMySql(model.PlaneModel) + ") ";
            }
            if (model.PlaneSeatsFrom != 0) {
                sql += "AND p.seats >=" + model.PlaneSeatsFrom + " ";
            }
            if (model.PlaneSeatsTo != 0) {
                sql += "AND p.seats <=" + model.PlaneSeatsTo + " ";
            }
            if (model.FlightDate != DateTime.Parse("01.01.0001 00:00:00")) {
                sql += "AND fd.departure >= '" + model.FlightDate.ToString("yyyy-MM-dd HH:mm:ss") + "' AND fd.departure <= '" 
                    + model.FlightDate.Date.ToString(@"yyyy-MM-dd") + " 23:59:59' ";
            }
            return sql;
        }
        
        private string MakeForMySql(string param) {
            List<string> Params = new List<string>();
            string temp = "";
            for (int i = 0; i < param.Length; i++) {
                temp = "";
                while (i < param.Length && param[i] != ',') {
                    temp += param[i];
                    i++;
                }
                Params.Add(temp);
            }
            string result = "";
            foreach(var p in Params)
                result += "'" + p +  "',";
            result = result.Remove(result.Length - 1);
            return result;
        }

    }
}