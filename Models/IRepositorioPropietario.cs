namespace inmobiliaria_airbnb.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        Propietario? ObtenerPorEmail(String email);
        List<Propietario> BuscarPorNombre(string nombre);

        List<Propietario> ObtenerLista(int pagina, int tamaño);

        int ObtenerCantidad();

        Propietario? ObtenerPorId(int id);
    }
}