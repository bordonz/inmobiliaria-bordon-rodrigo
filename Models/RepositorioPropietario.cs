using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

//TODO: CORREGIR COMENTARIOS
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
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Propietarios
                    (Nombre, Apellido, Dni, Telefono, Email, Clave)
                    Values (@nombre, @apellido, @dni, @telefono, @email, @clave);
                    SELECT LAST_INSERT_ID();";//devuelve el id insertado (LAST_INSERT_ID para mysql)
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);
                    command.Parameters.AddWithValue("@email", p.Email);
                    command.Parameters.AddWithValue("@clave", p.Clave);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    p.IdPropietario = res;
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
                string sql = @"DELETE FROM Propietarios WHERE id_propietario = @id";
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

        public int Modificacion(Propietario p)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"UPDATE Propietarios
                    SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email, Clave=@clave
                    WHERE id_propietario = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@nombre", p.Nombre);
                    command.Parameters.AddWithValue("@apellido", p.Apellido);
                    command.Parameters.AddWithValue("@dni", p.Dni);
                    command.Parameters.AddWithValue("@telefono", p.Telefono);
                    command.Parameters.AddWithValue("@email", p.Email);
                    command.Parameters.AddWithValue("@clave", p.Clave);
                    command.Parameters.AddWithValue("@id", p.IdPropietario);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close(); //TODO: Redundante?
                }
            }
            return res;
        }

        public List<Propietario> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			List<Propietario> res = new List<Propietario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT id_propietario, nombre, apellido, dni, telefono, email, clave
					FROM Propietarios
					ORDER BY id_propietario
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
						Propietario p = new Propietario
						{
							IdPropietario = reader.GetInt32("id_propietario"),
							Nombre = reader.GetString("nombre"),
							Apellido = reader.GetString("apellido"),
							Dni = reader.GetString("dni"),
							Telefono = reader.GetString("telefono"),
							Email = reader.GetString("email"),
							Clave = reader.GetString("clave"),
						};
						res.Add(p);
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
					SELECT COUNT(id_propietario)
					FROM Propietarios";
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

        public Propietario? ObtenerPorId(int id)
		{
			Propietario? p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT id_propietario, nombre, apellido, dni, telefono, email, clave 
					FROM Propietarios
					WHERE id_propietario=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Propietario
						{
							IdPropietario = reader.GetInt32("id_propietario"),
							Nombre = reader.GetString("nombre"),
							Apellido = reader.GetString("apellido"),
							Dni = reader.GetString("dni"),
							Telefono = reader.GetString("telefono"),
							Email = reader.GetString("email"),
							Clave = reader.GetString("clave"),
						};
					}
					connection.Close();
				}
			}
			return p;
		}

        public Propietario? ObtenerPorEmail(string email)
        {
            Propietario? p = null;

            return p;
        }

        public List<Propietario> BuscarPorNombre(string nombre)
        {
            List<Propietario> res = new List<Propietario>();
            nombre = "%" + nombre + "%"; //TODO: Optimizar
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT id_propietario, nombre, apellido, dni, telefono, email, clave
                    FROM Propietarios
                    WHERE nombre LIKE @nombre OR apellido LIKE @nombre";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nombre;;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var p = new Propietario
                        {    
                            IdPropietario = reader.GetInt32("id_propietario"),
                            Nombre = reader.GetString("nombre"),
                            Apellido = reader.GetString("apellido"),
                            Dni = reader.GetString("dni"),
                            Telefono = reader.GetString("telefono"),
                            Email = reader.GetString("email"),
                            Clave = reader.GetString("clave"),
                        };
                        res.Add(p);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public string Hashear(string clave)
        {
            clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: clave,
                salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 1000,
                numBytesRequested: 256 / 8));
            return clave;
        }

        public int ValidarClave(int IdPropietario, string claveNueva, string claveRepeticion)
        {
            int res = -1;
            if (string.IsNullOrWhiteSpace(claveNueva) || string.IsNullOrWhiteSpace(claveRepeticion))
            {
                return -1;
            }
            
            var claveN = Hashear(claveNueva);
            var claveR = Hashear(claveRepeticion);

            if(claveN != claveR)
            {
                return -1;
            }

            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Propietarios
                    SET Clave=@clave
                    WHERE id_propietario = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@clave", claveN);
                    command.Parameters.AddWithValue("@id", IdPropietario);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;

        }
    }
}