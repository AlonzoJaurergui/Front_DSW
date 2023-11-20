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

        public async Task<IActionResult> UpdateExtra(string id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:8080");
                HttpResponseMessage response = await httpClient.GetAsync($"api/nicola/extras/extraid/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Extra extra = JsonConvert.DeserializeObject<Extra>(apiResponse);
                    if (extra == null)
                    {
                        // Manejo si el extra no se encuentra
                        return View("Extra NoEncontrada");
                    }
                    return View(extra);
                }
                else
                {
                    // Manejo del error, puede redirigir a una vista de error o hacer algo más
                    return View("Error");
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> UpdateExtra(Extra extra)
        {
            try
            {
                string mensaje = await _extraR.Actualizar(extra);
                ViewBag.mensaje = mensaje;
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = $"Error al actualizar el extra en MongoDB: {ex.Message}";
            }
            return View(extra);
        }
        public async Task<IActionResult> DeleteExtra(string id)
        {
            try
            {
                string mensaje = await _extraR.Eliminar(id);
                ViewBag.mensaje = mensaje;
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = $"Error al eliminar el extra en MongoDB: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> BuscarExtras(string descripcion)
        {
            List<Extra> extras = new List<Extra>();

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:8080");
                    HttpResponseMessage response = await httpclient.GetAsync($"api/nicola/extras/extra/{descripcion}");

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        extras = JsonConvert.DeserializeObject<List<Extra>>(apiResponse);
                    }
                    else
                    {
                        return View("ERROR! T-T");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View("Index", extras);
        }
    }
}
