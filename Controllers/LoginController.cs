using Microsoft.AspNetCore.Mvc;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;
using PizzaNicola_AspNetCore.Models.Repositorios;
using Microsoft.AspNetCore.Http;
using SharpCompress.Common;

namespace PizzaNicola_AspNetCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioRepositorio _usuarioR;

        public LoginController(UsuarioRepositorio usuarioRepositorio)
        {
            _usuarioR = usuarioRepositorio;
        }
        private IUsuario iUsuario;
        const string SessionLogin = "_Login";

        public IActionResult Index()
        {
            string login = HttpContext.Session.GetString(SessionLogin);
            if (login == null)
                return View(new Usuario());
            else
                return RedirectToAction("Index", "Pizza");
        }

        [HttpPost]
        public async Task<IActionResult> Index(Usuario usuario)
        {
            try
            {
                bool mensaje = await _usuarioR.SeguridadUsuario(usuario);
                ViewBag.mensaje = mensaje;
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = $"Error al insertar el extra en MongoDB: {ex.Message}";
            }
            return RedirectToAction("Index","Home") ;
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove(SessionLogin);

            return RedirectToAction("Index", "Login");
        }
    }
}
