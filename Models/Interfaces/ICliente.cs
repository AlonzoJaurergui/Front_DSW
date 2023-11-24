using PizzaNicola_AspNetCore.Models.Entidades;

namespace PizzaNicola_AspNetCore.Models.Interfaces
{
    public interface ICliente
    {
        IEnumerable<Cliente> GetClientes();
        Task<string> Insertar(Cliente cliente);
        Task<string> Actualizar(Cliente cliente);
        Task<string> Eliminar(string id);
    }
}
