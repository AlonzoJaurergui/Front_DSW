using PizzaNicola_AspNetCore.Models.Entidades;

namespace PizzaNicola_AspNetCore.Models.Interfaces
{
    public interface IExtra
    {
        IEnumerable<Extra> GetExtra();
        Task<string> Insertar(Extra extra);
        Task<string> Actualizar(Extra extra);
        Task<string> Eliminar(string id);
    }
}
