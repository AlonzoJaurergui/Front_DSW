using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;
using PizzaNicola_AspNetCore.Models.Repositorios;
using System.Text;

namespace PizzaNicola_AspNetCore.Controllers
{
    public class PizzaController : Controller
    {
        private readonly PizzaRepositorio _pizzaR;

        public PizzaController(PizzaRepositorio pizzaRepositorio)
        {
            _pizzaR = pizzaRepositorio;
        }
        public async Task<IActionResult> Index()
        {
            List<Pizza> pizzas = new List<Pizza>();
            try {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:8080");
                    HttpResponseMessage response = await httpclient.GetAsync("api/nicola/pizzas/");

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        pizzas = JsonConvert.DeserializeObject<List<Pizza>>(apiResponse)
                        .Select(item => new Pizza()
                        {
                            id = item.id,
                            nombrePizza = item.nombrePizza,
                            ingredientes = item.ingredientes,
                            tamanio = item.tamanio,
                            tipoMasa = item.tipoMasa,
                            precio = item.precio,
                            stock = item.stock
                        }).ToList();
                    }
                    else
                    {
                        return View("ERROR! T-T");
                    }
                }
            }
            catch(Exception ex) { 
                Console.WriteLine(ex.Message);
            }
            
            return View(pizzas);
            
        }
        public IActionResult InsertPizza()
        {
            return View(new Pizza());
        }

        [HttpPost]
        public async Task<IActionResult> InsertPizza(Pizza pizza)
        {
            try
            {
                string mensaje = await _pizzaR.Insertar(pizza);
                ViewBag.mensaje = mensaje;
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = $"Error al insertar la pizza en MongoDB: {ex.Message}";
            }
            return View("InsertPizza", new Pizza());
        }
     
        [HttpPost]
        public async Task<IActionResult> UpdatePizza(Pizza pizza)
        {
            try
            {
                string mensaje = await _pizzaR.Actualizar(pizza);
                ViewBag.mensaje = mensaje;
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = $"Error al actualizar la pizza en MongoDB: {ex.Message}";
            }
            return View(pizza);
        }

        
        public async Task<IActionResult> UpdatePizza(string id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:8080");
                HttpResponseMessage response = await httpClient.GetAsync($"api/nicola/pizzas/pizzaid/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    Pizza pizza = JsonConvert.DeserializeObject<Pizza>(apiResponse);
                    if (pizza == null)
                    {
                        // Manejo si la pizza no se encuentra
                        return View("PizzaNoEncontrada");
                    }
                    return View(pizza);
                }
                else
                {
                    // Manejo del error, puede redirigir a una vista de error o hacer algo más
                    return View("Error");
                }
            }

        }
        
        public async Task<IActionResult> DeletePizza(string id)
        {
            try
            {
                string mensaje = await _pizzaR.Eliminar(id);
                ViewBag.mensaje = mensaje;
            }
            catch(Exception ex)
            {
                ViewBag.mensaje = $"Error al eliminar la pizza en MongoDB: {ex.Message}";
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> BuscarPizzas(string nombre)
        {
            if (nombre == null)
            {
                return RedirectToAction("Index");
            }

            List<Pizza> pizzas = new List<Pizza>();

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:8080");
                    HttpResponseMessage response = await httpclient.GetAsync($"api/nicola/pizzas/pizza/{nombre}");

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        pizzas = JsonConvert.DeserializeObject<List<Pizza>>(apiResponse);
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

            return View("Index", pizzas);
        }

    }
}
