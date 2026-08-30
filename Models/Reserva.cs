using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace inmobiliaria_airbnb.Models
{
    public class Reserva
    {
        [Key]
        [Display(Name = "Nº")]
        public int IdReserva { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public decimal Monto { get; set; }
        [Display(Name = "Reserva desde")]
        [Required]
        public DateTime FechaDesde { get; set; }
        [Display(Name = "Reserva hasta")]
        [Required]
        public DateTime FechaHasta { get; set; }
        public int InmuebleId { get; set; }
        [ForeignKey(nameof(InmuebleId))]
        [BindNever]
        public Inmueble? Inmueble { get; set; }
        public int InquilinoId { get; set; }
        [ForeignKey(nameof(InquilinoId))]
        [BindNever]
        public Inquilino? Inquilino { get; set; }
    }
}