using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TO.DbModels;

namespace TO.Services
{
    public class UzytkownikService
    {
        private readonly IMongoCollection<Uzytkownik> _uzytkownicy;
        public UzytkownikService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _uzytkownicy = database.GetCollection<Uzytkownik>(settings.UzytkownikCollectionName);
        }
        public List<Uzytkownik> Get() => _uzytkownicy.Find(Uzytkownik => true).ToList();
        public Uzytkownik Get(string id) => _uzytkownicy.Find(Uzytkownik => Uzytkownik.Id == id).FirstOrDefault();

        public List<Uzytkownik> Loguj(string nazwa) => _uzytkownicy.Find(Uzytkownik => true).ToList();

        public Uzytkownik Create(Uzytkownik Uzytkownik)
        {
            _uzytkownicy.InsertOne(Uzytkownik);
            return Uzytkownik;
        }
        public void Update(string id, Uzytkownik Uzytkownik)
        {
            _uzytkownicy.ReplaceOne(x => x.Id == id, Uzytkownik);
        }
        public void Remove(string id)
        {
            _uzytkownicy.DeleteOne(x => x.Id == id);
        }
        public void Remove(Uzytkownik Uzytkownik)
        {
            _uzytkownicy.DeleteOne(x => x.Id == Uzytkownik.Id);
        }
    }
}
