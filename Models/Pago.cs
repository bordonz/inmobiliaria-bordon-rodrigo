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
        [Display(Name = "Importe")]
        public decimal Monto { get; set; }
        [Required]
        public string Estado { get; set; }
        [Display(Name = "Nº Reserva")]
        [Required]
        public int ReservaId { get; set; }
        [ForeignKey(nameof(ReservaId))]
        [BindNever]
        public Reserva? Reserva { get; set; }

        public int? IdUsuarioCreador { get; set; }
        public int? IdUsuarioAnulador { get; set; }

        public Usuario UsuarioCreador { get; set; }
        public Usuario? UsuarioAnulador { get; set; }
    }
}