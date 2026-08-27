using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace inmobiliaria_airbnb.Models
{
    public class Inmueble
    {
        [Key]
        [Display(Name = "Nº")]
        public int IdInmueble { get; set; }
        [Display(Name = "Direccion")]
        [Required(ErrorMessage = "La dirección es requerida")]
        public string? Direccion { get; set; }
        [Required]
        public int Cupo { get; set; }
        [Required]
        public decimal PrecioPorDia { get; set; }
        [Required]
        public decimal PorcentajeReserva  { get; set; }
		public decimal Latitud { get; set; }
		public decimal Longitud { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Display(Name = "Dueño")]
        public int PropietarioId { get; set; }
        [ForeignKey(nameof(PropietarioId))]
        [BindNever]
        public Propietario? duenio { get; set; }
        public bool Habilitado { get; set; } = true;
    }
}