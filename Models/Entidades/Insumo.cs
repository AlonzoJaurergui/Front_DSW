namespace PizzaNicola_AspNetCore.Models.Entidades
{
    public class Insumo
    {
        public int idInsumo { get; set; }
        public string? nombreInsumo { get; set; }
        public string? descripcion { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
    }
}
