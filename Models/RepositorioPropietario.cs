namespace inmobiliaria_airbnb.Models
{
    public class RepositorioPropietario : RepositorioBase, IRepositorioPropietario
    {
        public RepositorioPropietario(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Propietario p)
        {
            int res = -1;

            return res;
        }

        public int Baja(int id)
        {
            int res = -1;

            return res;
        }

        public int Modificacion(Propietario p)
        {
            int res = -1;

            return res;
        }

        public Propietario? ObtenerPorEmail(string email)
        {
            Propietario? p = null;

            return p;
        }

        public IList<Propietario> BuscarPorNombre(string nombre)
        {
            List<Propietario> res = new List<Propietario>();

            return res;
        }
    }
}