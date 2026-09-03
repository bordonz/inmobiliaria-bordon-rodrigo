namespace inmobiliaria_airbnb.Models
{
	public interface IRepositorioImagen : IRepositorio<Imagen>
	{
		List<Imagen> BuscarPorInmueble(int inmuebleId);
        public Imagen? ObtenerPorId(int id);
	}
}