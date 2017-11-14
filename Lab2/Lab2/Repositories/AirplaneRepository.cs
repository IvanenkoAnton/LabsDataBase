using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using Lab2.Models;
namespace Lab2.Repositories
{
    public class AirplaneRepository : IRepository<AirplaneModel> {

        private string connectionString = "Server=localhost;Port=3306;Database=flights;Uid=root;Pwd=0212;SslMode=none";

        private MySqlConnection Connection;

        public AirplaneRepository() {
            Connection = new MySqlConnection(connectionString);
        }

        public void Add(AirplaneModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "INSERT INTO `Airplanes` VALUES(" + entity.Id + ",'" + entity.Model + "'," + entity.Seats.ToString() + ",'" 
                + entity.LastCheckUp.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public void Delete(AirplaneModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "DELETE FROM `Airplanes` WHERE AirplaneID = " + entity.Id + " LIMIT 1;";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public AirplaneModel Get(int id) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "SELECT * FROM `Airplanes` WHERE AirplaneID = " + id + ";";
            command.CommandText = sql;
            var dataReader = command.ExecuteReader();
            var result = new AirplaneModel();
            while (dataReader.Read()) {
                result.Id = id;
                result.Model = dataReader["Model"].ToString();
                result.Seats = Int32.Parse(dataReader["Seats"].ToString());
                result.LastCheckUp = DateTime.Parse(dataReader["LastCheckUp"].ToString());
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public IEnumerable<AirplaneModel> GetAll() {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "SELECT * FROM `Airplanes`;";
            command.CommandText = sql;
            var dataReader = command.ExecuteReader();
            var result = new List<AirplaneModel>();
            while (dataReader.Read()) {
                result.Add(new AirplaneModel {
                    Id = Int32.Parse(dataReader["AirplaneID"].ToString()),
                    Model = dataReader["Model"].ToString(),
                    Seats = Int32.Parse(dataReader["Seats"].ToString()),
                    LastCheckUp = DateTime.Parse(dataReader["LastCheckUp"].ToString())
                });
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public void Update(AirplaneModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "REPLACE INTO `Airplanes` VALUES(" + entity.Id + ",'" + entity.Model + "'," + entity.Seats.ToString() + ",'"
                + entity.LastCheckUp.ToString("yyyy-MM-dd HH:mm:ss") + "');";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}
