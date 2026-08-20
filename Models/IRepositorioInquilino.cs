namespace inmobiliaria_airbnb.Models
{
    public interface IRepositorioInquilino : IRepositorio<Inquilino>
    {
        IList<Inquilino> ObtenerLista(int pagina, int tamaño);

        int ObtenerCantidad();

        Inquilino? ObtenerPorId(int id);
    }
}