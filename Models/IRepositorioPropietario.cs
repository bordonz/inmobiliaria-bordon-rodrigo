namespace inmobiliaria_airbnb.Models
{
    public interface IRepositorioPropietario<Propietario>
    {
        Propietario? Alta(Propietario p);
        Propietario? Baja(int id);
        Propietario? Modificacion(Propietario p);
    }
}