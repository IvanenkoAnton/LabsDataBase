using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Lab2.Repositories;
using Lab2.Models;
using Lab2.Models.FlightsViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab2.Controllers
{
    public class FlightsController : Controller {

        private readonly FlightRepository _flights;
        private readonly AviaCompanyRepository _companies;
        private readonly AirportRepository _airports;
        private readonly AirplaneRepository _airplanes;

        public FlightsController (
                FlightRepository flights,
                AviaCompanyRepository companies,
                AirportRepository airports,
                AirplaneRepository airplanes
            ) {
            _flights = flights;
            _companies = companies;
            _airports = airports;
            _airplanes = airplanes;
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
            var ap = _airports.GetAll().ToList();
            var comp = _companies.GetAll().ToList();
            var pl = _airplanes.GetAll().ToList();
            ViewBag.Airports = new SelectList(ap, "Id", "Name");
            ViewBag.Companies = new SelectList(comp, "Id", "CompanyName");
            ViewBag.Airplanes = new SelectList(pl, "Id", "Model");
            return View();
        }

        [HttpPost]
        public IActionResult Add(FlightViewModel model, string returnUrtl = null) {
            var flight = new FlightModel {
                Id = model.Id,
                AirplaneId = model.AirplaneId,
                AviaCompanyId = model.AviaCompanyId,
                HomeAirportId = model.HomeAirportId,
                DestinationAirportId = model.DestinationAirportId,
                Departure = model.Departure,
                Arrival = model.Arrival
            };
            var curFlights = _flights.GetAll().ToList().OrderByDescending(item => item.Id);
            if (curFlights.Count() == 0)
                flight.Id = 1;
            else
                flight.Id = curFlights.First().Id + 1;
            _flights.Add(flight);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id, string returnUrtl = null) {
            var flight = _flights.Get(id);
            var model = new FlightViewModel {
                Id = flight.Id,
                AirplaneId = flight.AirplaneId,
                AviaCompanyId = flight.AviaCompanyId,
                HomeAirportId = flight.HomeAirportId,
                DestinationAirportId = flight.DestinationAirportId,
                Departure = flight.Departure,
                Arrival = flight.Arrival
            };
            var ap = _airports.GetAll().ToList();
            var comp = _companies.GetAll().ToList();
            var pl = _airplanes.GetAll().ToList();
            ViewBag.Airports = new SelectList(ap, "Id", "Name");
            ViewBag.Companies = new SelectList(comp, "Id", "CompanyName");
            ViewBag.Airplanes = new SelectList(pl, "Id", "Model");
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(FlightViewModel model, string returnUrtl = null) {
            var flight = new FlightModel {
                Id = model.Id,
                AirplaneId = model.AirplaneId,
                AviaCompanyId = model.AviaCompanyId,
                HomeAirportId = model.HomeAirportId,
                DestinationAirportId = model.DestinationAirportId,
                Departure = model.Departure,
                Arrival = model.Arrival
            };
            _flights.Update(flight);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id, string returnUrtl = null) {
            var flight = _flights.Get(id);
            var homeAirport = _airports.Get(flight.HomeAirportId);
            var destAirport = _airports.Get(flight.DestinationAirportId);
            var company = _companies.Get(flight.AviaCompanyId);
            var airplane = _airplanes.Get(flight.AirplaneId);
            var model = new ShowFlightModel {
                Id = flight.Id,
                Plane = airplane.Model,
                Company = company.CompanyName,
                HomeAirport = homeAirport.Name,
                DestinationAirport = destAirport.Name,
                Departure = flight.Departure,
                Arrival = flight.Arrival
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(ShowFlightModel model, string returnUrtl = null) {
            var flight = new FlightModel {
                Id = model.Id,
               
            };
            _flights.Delete(flight);
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
            if (!string.IsNullOrEmpty(model.MustBeWord)) {
                sql += "AND (MATCH(ha.name) AGAINST ('+" + model.MustBeWord + "' IN BOOLEAN MODE) OR MATCH(da.name) AGAINST ('+" + model.MustBeWord + "' IN BOOLEAN MODE)) ";
             }
            if (!string.IsNullOrEmpty(model.NoWord)) {
                sql += "AND (NOT MATCH(ha.name) AGAINST ('+" + model.NoWord + "' IN BOOLEAN MODE) AND  NOT MATCH(da.name) AGAINST ('+" + model.NoWord + "' IN BOOLEAN MODE)) ";
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