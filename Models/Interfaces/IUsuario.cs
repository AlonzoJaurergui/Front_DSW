using PizzaNicola_AspNetCore.Models.Entidades;

namespace PizzaNicola_AspNetCore.Models.Interfaces
{
    public interface IUsuario
    {
       Task<bool> SeguridadUsuario(Usuario usuario);
        Task<string> Registrar(Usuario usuario);
    }
}
