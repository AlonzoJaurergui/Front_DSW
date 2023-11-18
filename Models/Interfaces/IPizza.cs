using PizzaNicola_AspNetCore.Models.Entidades;

namespace PizzaNicola_AspNetCore.Models.Interfaces
{
    public interface IPizza
    {
        IEnumerable<Pizza> GetPizzas();
        Task<string> Insertar(Pizza pizza);
        Task<string> Actualizar(Pizza pizza);
        Task<string> Eliminar(string id);
    }
}
