using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using Lab2.Models;

namespace Lab2.Repositories
{
    public class AirportRepository : IRepository<AirportModel> {

        private string connectionString = "Server=localhost;Port=3306;Database=flights;Uid=root;Pwd=0212;SslMode=none";

        private MySqlConnection Connection;

        public AirportRepository() {
            Connection = new MySqlConnection(connectionString);
        }

        public void Add(AirportModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "INSERT INTO `Airports` VALUES(" + entity.Id + ",'" + entity.Name + "','" + entity.City + "','" + entity.Country+"');";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public void Delete(AirportModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "DELETE FROM `Airports` WHERE AirportID = " + entity.Id + " LIMIT 1;";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public AirportModel Get(int id) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "SELECT * FROM `Airports` WHERE AirportID = " + id + ";";
            command.CommandText = sql;
            var dataReader = command.ExecuteReader();
            var result = new AirportModel();
            while (dataReader.Read()) {
                result.Id = id;
                result.Name = dataReader["Name"].ToString();
                result.City = dataReader["City"].ToString();
                result.Country = dataReader["Country"].ToString();
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public IEnumerable<AirportModel> GetAll() {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "SELECT * FROM `Airports`;";
            command.CommandText = sql;
            var dataReader = command.ExecuteReader();
            var result = new List<AirportModel>();
            while (dataReader.Read()) {
                result.Add(new AirportModel {
                    Id = Int32.Parse(dataReader["AirportID"].ToString()),
                    Name = dataReader["Name"].ToString(),
                    City = dataReader["City"].ToString(),
                    Country = dataReader["Country"].ToString()
                });
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public void Update(AirportModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "REPLACE INTO `Airports` VALUES(" + entity.Id + ",'" + entity.Name + "'," + entity.City + "','" + entity.Country + "');";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}
