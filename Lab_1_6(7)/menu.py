from database import Database
from director import Director
from film import Film

class Menu(object):
   def __init__ (self):
       self.db = Database()
       self.db.load()

   @staticmethod
   def render():
       items = ["1.Show all directors",
                 "2.Show all films",
                 "3.Add director",
                 "4.Add film",
                 "5.Select all films by country",
                 "6.Delete director",
                 "7.Delete film",
                 "8.Exit"]

       print("Program menu:\n")
       print('\n'.join(item for item in items))


   def choose(self):
       number = input("Choose menu item: ")
       if number == "1":
           print(self.db.directors)
           return True
       elif number == "2":
           print(self.db.films)
           return True
       elif number == "3":
           self.add_director()
           return True
       elif number == "4":
           self.add_film();
           return True
       elif number == "5":
           self.find()
           return True
       elif number == "6":
           self.delete_director()
           return True
       elif number == "7":
           self.delete_film()
           return True
       elif number == "8":
           self.db.save()
           return False

   def add_director(self):
        dname = input("Director's name: ")
        dage = input("Director's age: ")
        dcountry = input("Director's country: ")
        self.db.directors.add(Director(self.db.directors.generate_id(), dname, dage, dcountry))

   def add_film(self):
       did = input("Director's id: ")
       fname = input("Films's name: ")
       fcountry = input("Films's country: ")
       fdate = input("Films's year: ")
       if did.isdecimal():
           try:
               self.db.films.add(Film(int(did), self.db.films.generate_id(), fname, fcountry, fdate), self.db.directors)
           except Exception as ex:
               print(ex.args)
       else:
           print("Director's Id isn't right")

   def find(self):
       country = input("Enter country: ")
       films = self.db.films.find_by_country(country)
       if films is not None:
           print('\n'.join(str(film) for film in films))
       else:
           print("Not found")

   def delete_director(self):
       did = input("Enter director's id: ");
       if did.isdecimal():
           self.db.directors.remove(int(did), self.db.films)
       else:
           print("Id isn't right")

   def delete_film(self):
       fid = input("Enter films's id: ");
       if fid.isdecimal():
           self.db.films.remove(int(fid))
       else:
           print("Id isn't right")

