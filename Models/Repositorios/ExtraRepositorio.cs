//using Microsoft.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Data;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;

namespace PizzaNicola_AspNetCore.Models.Repositorios
{
    public class ExtraRepositorio : IExtra
    {
        private IMongoDatabase _database;

        public ExtraRepositorio()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
                        .Build();
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("pizzeria_nicola_trux");
        }

        public IEnumerable<Extra> GetExtra()
        {
            var extrasCollection = _database.GetCollection<Extra>("extra");
            var filter = Builders<Extra>.Filter.Empty;
            var extras = extrasCollection.Find(filter).ToList();

            return extras;
        }

        public async Task<string> Insertar(Extra extra)
        {
            var extrasCollection = _database.GetCollection<Extra>("extra");
            extrasCollection.InsertOne(extra);
            return "Extra insertado correctamente en MongoDB.";
        }
    }
}
