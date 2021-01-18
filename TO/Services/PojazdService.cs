using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TO.DbModels;

namespace TO.Services
{
    public class PojazdService
    {
        private readonly IMongoCollection<Pojazd> _pojazdy;
        public PojazdService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _pojazdy = database.GetCollection<Pojazd>(settings.PojazdCollectionName);
        }
        public List<Pojazd> Get() => _pojazdy.Find(pojazd => true).ToList();
        public Pojazd Get(string id) => _pojazdy.Find(pojazd => pojazd.Id == id).FirstOrDefault();
        public List<Pojazd> Search(string vin) => _pojazdy.Find(pojazd => pojazd.Vin == vin).ToList();

        public Pojazd Create(Pojazd pojazd)
        {
            _pojazdy.InsertOne(pojazd);
            return pojazd;
        }
        public void Update(string id, Pojazd pojazd)
        {
            _pojazdy.ReplaceOne(x => x.Id == id, pojazd);
        }
        public void Remove(string id)
        {
            _pojazdy.DeleteOne(x => x.Id == id);
        }
        public void Remove(Pojazd pojazd)
        {
            _pojazdy.DeleteOne(x => x.Id == pojazd.Id);
        }
    }
}
