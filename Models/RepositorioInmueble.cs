using System.Data;
using MySql.Data.MySqlClient;
using ZstdSharp.Unsafe;

namespace inmobiliaria_airbnb.Models
{
    public class RepositorioInmueble : RepositorioBase, IRepositorioInmueble
    {
        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {

        }

        public int Alta(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Inmuebles
                    (direccion, cupo, precio_por_dia, porcentaje_reserva, latitud, longitud, tipo, habilitado,propietario_id)
                    VALUES (@direccion, @cupo, @precio_por_dia, @porcentaje_reserva, @latitud, @longitud, @tipo, @propietario_id, @habilitado);
                    SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text; //TODO: Por defecto es .Text osea que es redundante
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@cupo", i.Cupo);
                    command.Parameters.AddWithValue("@precio_por_dia", i.PrecioPorDia);
                    command.Parameters.AddWithValue("@porcentaje_reserva", i.PorcentajeReserva);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@propietario_id", i.PropietarioId);
                    command.Parameters.AddWithValue("@habilitado", i.Habilitado);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.IdInmueble = res;
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = "@DELETE FROM Inmuebles WHERE id_inmueble = @id";
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

        public int Modificacion(Inmueble i)
        {
            int res = -1;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @$"UPDATE Inmuebles
                    SET direccion=@direccion, cupo=@cupo, precio_por_dia=@precio_por_dia, porcentaje_reserva=@porcentaje_reserva,
                        latitud=@latitud, longitud=@longitud, tipo=@tipo, propietario_id=@propietario_id, habilitado=@habilitado
                    WHERE id_inmueble = @id";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@cupo", i.Cupo);
                    command.Parameters.AddWithValue("@precio_por_dia", i.PrecioPorDia);
                    command.Parameters.AddWithValue("@porcentaje_reserva", i.PorcentajeReserva);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@propietario_id", i.PropietarioId);
                    command.Parameters.AddWithValue("@habilitado", i.Habilitado);
                    command.Parameters.AddWithValue("@id", i.IdInmueble);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public List<Inmueble> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
        {
            List<Inmueble> res = new List<Inmueble>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id_inmueble, i.direccion, i.cupo, i.precio_por_dia, i.porcentaje_reserva,
                    i.latitud, i.longitud, i.tipo, i.propietario_id, i.habilitado,
                    p.nombre, p.apellido
                    FROM inmuebles i
                    INNER JOIN propietarios p ON i.propietario_id = p.id_propietario
                    ORDER BY i.id_inmueble
                    LIMIT @tamPagina OFFSET @offset";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("offset", (paginaNro - 1) * tamPagina);
                    connection.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32("id_inmueble"),
                            Direccion = reader.GetString("direccion"),
                            Cupo = reader.GetInt32("cupo"),
                            PrecioPorDia = reader.GetDecimal("precio_por_dia"),
                            PorcentajeReserva = reader.GetDecimal("porcentaje_reserva"),
                            Latitud = reader.GetDecimal("latitud"),
                            Longitud = reader.GetDecimal("longitud"),
                            Tipo = reader.GetString("tipo"),
                            PropietarioId = reader.GetInt32("propietario_id"),
                            duenio = new Propietario
                            {
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido")
                            },
                            Habilitado = reader.GetBoolean("habilitado"),
                        };
                        res.Add(i);
                    }
                }
            }
            return res;
        }

        public int ObtenerCantidad()
        {
            int res = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT COUNT(id_inmueble)
                    FROM Inmuebles";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        res = reader.GetInt32(0);
                    }
                }
            }
            return res;
        }

        public Inmueble? ObtenerPorId(int id)
        {
            Inmueble? i = null;
            using(MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id_inmueble, i.direccion, i.cupo, i.precio_por_dia, i.porcentaje_reserva,
                    i.latitud, i.longitud, i.tipo, i.propietario_id, i.portada, i.habilitado,
                    p.nombre, p.apellido
                    FROM Inmuebles i
                    INNER JOIN propietarios p ON i.propietario_id = p.id_propietario
                    Where i.id_inmueble = @id";
                using(MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32("id_inmueble"),
                            Direccion = reader.GetString("direccion"),
                            Cupo = reader.GetInt32("cupo"),
                            PrecioPorDia = reader.GetDecimal("precio_por_dia"),
                            PorcentajeReserva = reader.GetDecimal("porcentaje_reserva"),
                            Latitud = reader.GetDecimal("latitud"),
                            Longitud = reader.GetDecimal("longitud"),
                            Tipo = reader.GetString("tipo"),
                            PropietarioId = reader.GetInt32("propietario_id"),
                            Portada = reader.IsDBNull(reader.GetOrdinal("portada")) ? null : reader.GetString("portada"),
                            Habilitado = reader.GetBoolean("habilitado"),
                            duenio = new Propietario
                            {
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido")
                            }
                        };
                    }
                }
            }
            return i;
        }
        
        public int ModificarPortada(int id, string url)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"
					UPDATE Inmuebles SET
					Portada=@portada
					WHERE id_inmueble = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@portada", String.IsNullOrEmpty(url) ? DBNull.Value : url);
					command.Parameters.AddWithValue("@id", id);
					command.CommandType = CommandType.Text;
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

        public int ObtenerCantidadPorPropietario(int idPropietario)
        {
            int res = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT COUNT(id_inmueble)
                            FROM Inmuebles
                            WHERE propietario_id=@idPropietario";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@idPropietario", idPropietario);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                }
            }
            return res;
        }

        public List<Inmueble> BuscarPorPropietario(int idPropietario, int paginaNro = 1, int tamPagina = 10)
		{
            List<Inmueble> res = new List<Inmueble>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT i.id_inmueble, i.direccion, i.cupo, i.precio_por_dia, i.porcentaje_reserva,
                    i.latitud, i.longitud, i.tipo, i.propietario_id, i.habilitado,
                    p.nombre, p.apellido
                    FROM inmuebles i
                    INNER JOIN propietarios p ON i.propietario_id = p.id_propietario
                    WHERE i.propietario_id=@idPropietario
                    ORDER BY i.id_inmueble
                    LIMIT @tamPagina OFFSET @offset";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add("@idPropietario", MySqlDbType.Int32).Value = idPropietario;
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("offset", (paginaNro - 1) * tamPagina);
                    connection.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            IdInmueble = reader.GetInt32("id_inmueble"),
                            Direccion = reader.GetString("direccion"),
                            Cupo = reader.GetInt32("cupo"),
                            PrecioPorDia = reader.GetDecimal("precio_por_dia"),
                            PorcentajeReserva = reader.GetDecimal("porcentaje_reserva"),
                            Latitud = reader.GetDecimal("latitud"),
                            Longitud = reader.GetDecimal("longitud"),
                            Tipo = reader.GetString("tipo"),
                            PropietarioId = reader.GetInt32("propietario_id"),
                            duenio = new Propietario
                            {
                                Nombre = reader.GetString("nombre"),
                                Apellido = reader.GetString("apellido")
                            },
                            Habilitado = reader.GetBoolean("habilitado"),
                        };
                        res.Add(i);
                    }
                }
            }
            return res;
		}
    }
}