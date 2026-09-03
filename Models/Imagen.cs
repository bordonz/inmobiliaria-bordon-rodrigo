namespace inmobiliaria_airbnb.Models
{
	public class Imagen
	{
		public int IdImagen { get; set; }
		public int InmuebleId { get; set; }
		public string Url { get; set; } = "";
		public IFormFile? Archivo { get; set; } = null;
	}
}