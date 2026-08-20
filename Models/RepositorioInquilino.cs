using System.Data;
using MySql.Data.MySqlClient;

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
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inquilinos
                    (Nombre, Apellido, Dni, Telefono, Email)
                    Values (@nombre, @apellido, @dni, @telefono, @email);
                    SELECT LAST_INSERT_ID();";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", i.Nombre);
                    command.Parameters.AddWithValue("@apellido", i.Apellido);
                    command.Parameters.AddWithValue("@dni", i.Dni);
                    command.Parameters.AddWithValue("@telefono", i.Telefono);
                    command.Parameters.AddWithValue("@email", i.Email);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.IdInquilino = res;
                    connection.Close(); //TODO: Redundante?
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"DELETE FROM Inquilinos WHERE id_inquilino = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close(); //TODO: Redundante?
                }
            }
            return res;
        }

        public int Modificacion(Inquilino i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"UPDATE Inquilinos
                    SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email
                    WHERE id_inquilino = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", i.Nombre);
                    command.Parameters.AddWithValue("@apellido", i.Apellido);
                    command.Parameters.AddWithValue("@dni", i.Dni);
                    command.Parameters.AddWithValue("@telefono", i.Telefono);
                    command.Parameters.AddWithValue("@email", i.Email);
                    command.Parameters.AddWithValue("@id", i.IdInquilino);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close(); //TODO: Redundante?
                }
            }
            return res;
        }
        public IList<Inquilino> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			IList<Inquilino> res = new List<Inquilino>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT id_inquilino, nombre, apellido, dni, telefono, email
					FROM Inquilinos
					ORDER BY id_inquilino
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
						Inquilino i = new Inquilino
						{
							IdInquilino = reader.GetInt32("id_inquilino"),
							Nombre = reader.GetString("nombre"),
							Apellido = reader.GetString("apellido"),
							Dni = reader.GetInt32("dni"),
							Telefono = reader.GetString("telefono"),
							Email = reader.GetString("email"),
						};
						res.Add(i);
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
					SELECT COUNT(id_inquilino)
					FROM Inquilinos";
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

        public Inquilino? ObtenerPorId(int id)
		{
			Inquilino? i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT id_inquilino, nombre, apellido, dni, telefono, email
					FROM Inquilinos
					WHERE id_inquilino=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inquilino
						{
							IdInquilino = reader.GetInt32("id_inquilino"),
							Nombre = reader.GetString("nombre"),
							Apellido = reader.GetString("apellido"),
							Dni = reader.GetInt32("dni"),
							Telefono = reader.GetString("telefono"),
							Email = reader.GetString("email"),
						};
					}
					connection.Close();
				}
			}
			return i;
		}

        public List<Inquilino> Consultar()
        {
            List<Inquilino> inquilinos = null;
            
            return inquilinos;
        }
    }
}