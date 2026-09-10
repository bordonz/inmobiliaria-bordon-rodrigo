using System.ComponentModel.DataAnnotations;

namespace inmobiliaria_airbnb.Models
{
    public class TipoInmueble
    {
        [Key]
        [Display(Name = "Nº")]
        public int IdTipoInmueble { get; set; }

        [Required(ErrorMessage = "La descripción es requerida.")]
        public string Descripcion { get; set; } = string.Empty;
    }   
}
