namespace inmobiliaria_airbnb.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        //int ModificarPortada(int InmuebleId, string ruta);
        List<Inmueble> ObtenerLista(int paginaNro, int tamPagina);
        int ObtenerCantidad();
        Inmueble? ObtenerPorId(int id);
        int ModificarPortada(int InmuebleId, string ruta);
    }
}