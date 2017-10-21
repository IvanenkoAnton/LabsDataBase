import pickle
from directors import Directors
from films import Films
from director import Director
from film import Film


class Database(object):
    def __init__(self):
        self.directors_file = "directors.pickle"
        self.films_file = "films.pickle"
        self.directors = Directors()
        self.films = Films()

    def load(self):
        try:
            pickle_load = open(self.directors_file, "rb")
            self.directors = pickle.load(pickle_load)
            pickle_load.close()
            pickle_load = open(self.films_file, "rb")
            self.films = pickle.load(pickle_load)
            pickle_load.close()
        except Exception as ex:
            print("Can't load database, error: ", ex.args)
            self.directors.add(Director(0, "Ridley Scott", "79", "United Kingdom"))
            self.directors.add(Director(1, "Alexey Kiryushenko", "53", "Ukraine"))
            self.directors.add(Director(2, "Guy Ritchie", "49", "United Kingdom"))
            self.films.add(Film(0, 0, "Gladiator", "USA", "2000"), self.directors)
            self.films.add(Film(0, 1, "Alien", "USA", "1979"), self.directors)
            self.films.add(Film(1, 2, "Sluga Naroda", "Ukraine", "2015"), self.directors)
            self.films.add(Film(2, 3, "Sherlock Holmes", "USA", "2009"),  self.directors)
            self.films.add(Film(2, 4, "Lock, Stock and Two Smoking Barrels", "United Kingdom", "1998"), self.directors)
            print("Default database has been created successful!")

    def save(self):
        try:
            pickle_save = open(self.directors_file, "wb")
            pickle.dump(self.directors,pickle_save, pickle.HIGHEST_PROTOCOL)
            pickle_save.close()
            pickle_save = open(self.films_file, "wb")
            pickle.dump(self.films, pickle_save, pickle.HIGHEST_PROTOCOL)
            pickle_save.close()
            print("Database has been saved successful")
        except Exception as ex:
            print("Can't save database, error: ", ex.args)
