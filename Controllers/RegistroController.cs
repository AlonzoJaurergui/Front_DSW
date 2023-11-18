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

        public IActionResult Index()
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
            string mensaje = "";

            if(!ModelState.IsValid)
            {
               
                return View(usuario);
            }

            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("https://localhost:7061/api/Usuario/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(usuario),
                                        Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpclient.PostAsync("AddUsuario", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse.Trim();
            }

            ViewBag.mensaje = mensaje;

            
            return View(usuario);
        }
    }
}
