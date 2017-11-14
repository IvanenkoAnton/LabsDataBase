using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.IO;

using MySql.Data.MySqlClient;

using Lab2.Models;
using Lab2.Repositories;

namespace Lab2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            //Add our Repositories
            services.AddSingleton<FlightRepository>();
            services.AddSingleton<AirportRepository>();
            services.AddSingleton<AviaCompanyRepository>();
            services.AddSingleton<AirplaneRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            env.ContentRootPath = (Path.GetFullPath("App_Data"));

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var scope = app.ApplicationServices.CreateScope()) {
                CreateDatabase(scope, env);
            }

        }

        ///<summary>
        ///Create Database for this app if it doesn't exist
        ///</summary>
        private void CreateDatabase(IServiceScope scope, IHostingEnvironment env) {
            var connection = new MySqlConnection("Server=localhost;Port=3306;Database=flights;Uid=root;Pwd=0212;SslMode=none");
            try {
                connection.Open();
                connection.Close();
                return;
            }
            catch (Exception ex) {
                var companies = scope.ServiceProvider.GetRequiredService<AviaCompanyRepository>();
                var airports = scope.ServiceProvider.GetRequiredService<AirportRepository>();
                var airplanes = scope.ServiceProvider.GetRequiredService<AirplaneRepository>();
                var flights = scope.ServiceProvider.GetRequiredService<FlightRepository>();
                connection = new MySqlConnection("Server=localhost;Port=3306;Uid=root;Pwd=0212;SslMode=none");
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = File.ReadAllText(Path.Combine(env.ContentRootPath, "flights.sql"));
                command.ExecuteNonQuery();
                connection.Close();
                var Lines = File.ReadAllLines(Path.Combine(env.ContentRootPath,"companies.csv"));

                foreach (var line in Lines) {
                    companies.Add(GetCompanyFromString(Parse(line)));
                }
                Lines = File.ReadAllLines(Path.Combine(env.ContentRootPath, "airports.csv"));

                foreach (var line in Lines) {
                    airports.Add(GetAirportFromString(Parse(line)));
                }
                Lines = File.ReadAllLines(Path.Combine(env.ContentRootPath, "airplanes.csv"));

                foreach (var line in Lines) {
                    airplanes.Add(GetAirplaneFromString(Parse(line)));
                }

                Lines = File.ReadAllLines(Path.Combine(env.ContentRootPath, "flights_info.csv"));

                foreach (var line in Lines) {
                    flights.Add(GetFlightFromString(Parse(line)));
                }


            }
        }
        private AviaCompanyModel GetCompanyFromString(string[] param) {
            return new AviaCompanyModel {
                Id = Int32.Parse(param[0]),
                CompanyName = param[1],
                Country = param[2]
            };
        }
        private AirportModel GetAirportFromString(string[] param) {

            return new AirportModel {
                Id = Int32.Parse(param[0]),
                Name = param[1],
                City = param[2],
                Country = param[3]
            };
        }
        private AirplaneModel GetAirplaneFromString(string[] param) {

            return new AirplaneModel {
                Id = Int32.Parse(param[0]),
                Model = param[1],
                Seats = Int32.Parse(param[2]),
                LastCheckUp = DateTime.Parse(param[3])
            };
        }
        private FlightModel GetFlightFromString(string[] param) {
            return new FlightModel {
                Id = Int32.Parse(param[0]),
                AirplaneId = Int32.Parse(param[1]),
                AviaCompanyId = Int32.Parse(param[2]),
                HomeAirportId = Int32.Parse(param[3]),
                DestinationAirportId = Int32.Parse(param[4]),
                Departure = DateTime.Parse(param[5]),
                Arrival = DateTime.Parse(param[6])
            };
        }

        private string[] Parse(string value) {
            return value.Split(",");
        }
   
    }
}
