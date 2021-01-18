using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TO.DbModels
{
    public class Pojazd
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Typ{ get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
        public string DataProdukcji { get; set; }
        public decimal Pojemnosc { get; set; }
        public decimal Waga { get; set; }
        public string Rejestracja { get; set; }
        public string Wlasciciel { get; set; }
        public string Ubezpieczenie { get; set; }
        public string Badanie { get; set; }
        public string ZatrzymanieDowodu { get; set; }
        public string UtrataDowodu { get; set; }
        public string Kradziez { get; set; }
        public string Uwagi { get; set; }
    }
}
