namespace inmobiliaria_airbnb.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        Propietario? ObtenerPorEmail(String email);
        IList<Propietario> BuscarPorNombre(string nombre);
    }
}