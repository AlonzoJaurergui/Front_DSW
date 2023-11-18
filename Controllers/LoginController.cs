using Microsoft.AspNetCore.Mvc;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;
using PizzaNicola_AspNetCore.Models.Repositorios;
using Microsoft.AspNetCore.Http;

namespace PizzaNicola_AspNetCore.Controllers
{
    public class LoginController : Controller
    {
        private IUsuario iUsuario;
        const string SessionLogin = "_Login";

        public LoginController()
        {
            iUsuario = new UsuarioRepositorio();
        }

        public IActionResult Index()
        {
            string? login = HttpContext.Session.GetString(SessionLogin);
            if (login == null)
                return View(new Usuario());
            else
                return RedirectToAction("Index", "Pizza");
        }

        [HttpPost]
        public IActionResult Index(Usuario usuario)
        {
            if(string.IsNullOrEmpty(usuario.login) || string.IsNullOrEmpty(usuario.password))
            {
                ModelState.AddModelError("", "Por favor, ingrese los datos solicitados.");
            }
            else
            {
                bool existeUsuario = iUsuario.SeguridadUsuario(usuario);
                if (existeUsuario)
                {
                    HttpContext.Session.SetString(SessionLogin, usuario.login);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Los datos ingresados son incorrectos o inválidos.");
                }
            }

            return View(usuario);
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove(SessionLogin);

            return RedirectToAction("Index", "Login");
        }
    }
}
