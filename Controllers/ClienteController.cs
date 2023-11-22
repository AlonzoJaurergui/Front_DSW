using Microsoft.AspNetCore.Mvc;
using PizzaNicola_AspNetCore.Models.Repositorios;
using PizzaNicola_AspNetCore.Models.Entidades;
using Newtonsoft.Json;

namespace PizzaNicola_AspNetCore.Controllers
{
    public class ClienteController : Controller
    {
        /*private readonly ClienteRepositorio _clienteR;

        public ClienteController(ClienteRepositorio clienteRepositorio)
        {
            _clienteR = clienteRepositorio;
        }*/

        public async Task<IActionResult> Index()
        {
            List<Cliente> clientes = new List<Cliente>();
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:8080");
                    HttpResponseMessage response = await httpclient.GetAsync("api/nicola/clientes/");

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        clientes = JsonConvert.DeserializeObject<List<Cliente>>(apiResponse)
                        .Select(item => new Cliente()
                        {
                            id = item.id,
                            dni = item.dni,
                            nombre = item.nombre,
                            apellidos = item.apellidos,
                            edad = item.edad,
                            telefono = item.telefono,
                            direccion = item.direccion
                        }).ToList();
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
            return View(clientes);
        }
    }
}
