using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PizzaNicola_AspNetCore.Models.Repositorios;
using PizzaNicola_AspNetCore.Models.Entidades;
using Newtonsoft.Json;
using System.Text;

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

        public IActionResult InsertCliente()
        {
            return View(new Cliente());
        }

        [HttpPost]
        public async Task<IActionResult> InsertCliente(Cliente cliente)
        {
            string mensaje = "";

            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:8080");
                StringContent content = new StringContent(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpclient.PostAsync("api/nicola/clientes/cliente", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse.Trim();
            }

            ViewBag.mensaje = mensaje;

            return View(cliente);
        }

        public async Task<Cliente> GetClienteById(string id)
        {
            Cliente cliente = new Cliente();

            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:8080");
                HttpResponseMessage response = await httpclient.GetAsync($"api/nicola/clientes/clienteid/{id}");

                string apiResponse = await response.Content.ReadAsStringAsync();
                cliente = JsonConvert.DeserializeObject<Cliente>(apiResponse);
            }

            return cliente;
        }

        public async Task<IActionResult> UpdateCliente(string id)
        {
            Cliente cliente = await GetClienteById(id);

            return View(await Task.Run(() => cliente));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCliente(Cliente cliente)
        {
            string mensaje = "";
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:8080");
                StringContent content = new StringContent(JsonConvert.SerializeObject(cliente), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpclient.PutAsync("api/nicola/clientes/cliente/" + cliente.id, content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse.Trim();
            }

            ViewBag.mensaje = mensaje;

            return await Task.Run(() => View(cliente));
        }

        public async Task<IActionResult> DeleteCliente(string id)
        {
            try
            {
                string mensaje = "";
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:8080");

                    HttpResponseMessage response = await httpclient.DeleteAsync($"api/nicola/clientes/cliente/{id}");
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    mensaje = apiResponse.Trim();
                }
                ViewBag.mensaje = mensaje;
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = $"Error al eliminar el cliente en MongoDB: {ex.Message}";
            }
            
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SearchCliente(string dni)
        {
            if (dni == null)
            {
                ViewBag.mensaje = "Debe ingresar un DNI";
                return View(new Cliente());
            }

            Cliente cliente = new Cliente();
            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:8080");
                    HttpResponseMessage response = await httpclient.GetAsync($"api/nicola/clientes/cliente/{dni}");

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        cliente = JsonConvert.DeserializeObject<Cliente>(apiResponse);
                    }
                    else
                    {
                        return View("ERROR! T-T");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = $"Error al listar cliente: {ex.Message}";
            }
            return View(cliente);
        }
    }
}
