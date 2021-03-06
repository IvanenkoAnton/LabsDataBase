﻿// <auto-generated />
using Lab3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Lab3.Migrations
{
    [DbContext(typeof(FlightContext))]
    [Migration("20171203222729_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("Lab3.Models.Airplane", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastCheckUp");

                    b.Property<string>("Model");

                    b.Property<int>("Seats");

                    b.HasKey("Id");

                    b.ToTable("Airplanes");
                });

            modelBuilder.Entity("Lab3.Models.Airport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Airports");
                });

            modelBuilder.Entity("Lab3.Models.AviaCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName");

                    b.Property<string>("Country");

                    b.HasKey("Id");

                    b.ToTable("AviaCompanies");
                });

            modelBuilder.Entity("Lab3.Models.BackUpFlight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AirplaneId");

                    b.Property<DateTime>("Arrival");

                    b.Property<int>("AviaCompanyId");

                    b.Property<DateTime>("Departure");

                    b.Property<int>("DestinationAirportId");

                    b.Property<int>("HomeAirportId");

                    b.HasKey("Id");

                    b.HasIndex("AirplaneId");

                    b.HasIndex("AviaCompanyId");

                    b.HasIndex("DestinationAirportId");

                    b.HasIndex("HomeAirportId");

                    b.ToTable("BackUpFlights");
                });

            modelBuilder.Entity("Lab3.Models.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AirplaneId");

                    b.Property<DateTime>("Arrival");

                    b.Property<int>("AviaCompanyId");

                    b.Property<DateTime>("Departure");

                    b.Property<int>("DestinationAirportId");

                    b.Property<int>("HomeAirportId");

                    b.HasKey("Id");

                    b.HasIndex("AirplaneId");

                    b.HasIndex("AviaCompanyId");

                    b.HasIndex("DestinationAirportId");

                    b.HasIndex("HomeAirportId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("Lab3.Models.BackUpFlight", b =>
                {
                    b.HasOne("Lab3.Models.Airplane", "Airplane")
                        .WithMany()
                        .HasForeignKey("AirplaneId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab3.Models.AviaCompany", "AviaCompany")
                        .WithMany()
                        .HasForeignKey("AviaCompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab3.Models.Airport", "DestinationAirport")
                        .WithMany()
                        .HasForeignKey("DestinationAirportId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab3.Models.Airport", "HomeAirport")
                        .WithMany()
                        .HasForeignKey("HomeAirportId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Lab3.Models.Flight", b =>
                {
                    b.HasOne("Lab3.Models.Airplane", "Airplane")
                        .WithMany()
                        .HasForeignKey("AirplaneId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab3.Models.AviaCompany", "AviaCompany")
                        .WithMany()
                        .HasForeignKey("AviaCompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab3.Models.Airport", "DestinationAirport")
                        .WithMany()
                        .HasForeignKey("DestinationAirportId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Lab3.Models.Airport", "HomeAirport")
                        .WithMany()
                        .HasForeignKey("HomeAirportId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
