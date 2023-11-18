using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;
using System.Data;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Components.Forms;
using SharpCompress.Common;
using Microsoft.AspNetCore.Mvc;

namespace PizzaNicola_AspNetCore.Models.Repositorios
{
    public class PizzaRepositorio : IPizza
    {
        
        private IMongoDatabase _database;

        public PizzaRepositorio()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
                        .Build();
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("pizzeria_nicola_trux");
        }

        public async Task<string> Actualizar(Pizza pizza)
        {
            var pizzaCollection = _database.GetCollection<Pizza>("pizza");
            var filter = Builders<Pizza>.Filter.Eq(x => x.id, pizza.id);
            var update = Builders<Pizza>.Update
                .Set(x => x.nombrePizza, pizza.nombrePizza)
                .Set(x => x.ingredientes, pizza.ingredientes)
                .Set(x => x.tamanio, pizza.tamanio)
                .Set(x => x.tipoMasa, pizza.tipoMasa)
                .Set(x => x.precio, pizza.precio)
                .Set(x => x.stock, pizza.stock);
            var result = await pizzaCollection.UpdateOneAsync(filter, update);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return "Pizza actualizada correctamente en MongoDB.";
            }
            else
            {
                return "Error al actualizar la pizza en MongoDB.";
            }
        }

        public async Task<string> Eliminar(string id)
        {
            var pizaCollection = _database.GetCollection<Pizza>("pizza");
            var filter = Builders<Pizza>.Filter.Eq(x => x.id, id);
            var result = await pizaCollection.DeleteOneAsync(filter);
            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                return "Pizza eliminada correctamente";
            }
            else 
            {
                return "Hubo un error al eliminar la pizza";
            }
        }

        public IEnumerable<Pizza> GetPizzas()
        {
            var pizzasCollection = _database.GetCollection<Pizza>("pizza");
            var filter = Builders<Pizza>.Filter.Empty;
            var pizzas = pizzasCollection.Find(filter).ToList();
            return pizzas;
        }

        public async Task<string> Insertar(Pizza pizza)
        {
            var pizzaCollection = _database.GetCollection<Pizza>("pizza");
            pizzaCollection.InsertOne(pizza);
            return "Pizza insertado correctamente en MongoDB.";
        }
    }
}
