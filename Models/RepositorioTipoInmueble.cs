using System.Data;
using MySql.Data.MySqlClient;

namespace inmobiliaria_airbnb.Models
{
    public class RepositorioTipoInmueble : RepositorioBase, IRepositorioTiposInmuebles
    {
        public RepositorioTipoInmueble(IConfiguration configuration) : base(configuration)
        {
            
        }

        public int Alta(TipoInmueble t)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO tipos_inmueble
                    (descripcion)
                    Values (@descripcion);
                    SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@descripcion", t.Descripcion);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    t.IdTipoInmueble = res;
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM tipos_inmueble WHERE id_tipo_inmueble = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Modificacion(TipoInmueble t)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE tipos_inmueble
                    SET Descripcion=@descripcion
                    WHERE id_tipo_inmueble = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@descripcion", t.Descripcion);
                    command.Parameters.AddWithValue("@id", t.IdTipoInmueble);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public List<TipoInmueble> ObtenerLista(int paginaNro, int tamPagina)
		{
			List<TipoInmueble> res = new List<TipoInmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT id_tipo_inmueble, descripcion
					FROM tipos_inmueble
					ORDER BY id_tipo_inmueble
					LIMIT @tamPagina OFFSET @offset";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (paginaNro - 1) * tamPagina);
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						TipoInmueble t = new TipoInmueble
						{
							IdTipoInmueble = reader.GetInt32("id_tipo_inmueble"),
							Descripcion= reader.GetString("descripcion")
						};
						res.Add(t);
					}
					connection.Close();
				}
			}
			return res;
		}

        public int ObtenerCantidad()
		{
			int res = 0;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"
					SELECT COUNT(id_tipo_inmueble)
					FROM tipos_inmueble";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						res = reader.GetInt32(0);
					}
					connection.Close();
				}
			}
			return res;
		}

        public TipoInmueble? ObtenerPorId(int id)
		{
			TipoInmueble? t = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT id_tipo_inmueble, descripcion
					FROM tipos_inmueble
					WHERE id_tipo_inmueble=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						t = new TipoInmueble
						{
							IdTipoInmueble = reader.GetInt32("id_tipo_inmueble"),
							Descripcion = reader.GetString("descripcion"),
						};
					}
					connection.Close();
				}
			}
			return t;
		}
    }
}