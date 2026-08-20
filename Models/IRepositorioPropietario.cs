namespace inmobiliaria_airbnb.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        Propietario? ObtenerPorEmail(String email);
        IList<Propietario> BuscarPorNombre(string nombre);

        IList<Propietario> ObtenerLista(int pagina, int tamaño);

        int ObtenerCantidad();

        Propietario? ObtenerPorId(int id);
    }
}