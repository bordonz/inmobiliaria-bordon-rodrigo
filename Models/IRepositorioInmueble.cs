namespace inmobiliaria_airbnb.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        //int ModificarPortada(int InmuebleId, string ruta);
        List<Inmueble> ObtenerLista(int paginaNro, int tamPagina);
        int ObtenerCantidad();
        Inmueble? ObtenerPorId(int id);
        int ModificarPortada(int InmuebleId, string ruta);

        List<Inmueble> BuscarPorPropietario(int id, int paginaNro, int tamPagina);
        int ObtenerCantidadPorPropietario(int idPropietario);

        List<Inmueble> ListarPorDisponibilidad(int paginaNro = 1, int tamPagina = 10);

        List<Inmueble> ListarMasReservados(int paginaNro = 1, int tamPagina = 10);
    }
}