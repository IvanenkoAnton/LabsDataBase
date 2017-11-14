using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using Lab2.Models;

namespace Lab2.Repositories
{
    public class FlightRepository : IRepository<FlightModel> {

        private string connectionString = "Server=localhost;Port=3306;Database=flights;Uid=root;Pwd=0212;SslMode=none";

        private MySqlConnection Connection;

        public FlightRepository() {
            Connection = new MySqlConnection(connectionString);
        }

        public void Add(FlightModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "INSERT INTO `FlightDetails` VALUES("+entity.Id+","+entity.AirplaneId+","+entity.AviaCompanyId+","+entity.HomeAirportId+","+
                entity.DestinationAirportId+", '"+entity.Departure.ToString("yyyy-MM-dd HH:mm:ss") +"', '"+entity.Arrival.ToString("yyyy-MM-dd HH:mm:ss") +"');";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public void Delete(FlightModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "DELETE FROM `FlightDetails` WHERE FlightDetailsID = " +entity.Id+" LIMIT 1;";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public FlightModel Get(int id) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "SELECT * FROM `FlightDetails` WHERE FlightDetailsID = " + id + ";";
            command.CommandText = sql;
            var dataReader = command.ExecuteReader();
            var result = new FlightModel();
            while (dataReader.Read()) {
                result.Id = id;
                result.AviaCompanyId = Int32.Parse(dataReader["AviaCompanyID"].ToString());
                result.AirplaneId = Int32.Parse(dataReader["AirplaneID"].ToString());
                result.HomeAirportId = Int32.Parse(dataReader["HomeAirportID"].ToString());
                result.DestinationAirportId = Int32.Parse(dataReader["DestinationAirportID"].ToString());
                result.Departure = DateTime.Parse(dataReader["Departure"].ToString());
                result.Arrival = DateTime.Parse(dataReader["Arrival"].ToString());
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public IEnumerable<FlightModel> GetAll() {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "SELECT * FROM `FlightDetails`;";
            command.CommandText = sql;
            var dataReader = command.ExecuteReader();
            var result = new List<FlightModel>();
            while (dataReader.Read()) {
                result.Add(new FlightModel {
                    Id = Int32.Parse(dataReader["FlightDetailsID"].ToString()),
                    AviaCompanyId = Int32.Parse(dataReader["AviaCompanyID"].ToString()),
                    AirplaneId = Int32.Parse(dataReader["AirplaneID"].ToString()),
                    HomeAirportId = Int32.Parse(dataReader["HomeAirportID"].ToString()),
                    DestinationAirportId = Int32.Parse(dataReader["DestinationAirportID"].ToString()),
                    Departure = DateTime.Parse(dataReader["Departure"].ToString()),
                    Arrival = DateTime.Parse(dataReader["Arrival"].ToString())
                });
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public IEnumerable<ShowFlightModel> GetAllView() {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = @"SELECT fd.FlightDetailsId as Id, ac.companyname AS Company,p.model AS Plane, ha.name AS Home, da.name AS Destination, fd.departure AS Departure, fd.Arrival AS Arrival
FROM FlightDetails AS fd,
	 AviaCompanies AS ac,
	 Airplanes AS p,
	 airports AS ha,
	 airports AS da
	 WHERE fd.airplaneid = p.airplaneid
	 AND fd.aviacompanyid = ac.aviacompanyid
     AND fd.homeairportid = ha.airportid
	 AND fd.destinationairportid = da.airportid ORDER BY fd.departure;";
            command.CommandText = sql;
            var result = new List<ShowFlightModel>();
            var dataReader = command.ExecuteReader();
            while (dataReader.Read()) {
                result.Add(new ShowFlightModel {
                    Id = Int32.Parse(dataReader["Id"].ToString()),
                    Company = dataReader["Company"].ToString(),
                    Plane = dataReader["Plane"].ToString(),
                    HomeAirport = dataReader["Home"].ToString(),
                    DestinationAirport = dataReader["Destination"].ToString(),
                    Departure = DateTime.Parse(dataReader["Departure"].ToString()),
                    Arrival = DateTime.Parse(dataReader["Arrival"].ToString())
                });
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public IEnumerable<ShowFlightModel> GetAllView(string addSql) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = @"SELECT fd.FlightDetailsId as Id, ac.companyname AS Company,p.model AS Plane, ha.name AS Home, da.name AS Destination, fd.departure AS Departure, fd.Arrival AS Arrival
FROM FlightDetails AS fd,
	 AviaCompanies AS ac,
	 Airplanes AS p,
	 airports AS ha,
	 airports AS da
	 WHERE fd.airplaneid = p.airplaneid
	 AND fd.aviacompanyid = ac.aviacompanyid
     AND fd.homeairportid = ha.airportid
	 AND fd.destinationairportid = da.airportid " + addSql + "  ORDER BY fd.departure;";
            command.CommandText = sql;
            var result = new List<ShowFlightModel>();
            var dataReader = command.ExecuteReader();
            while (dataReader.Read()) {
                result.Add(new ShowFlightModel {
                    Id = Int32.Parse(dataReader["Id"].ToString()),
                    Company = dataReader["Company"].ToString(),
                    Plane = dataReader["Plane"].ToString(),
                    HomeAirport = dataReader["Home"].ToString(),
                    DestinationAirport = dataReader["Destination"].ToString(),
                    Departure = DateTime.Parse(dataReader["Departure"].ToString()),
                    Arrival = DateTime.Parse(dataReader["Arrival"].ToString())
                });
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public void Update(FlightModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "REPLACE INTO `FlightDetails` VALUES(" + entity.Id + "," + entity.AirplaneId + "," + entity.AviaCompanyId + "," + entity.HomeAirportId + "," +
                entity.DestinationAirportId + ",'" + entity.Departure.ToString("yyyy-MM-dd HH:mm:ss") + "','" + entity.Arrival.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}
