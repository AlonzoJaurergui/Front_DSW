//using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;
using MongoDB.Driver;

namespace PizzaNicola_AspNetCore.Models.Repositorios
{
    public class UsuarioRepositorio : IUsuario
    {
        private IMongoDatabase _database;

        public UsuarioRepositorio()
        {
            IConfiguration configuration = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
                       .Build();
            var connectionString = configuration.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("pizzeria_nicola_trux");
        }

        public async Task<string> Registrar(Usuario usuario)
        {
            var usuarioCollection = _database.GetCollection<Usuario>("usuario");
            usuarioCollection.InsertOneAsync(usuario);
            return "Usuario registrado correctamente!";
        }

        public async Task<bool> SeguridadUsuario(Usuario usuario)
        {
            var usuarioCollection = _database.GetCollection<Usuario>("usuario");
            var filter = Builders<Usuario>.Filter.And(
                Builders<Usuario>.Filter.Eq(u => u.login, usuario.login),
                Builders<Usuario>.Filter.Eq(u => u.password, usuario.password)
                );
            var usuarioEncontrado = usuarioCollection.Find(filter).FirstOrDefault();

            return usuarioEncontrado != null;
        }
    }
}
