SET FOREIGN_KEY_CHECKS=0;

CREATE DATABASE IF NOT EXISTS `flights`;

USE `flights`;

-- ----------------------------
-- Drop All tables
-- ----------------------------
DROP TABLE IF EXISTS `Airplanes`;
DROP TABLE IF EXISTS `Airports`;
DROP TABLE IF EXISTS `AviaCompanies`;
DROP TABLE IF EXISTS `FlightDetails`;

-- ----------------------------
-- Table structure for `Airplanes`
-- ----------------------------
CREATE TABLE `Airplanes` (
  `AirplaneID` int(11) NOT NULL DEFAULT '0',
  `Model` varchar(45) NOT NULL,
  `Seats` int(3),
  `LastCheckUp` datetime,
  PRIMARY KEY (`AirplaneID`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for `Airports`
-- ----------------------------
CREATE TABLE `Airports` (
  `AirportID` int(11) NOT NULL DEFAULT '0',
  `Name` varchar(45) NOT NULL,
  `City` varchar(45),
  `Country` varchar(45),
  PRIMARY KEY (`AirportID`),
  FULLTEXT (`Name`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for `AviaCompanies`
-- ----------------------------
CREATE TABLE `AviaCompanies` (
  `AviaCompanyID` int(11) NOT NULL DEFAULT '0',
  `CompanyName` varchar(45) NOT NULL,
  `Country` varchar(45),
  PRIMARY KEY (`AviaCompanyID`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Table structure for `FlightDetails`
-- ----------------------------
CREATE TABLE `FlightDetails` (
  `FlightDetailsID` int(11) NOT NULL DEFAULT '0',
  `AirplaneID` int(11),
  `AviaCompanyID` int(11),
  `HomeAirportID` int(11),
  `DestinationAirportID` int(11),
  `Departure` datetime,
  `Arrival` datetime,
  PRIMARY KEY (`FlightDetailsID`),
  FOREIGN KEY (`AirplaneID`) REFERENCES `Airplanes`(`AirplaneID`),
  FOREIGN KEY (`AviaCompanyID`) REFERENCES `AviaCompanies`(`AviaCompanyID`),
  FOREIGN KEY (`HomeAirportID`) REFERENCES `Airports`(`AirportID`),
  FOREIGN KEY (`DestinationAirportID`) REFERENCES `Airports`(`AirportID`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;
