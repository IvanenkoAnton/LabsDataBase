class Films(object):
    def __init__(self):
        self.films = []

    def add(self, film, directors):
        if directors.exists(film.did):
            self.films.append(film)
        else:
            raise Exception("Director does not exist")

    def generate_id(self):
        if (len(self.films) == 0):
            return 0
        return self.films[len(self.films) - 1].fid + 1

    def find_by_country(self, country):
        films = []
        for film in self.films:
            if film.fcountry.lower() == country.lower():
                films.append(film)
        if len(films) == 0:
            return None
        return films

    def find_by_did(self, did):
        films = []
        for film in self.films:
            if film.did == did:
                films.append(film)
        if len(films) == 0:
            return None
        return films

    def remove(self, fid):
        for film in self.films:
            if film.fid == fid:
                self.films.remove(film)

    def remove_with_director(self, did):
        for film in self.films:
            if film.did == did:
                self.films.remove(film)

    def __str__(self):
        return '\n'.join(str(film) for film in self.films)