using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;

using Lab2.Models;
namespace Lab2.Repositories
{
    public class AviaCompanyRepository : IRepository<AviaCompanyModel> {

        private string connectionString = "Server=localhost;Port=3306;Database=flights;Uid=root;Pwd=0212;SslMode=none";

        private MySqlConnection Connection;

        public AviaCompanyRepository() {
            Connection = new MySqlConnection(connectionString);
        }

        public void Add(AviaCompanyModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "INSERT INTO `AviaCompanies` VALUES(" + entity.Id + ",'" + entity.CompanyName +"', '" + entity.Country + "');";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public void Delete(AviaCompanyModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "DELETE FROM `AviaCompanies` WHERE AviaCompanyID = " + entity.Id + " LIMIT 1;";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }

        public AviaCompanyModel Get(int id) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "SELECT * FROM `AviaCompanies` WHERE AviaCompanyID = " + id + ";";
            command.CommandText = sql;
            var dataReader = command.ExecuteReader();
            var result = new AviaCompanyModel();
            while (dataReader.Read()) {
                result.Id = id;
                result.CompanyName = dataReader["CompanyName"].ToString();
                result.Country = dataReader["Country"].ToString();
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public IEnumerable<AviaCompanyModel> GetAll() {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "SELECT * FROM `AviaCompanies`;";
            command.CommandText = sql;
            var dataReader = command.ExecuteReader();
            var result = new List<AviaCompanyModel>();
            while (dataReader.Read()) {
                result.Add(new AviaCompanyModel {
                    Id = Int32.Parse(dataReader["AviaCompanyID"].ToString()),
                    CompanyName = dataReader["CompanyName"].ToString(),
                    Country = dataReader["Country"].ToString()
                });
            }
            dataReader.Close();
            Connection.Close();
            return result;
        }

        public void Update(AviaCompanyModel entity) {
            Connection.Open();
            var command = Connection.CreateCommand();
            var sql = "REPLACE INTO `AviaCompanies` VALUES(" + entity.Id + ",'" + entity.CompanyName + "', '" + entity.Country + "');";
            command.CommandText = sql;
            command.ExecuteNonQuery();
            Connection.Close();
        }
    }
}
