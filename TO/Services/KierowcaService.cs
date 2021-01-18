using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TO.DbModels;

namespace TO.Services
{
    public class KierowcaService
    {
        private readonly IMongoCollection<Kierowca> _kierowcy;
        public KierowcaService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _kierowcy = database.GetCollection<Kierowca>(settings.KierowcaCollectionName);
        }
        public List<Kierowca> Get() => _kierowcy.Find(kierowca => true).ToList();
        public Kierowca Get(string id) => _kierowcy.Find(kierowca => kierowca.Id == id).FirstOrDefault();
        public List<Kierowca> Search(string PESEL) => _kierowcy.Find(kierowca => kierowca.PESEL == PESEL).ToList();

        public Kierowca Create(Kierowca kierowca)
        {
            _kierowcy.InsertOne(kierowca);
            return kierowca;
        }
        public void Update(string id, Kierowca kierowca)
        {
            _kierowcy.ReplaceOne(x => x.Id == id, kierowca);
        }
        public void Remove(string id)
        {
            _kierowcy.DeleteOne(x => x.Id == id);
        }
        public void Remove(Kierowca kierowca)
        {
            _kierowcy.DeleteOne(x => x.Id == kierowca.Id);
        }
        public bool Wypozycz(string id, string pojazdId)
        {
            var kierowcaPojazd = Get().SelectMany(x => x.Pojazdy, (x, y) => new { x.Id, PojazdId = y.ToString() });
            if(kierowcaPojazd.Any(x => x.PojazdId == pojazdId))
            {
                //jak wypozyczony juz
                return false;
            }
            
            var updated = Get(id);
            updated.Pojazdy.Add(MongoDB.Bson.ObjectId.Parse(pojazdId));
            Update(id, updated);
            return true;
        }
        public void Zwroc(string id, string pojazdId)
        {
            var kierowca = Get(id);
            var pojazdObjId = MongoDB.Bson.ObjectId.Parse(pojazdId);
            kierowca.Pojazdy.RemoveAll(x => x == pojazdObjId);
            Update(id, kierowca);
        }
    }
}
