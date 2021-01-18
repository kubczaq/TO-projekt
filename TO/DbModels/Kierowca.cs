using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TO.DbModels
{
    public class Kierowca
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string PESEL { get; set; }
        public string NumerTelefonu { get; set; }
        public string DataUrodzenia { get; set; }
        public string Kategorie { get; set; }
        public string Zatrzymanie { get; set; }
        public string Zakaz { get; set; }
        public string Utrata { get; set; }
        public decimal Punkty { get; set; }
        public string Uwagi { get; set; }
        public List<ObjectId> Pojazdy { get; set; }
    }
}
