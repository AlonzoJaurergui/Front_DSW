using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace PizzaNicola_AspNetCore.Models.Entidades
{
    public class Usuario
    {
        [Display(Name = "ID")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [Required]
        [Display(Name = "Nombres")]
        public string nombre { get; set;}

        [Required]
        [Display(Name = "Apellidos")]
        public string apellidos { get; set; }

        [Required]
        [Display(Name = "DNI")]
        public string dni { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string login { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string password { get; set; }
    }
}
