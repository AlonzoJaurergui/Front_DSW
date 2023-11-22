using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace PizzaNicola_AspNetCore.Models.Entidades
{
    public class Cliente
    {
        [Display(Name = "ID")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [Display(Name = "DNI")]
        public string? dni { get; set; }
        [Display(Name = "Nombre")]
        public string? nombre { get; set; }
        [Display(Name = "Apellidos")]
        public string? apellidos { get; set; }
        [Display(Name = "Edad")]
        public int edad {  get; set; }
        [Display(Name = "Teléfono")]
        public string? telefono { get; set; }
        [Display(Name = "Dirección")]
        public string? direccion { get; set; }

    }
}
