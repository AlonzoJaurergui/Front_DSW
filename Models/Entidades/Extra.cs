using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace PizzaNicola_AspNetCore.Models.Entidades
{
    public class Extra
    {
        [Display(Name = "Cód.")]
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public String id { get; set; }

        [Required]
        [Display(Name = "Extra")]
        public string descripcion { get; set; }
    }
}
