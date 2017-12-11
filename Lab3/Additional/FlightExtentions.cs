using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using Lab3.Models;
using Microsoft.Extensions.Configuration;

namespace Lab3.Additional
{
    public class FlightExtentions {

        private string _connectionString;
        private MySqlConnection Connection;

        public FlightExtentions(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            Connection = new MySqlConnection(_connectionString);
        }

        public IEnumerable<ShowFlightModel> GetAllView() {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = @"SELECT fd.Id as Id, ac.companyname AS Company,p.model AS Plane, ha.name AS Home, da.name AS Destination, fd.departure AS Departure, fd.Arrival AS Arrival
FROM flights AS fd,
	 aviacompanies AS ac,
	 airplanes AS p,
	 airports AS ha,
	 airports AS da
	 WHERE fd.airplaneid = p.id
	 AND fd.aviacompanyid = ac.id
     AND fd.homeairportid = ha.id
	 AND fd.destinationairportid = da.id ORDER BY fd.departure;";
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
            var sql = @"SELECT fd.Id as Id, ac.companyname AS Company,p.model AS Plane, ha.name AS Home, da.name AS Destination, fd.departure AS Departure, fd.Arrival AS Arrival
FROM flights AS fd,
	 aviacompanies AS ac,
	 airplanes AS p,
	 airports AS ha,
	 airports AS da
	 WHERE fd.airplaneid = p.id
	 AND fd.aviacompanyid = ac.id
     AND fd.homeairportid = ha.id
	 AND fd.destinationairportid = da.id " + addSql + "  ORDER BY fd.departure;";
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
    }
}
