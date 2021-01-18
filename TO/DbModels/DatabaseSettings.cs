using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TO.DbModels
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string PojazdCollectionName { get; set; }
        public string KierowcaCollectionName { get; set; }
        public string UzytkownikCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IDatabaseSettings
    {
        string PojazdCollectionName { get; set; }
        string KierowcaCollectionName { get; set; }
        string UzytkownikCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
