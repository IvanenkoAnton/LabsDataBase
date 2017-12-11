DROP TRIGGER IF EXISTS `delete_flight`;
CREATE TRIGGER `delete_flight` before delete ON `flights`
FOR EACH ROW BEGIN
  INSERT INTO backupflights Set id = OLD.id, aviacompanyid = OLD.aviacompanyid, homeairportid = OLD.homeairportid, 
  destinationairportid = OLD.destinationairportid, airplaneid = OLD.airplaneid, departure = OLD.departure, arrival = OLD.arrival;
END