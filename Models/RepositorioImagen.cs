using System.Data;
using MySql.Data.MySqlClient;
namespace inmobiliaria_airbnb.Models
{
	public class RepositorioImagen : RepositorioBase, IRepositorioImagen
	{
		public RepositorioImagen(IConfiguration configuration) : base(configuration)
		{
		}

		public int Alta(Imagen p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO Imagenes 
					(inmueble_id, url) 
					VALUES (@inmueble_id, @url)";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@inmueble_id", p.InmuebleId);
					command.Parameters.AddWithValue("@url", p.Url);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"DELETE FROM Imagenes WHERE id_imagen = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int Modificacion(Imagen p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"
				UPDATE Imagenes SET 
					Url=@url
				WHERE Id=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", p.IdImagen);
					command.Parameters.AddWithValue("@url", p.Url);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public Imagen? ObtenerPorId(int id)
		{
			Imagen? res = null;
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				string sql = @"
					SELECT id_imagen, inmueble_id, url
					FROM Imagenes
					WHERE id_imagen=@id";
				using (MySqlCommand comm = new MySqlCommand(sql, conn))
				{
					comm.Parameters.AddWithValue("@id", id);
					conn.Open();
					var reader = comm.ExecuteReader();
					if (reader.Read())
					{
						res = new Imagen();
						res.IdImagen = reader.GetInt32("id_imagen");
						res.InmuebleId = reader.GetInt32("inmueble_id");
						res.Url = reader.GetString("url");
					}
					conn.Close();
				}
			}
			return res;
		}

		public List<Imagen> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			List<Imagen> res = new List<Imagen>();
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT 
						{nameof(Imagen.IdImagen)}, 
						{nameof(Imagen.InmuebleId)}, 
						{nameof(Imagen.Url)} 
					FROM Imagenes
					ORDER BY Id
					OFFSET {(paginaNro - 1) * tamPagina} ROW
					FETCH NEXT {tamPagina} ROWS ONLY
				";
				using (MySqlCommand comm = new MySqlCommand(sql, conn))
				{
					conn.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						res.Add(new Imagen
						{
							IdImagen = reader.GetInt32(nameof(Imagen.IdImagen)),
							InmuebleId = reader.GetInt32(nameof(Imagen.InmuebleId)),
							Url = reader.GetString(nameof(Imagen.Url)),
						});
					}
					conn.Close();
				}
			}
			return res;
		}

		public int ObtenerCantidad()
		{
			int res = 0;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT COUNT(Id)
					FROM Imagenes
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					connection.Close();
				}
			}
			return res;
		}

		public List<Imagen> BuscarPorInmueble(int inmuebleId)
		{
			List<Imagen> res = new List<Imagen>();
			using (MySqlConnection conn = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT id_imagen, inmueble_id, url
					FROM Imagenes
					WHERE inmueble_id=@inmuebleId";
				using (MySqlCommand comm = new MySqlCommand(sql, conn))
				{
					comm.Parameters.AddWithValue("@inmuebleId", inmuebleId);
					conn.Open();
					var reader = comm.ExecuteReader();
					while (reader.Read())
					{
						res.Add(new Imagen
						{
							IdImagen = reader.GetInt32("id_imagen"),
							Url = reader.GetString("url"),
							InmuebleId = reader.GetInt32("inmueble_id"),
						});
					}
					conn.Close();
				}
			}
			return res;
		}
	}
}