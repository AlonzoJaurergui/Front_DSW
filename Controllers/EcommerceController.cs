using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PizzaNicola_AspNetCore.Models.Entidades;
using PizzaNicola_AspNetCore.Models.Interfaces;
using System.Data;
using System.Drawing;

namespace PizzaNicola_AspNetCore.Controllers
{
    public class EcommerceController : Controller
    {
        public readonly IConfiguration _config;
        public string cadena;
        private static readonly string _CANASTA = "canasta";

        public EcommerceController(IConfiguration iConfig)
        {
            _config = iConfig;
            cadena = _config["ConnectionStrings:connection"];
        }

        IEnumerable<Insumo> GetInsumos()
        {
            List<Insumo> insumos = new List<Insumo>();

            try
            {
                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("http://localhost:8080");
                    HttpResponseMessage response = httpclient.GetAsync("api/nicola/insumos/").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = response.Content.ReadAsStringAsync().Result;
                        insumos = JsonConvert.DeserializeObject<List<Insumo>>(apiResponse)
                        .Select(item => new Insumo()
                        {
                            id = item.id,
                            nombreInsumo = item.nombreInsumo,
                            descripcion = item.descripcion,
                            proveedor = item.proveedor,
                            precio = item.precio,
                            stock = item.stock
                        }).ToList();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return insumos;
        }

        Insumo Buscar(string id = "")
        {
            Insumo insumo = GetInsumos()
                            .Where(item => item.id == id)
                            .FirstOrDefault();
            if (insumo == null)
                insumo = new Insumo();

            return insumo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => GetInsumos()));
        }

        public async Task<IActionResult> Agregar(string id)
        {
            return View(await Task.Run(() => Buscar(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(string id, int cantidad)
        {
            Insumo insumo = Buscar(id);

            if (cantidad > insumo.stock)
            {
                ViewBag.mensaje = "El insumo cuenta con "+insumo.stock+" unidades.";
                return View(insumo);
            }

            List<ItemInsumo> canasta = new List<ItemInsumo>();

            if (HttpContext.Session.GetString(_CANASTA) != null)
            {
                canasta = JsonConvert.DeserializeObject<List<ItemInsumo>>(HttpContext.Session.GetString(_CANASTA));
            }

            ItemInsumo? itemAux = canasta.Where(item => item.id == id).FirstOrDefault();

            if (itemAux == null)
            {
                ItemInsumo item = new ItemInsumo();
                item.id = insumo.id;
                item.nombreInsumo = insumo.nombreInsumo;
                item.descripcion = insumo.descripcion;
                item.proveedor = insumo.proveedor;
                item.precio = insumo.precio;
                item.cantidad = cantidad;

                canasta.Add(item);
            }
            else
            {
                itemAux.cantidad = itemAux.cantidad + cantidad;
            }

            HttpContext.Session.SetString(_CANASTA, JsonConvert.SerializeObject(canasta));
            ViewBag.mensaje = "Insumo agregado.";
            return View(insumo);
        }

        public IActionResult Canasta()
        {
            if (HttpContext.Session.GetString(_CANASTA) == null)
                return RedirectToAction("Index");
            IEnumerable<ItemInsumo> canasta = JsonConvert.DeserializeObject<List<ItemInsumo>>(HttpContext.Session.GetString(_CANASTA));
            return View(canasta);
        }

        public IActionResult Delete(string id)
        {
            List<ItemInsumo> canasta = JsonConvert.DeserializeObject<List<ItemInsumo>>(HttpContext.Session.GetString(_CANASTA));

            ItemInsumo item = canasta.Where(item => item.id == id).FirstOrDefault();
            canasta.Remove(item);

            if(canasta.Count == 0)
                HttpContext.Session.Remove(_CANASTA);
            else
                HttpContext.Session.SetString(_CANASTA, JsonConvert.SerializeObject(canasta));

            return RedirectToAction("Canasta");
        }
    }
}
