using MongoDB.Driver;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;

namespace PizzaNicola_AspNetCore.Models.Repositorios
{
    public class ClienteRepositorio : ICliente
    {
        private IMongoDatabase _database;

        public ClienteRepositorio()
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
                        .Build();
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("pizzeria_nicola_trux");
        }

        public async Task<string> Actualizar(Cliente cliente)
        {
            var clienteCollection = _database.GetCollection<Cliente>("cliente");
            var filter = Builders<Cliente>.Filter.Eq(x => x.id, cliente.id);
            var update = Builders<Cliente>.Update
                .Set(x => x.dni, cliente.dni)
                .Set(x => x.nombre, cliente.nombre)
                .Set(x => x.apellidos, cliente.apellidos)
                .Set(x => x.edad, cliente.edad)
                .Set(x => x.telefono, cliente.telefono)
                .Set(x => x.direccion, cliente.direccion);
            var result = await clienteCollection.UpdateOneAsync(filter, update);
            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                return "Cliente actualizado correctamente en MongoDB.";
            }
            else
            {
                return "Error al actualizar el cliente en MongoDB.";
            }
        }

        public async Task<string> Eliminar(string id)
        {
            var clienteCollection = _database.GetCollection<Cliente>("cliente");
            var filter = Builders<Cliente>.Filter.Eq(x => x.id, id);
            var result = await clienteCollection.DeleteOneAsync(filter);
            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                return "Cliente eliminado correctamente";
            }
            else
            {
                return "Hubo un error al eliminar el cliente";
            }
        }

        public IEnumerable<Cliente> GetClientes()
        {
            var clienteCollection = _database.GetCollection<Cliente>("cliente");
            var filter = Builders<Cliente>.Filter.Empty;
            var clientes = clienteCollection.Find(filter).ToList();
            return clientes;
        }

        public async Task<string> Insertar(Cliente cliente)
        {
            var clienteCollection = _database.GetCollection<Cliente>("cliente");
            clienteCollection.InsertOne(cliente);
            return "Cliente insertado correctamente en MongoDB.";
        }
    }
}
