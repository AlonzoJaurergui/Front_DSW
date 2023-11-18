using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using PizzaNicola_AspNetCore.Models.Entidades;
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

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_GetInsumos", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    insumos.Add(new Insumo
                    {
                        idInsumo = dr.GetInt32(0),
                        nombreInsumo = dr.GetString(1),
                        descripcion = dr.GetString(2),
                        precio = dr.GetDecimal(3),
                        stock = dr.GetInt32(4)
                    });
                }
            }

            return insumos;
        }

        Insumo Buscar(int idInsumo = 0)
        {
            Insumo insumo = GetInsumos()
                            .Where(item => item.idInsumo == idInsumo)
                            .FirstOrDefault();
            if (insumo == null)
                insumo = new Insumo();

            return insumo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await Task.Run(() => GetInsumos()));
        }

        public async Task<IActionResult> Agregar(int idInsumo)
        {
            return View(await Task.Run(() => Buscar(idInsumo)));
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(int idInsumo, int cantidad)
        {
            Insumo insumo = Buscar(idInsumo);

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

            ItemInsumo? itemAux = canasta.Where(item => item.idInsumo == idInsumo).FirstOrDefault();

            if (itemAux == null)
            {
                ItemInsumo item = new ItemInsumo();
                item.idInsumo = insumo.idInsumo;
                item.nombreInsumo = insumo.nombreInsumo;
                item.descripcion = insumo.descripcion;
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

        public IActionResult Delete(int idInsumo)
        {
            List<ItemInsumo> canasta = JsonConvert.DeserializeObject<List<ItemInsumo>>(HttpContext.Session.GetString(_CANASTA));

            ItemInsumo item = canasta.Where(item => item.idInsumo == idInsumo).FirstOrDefault();
            canasta.Remove(item);

            if(canasta.Count == 0)
                HttpContext.Session.Remove(_CANASTA);
            else
                HttpContext.Session.SetString(_CANASTA, JsonConvert.SerializeObject(canasta));

            return RedirectToAction("Canasta");
        }
    }
}
