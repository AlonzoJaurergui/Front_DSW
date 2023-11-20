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

        public async Task<string> Actualizar(Extra extra)
        {
            var extraCollection = _database.GetCollection<Extra>("extra");
            var filter = Builders<Extra>.Filter.Eq(x => x.id, extra.id);
            var update = Builders<Extra>.Update
                .Set(x => x.descripcion, extra.descripcion);                
            var result = await extraCollection.UpdateOneAsync(filter, update);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return "Extra actualizado correctamente en MongoDB.";
            }
            else
            {
                return "Error al actualizar el extra en MongoDB.";
            }
        }

        public async Task<string> Eliminar(string id)
        {
            var extraCollection = _database.GetCollection<Extra>("extra");
            var filter = Builders<Extra>.Filter.Eq(x => x.id, id);
            var result = await extraCollection.DeleteOneAsync(filter);
            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                return "Extra eliminada correctamente";
            }
            else
            {
                return "Hubo un error al eliminar el extra";
            }
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
