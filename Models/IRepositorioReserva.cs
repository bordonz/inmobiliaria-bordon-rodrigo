namespace inmobiliaria_airbnb.Models
{
    public interface IRepositorioReserva : IRepositorio<Reserva>
    {
        List<Reserva> ObtenerLista(int paginaNro, int tamPagina);
        int ObtenerCantidad();
        Reserva? ObtenerPorId(int id);

        List<Reserva> FiltrarPorFecha(DateTime fechaDesde, DateTime fechaHasta, int paginaNro, int tamPagina);
    }
}