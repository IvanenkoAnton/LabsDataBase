class Film(object):
    def __init__(self, did, fid, fname, fcountry, fdate):
        self.did = did
        self.fid = fid
        self.fname = fname
        self.fcountry = fcountry
        self.fdate = fdate

    def __str__(self):
        return "Director ID:%d, ID:%d, Name:%s, Country:%s, Date:%s"%(self.did, self.fid, self.fname, self.fcountry, self.fdate)