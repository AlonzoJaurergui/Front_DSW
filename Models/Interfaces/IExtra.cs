using PizzaNicola_AspNetCore.Models.Entidades;

namespace PizzaNicola_AspNetCore.Models.Interfaces
{
    public interface IExtra
    {
        IEnumerable<Extra> GetExtra();
        Task<string> Insertar(Extra extra);
    }
}
