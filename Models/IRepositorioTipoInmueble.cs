namespace inmobiliaria_airbnb.Models
{
    public interface IRepositorioTiposInmuebles : IRepositorio<TipoInmueble>
    {
        List<TipoInmueble> ObtenerLista(int pagina, int tamaño);
        int ObtenerCantidad();
        TipoInmueble? ObtenerPorId(int id);
    }
}