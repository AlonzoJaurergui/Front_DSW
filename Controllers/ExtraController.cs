using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;
using PizzaNicola_AspNetCore.Models.Repositorios;
using System.Text;

namespace PizzaNicola_AspNetCore.Controllers
{
    public class ExtraController : Controller
    {
        private readonly ExtraRepositorio _extraR;

        public ExtraController(ExtraRepositorio extraRepositorio)
        {
            _extraR = extraRepositorio;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Extra> extras = new List<Extra>();

            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:8080");
                HttpResponseMessage response = await httpclient.GetAsync("api/nicola/extras/");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    extras = JsonConvert.DeserializeObject<List<Extra>>(apiResponse)
                        .Select(item => new Extra()
                        {
                            id = item.id,
                            descripcion = item.descripcion
                        }).ToList();
                }
                else
                {
                    return View("Error");
                }
            }
            return View(extras);
        }

        public IActionResult InsertExtra()
        {
            return View(new Extra());
        }

        [HttpPost]
        public async Task<IActionResult> InsertExtra(Extra extra)
        {
            try 
            { 
                string mensaje = await _extraR.Insertar(extra);
                ViewBag.mensaje = mensaje;
            }
            catch(Exception ex)
            { 
                ViewBag.mensaje = $"Error al insertar el extra en MongoDB: {ex.Message}";
            }
            return View("InsertExtra", new Extra());
        }
    }
}
