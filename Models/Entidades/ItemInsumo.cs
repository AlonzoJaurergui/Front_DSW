using System.ComponentModel.DataAnnotations;

namespace PizzaNicola_AspNetCore.Models.Entidades
{
    public class ItemInsumo
    {
        [Display(Name = "Código")]
        public int idInsumo { get; set; }
        [Display(Name = "Nombre")]
        public string? nombreInsumo { get; set; }
        [Display(Name = "Descripción")]
        public string? descripcion { get; set; }
        [Display(Name = "Precio")]
        public decimal precio { get; set; }
        [Display(Name = "Cantidad")]
        public int cantidad { get; set; }
        [Display(Name = "Monto")]
        public decimal monto { get { return precio * cantidad; } }
    }
}
