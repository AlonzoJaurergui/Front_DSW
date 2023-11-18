using System.ComponentModel.DataAnnotations;

namespace PizzaNicola_AspNetCore.Models.Entidades
{
    public class Usuario
    {
        [Display(Name = "ID del Usuario")]
        public int idUsuario { get; set; }
        [Required]
        [Display(Name = "Nombres")]
        public string? nombres { get; set;}
        [Required]
        [Display(Name = "Apellidos")]
        public string? apellidos { get; set; }
        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime fechaNacimiento { get; set; }
        [Required]
        [Display(Name = "DNI")]
        public string? dni { get; set; }
        [Required]
        [Display(Name = "Región")]
        public int idRegion { get; set; }
        [Display(Name = "Región")]
        public string? nombreRegion { get; set; }
        [Required]
        [Display(Name = "Login")]
        public string? login { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string? password { get; set; }
    }
}
