class Directors(object):
    def __init__(self):
        self.directors = []

    def add(self, director):
        self.directors.append(director)

    def generate_id(self):
        if (len(self.directors) == 0):
            return 0
        return self.directors[len(self.directors) - 1].did + 1

    def remove(self, did, films):
        if films.find_by_did(did) is not None:
            flag = input("There are films of this director in the database.\n "
                         "The will be removed. Are you sure?(y/n): ").lower()
            if flag != 'y':
                return
            films.remove_with_director(did)
        for director in self.directors:
            if director.did == did:
                self.directors.remove(director)

    def __str__(self):
        return '\n'.join(str(director) for director in self.directors)

    def exists(self, did):
        for director in self.directors:
            if director.did == did:
                return True
        return False
