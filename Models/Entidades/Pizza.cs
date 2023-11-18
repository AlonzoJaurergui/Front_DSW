using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace PizzaNicola_AspNetCore.Models.Entidades
{
    public class Pizza
    {
        [Display(Name = "ID")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string? nombrePizza { get; set; }
        [Required]
        [Display(Name = "Ingredientes")]
        public string? ingredientes { get; set; }
        [Display(Name = "Tamaño")]
        public string? tamanio { get; set; }        
        [Display(Name = "Tipo de Masa")]
        public string? tipoMasa { get; set; }        
        [Required]
        [Display(Name = "Precio")]
        public double precio { get; set; }
        [Required]
        [Display(Name = "Stock")]
        public int stock { get; set; }
    }
}
