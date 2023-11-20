using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;
using PizzaNicola_AspNetCore.Models.Repositorios;
using System.Text;

namespace PizzaNicola_AspNetCore.Controllers
{
    public class RegistroController : Controller
    {
        private readonly UsuarioRepositorio _usuRepo;
        public RegistroController( UsuarioRepositorio usuarioRepositorio) 
        {
            _usuRepo = usuarioRepositorio;
        }

        public async Task<IActionResult>  Index()
        {
            return View();
        }

        public async Task<IActionResult> RegisterUsuario()
        {
            
            return View(new Usuario());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUsuario(Usuario usuario)
        {
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:8080");
                    HttpResponseMessage response = await httpclient.PostAsJsonAsync("api/nicola/usuarios/usuario",usuario);
                    if (response.IsSuccessStatusCode)
                    {
                        string mensaje = await _usuRepo.Registrar(usuario);
                        ViewBag.mensaje = mensaje;
                    }
                    else
                    {
                        return View("Error prro lo siento");
                    }
                }
                
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = $"Error al insertar la pizza en MongoDB: {ex.Message}";
            }            
            return View("RegisterUsuario",usuario);
        }
    }
}
