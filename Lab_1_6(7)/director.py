class Director(object):
    def __init__(self,did, dname, dage, dcountry):
        self.did = did
        self.dname = dname
        self.dage = dage
        self.dcountry = dcountry

    def __str__(self):
        return "ID:%d, Name:%s, Age:%s, Country:%s"%(self.did, self.dname, self.dage, self.dcountry)