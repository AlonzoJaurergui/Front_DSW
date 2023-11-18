using PizzaNicola_AspNetCore.Models.Entidades;

namespace PizzaNicola_AspNetCore.Models.Interfaces
{
    public interface IUsuario
    {
        bool SeguridadUsuario(Usuario usuario);
        string Registrar(Usuario usuario);
    }
}
