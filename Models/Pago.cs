using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace inmobiliaria_airbnb.Models
{
    public class Pago
    {
        [Key]
        [Display(Name = "Nº")]
        public int IdPago { get; set; }
        [Required]
        public string Concepto { get; set; }
        [Display(Name = "Fecha de pago")]
        [Required]
        public DateTime FechaPago { get; set; }
        [Required]
        public decimal Monto { get; set; }
        [Required]
        public string Estado { get; set; }
        [Display(Name = "Nº Inmueble")]
        [Required]
        public int ReservaId { get; set; }
        [ForeignKey(nameof(ReservaId))]
        [BindNever]
        public Reserva? Reserva { get; set; }
    }
}