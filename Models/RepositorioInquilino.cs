namespace inmobiliaria_airbnb.Models
{
    public class RepositorioInquilino : RepositorioBase, IRepositorioInquilino
    {
        public RepositorioInquilino(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Inquilino i)
        {
            int res = -1;

            return res;
        }

        public int Baja(int id)
        {
            int res = -1;

            return res;
        }

        public int Modificacion(Inquilino i)
        {
            int res = -1;

            return res;
        }

        public List<Inquilino> Consultar()
        {
            List<Inquilino> inquilinos = null;
            
            return inquilinos;
        }
    }
}