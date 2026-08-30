using MySql.Data.MySqlClient;

namespace inmobiliaria_airbnb.Models
{
    public class RepositorioReserva : RepositorioBase, IRepositorioReserva
    {
        public RepositorioReserva(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Reserva r)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"INSERT INTO Reservas
                    (id_reserva, estado, monto, fecha_desde, fecha_hasta, inmueble_id, inquilino_id)
                    VALUES (@id_reserva, @estado, @monto, @fecha_desde, @fecha_hasta, @inmueble_id, @inquilino_id);
                    SELECT LAST_INSERT_ID();";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id_reserva", r.IdReserva);
                    command.Parameters.AddWithValue("@estado", r.Estado);
                    command.Parameters.AddWithValue("@monto", r.Monto);
                    command.Parameters.AddWithValue("@fecha_desde", r.FechaDesde);
                    command.Parameters.AddWithValue("@fecha_hasta", r.FechaHasta);
                    command.Parameters.AddWithValue("@inmueble_id", r.InmuebleId);
                    command.Parameters.AddWithValue("@inquilino_id", r.InquilinoId);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    r.IdReserva = res;
                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"DELETE FROM Reservas WHERE id_reserva = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public int Modificacion(Reserva r)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Reservas
                    SET estado=@estado, monto=@monto, fecha_desde=@fecha_desde, fecha_hasta=@fecha_hasta,
                        inmueble_id=@inmueble_id, inquilino_id=@inquilino_id
                    WHERE id_reserva = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@estado", r.Estado);
                    command.Parameters.AddWithValue("@monto", r.Monto);
                    command.Parameters.AddWithValue("@fecha_desde", r.FechaDesde);
                    command.Parameters.AddWithValue("@fecha_hasta", r.FechaHasta);
                    command.Parameters.AddWithValue("@inmueble_id", r.InmuebleId);
                    command.Parameters.AddWithValue("@inquilino_id", r.InquilinoId);
                    command.Parameters.AddWithValue("@id", r.IdReserva);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }

        public List<Reserva> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
        {
            List<Reserva> res = new List<Reserva>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT r.id_reserva, r.estado, r.monto, r.fecha_desde, r.fecha_hasta,
                    r.inmueble_id, r.inquilino_id,
                    p.nombre AS propietario_nombre, p.apellido AS propietario_apellido, 
                    i.nombre AS inquilino_nombre, i.apellido AS inquilino_apellido
                    FROM Reservas r
                    INNER JOIN Inmuebles inm ON r.inmueble_id = inm.id_inmueble
                    INNER JOIN Propietarios p ON inm.propietario_id = p.id_propietario
                    INNER JOIN Inquilinos i ON r.inquilino_id = i.id_inquilino
                    ORDER BY r.id_reserva
                    LIMIT @tamPagina OFFSET @offset";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("@offset", (paginaNro -1) * tamPagina);
                    connection.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Reserva r = new Reserva
                        {
                            IdReserva = reader.GetInt32("id_reserva"),
                            Estado = reader.GetString("estado"),
                            Monto = reader.GetDecimal("monto"),
                            FechaDesde = reader.GetDateTime("fecha_desde"),
                            FechaHasta = reader.GetDateTime("fecha_hasta"),
                            InmuebleId = reader.GetInt32("inmueble_id"),
                            Inmueble = new Inmueble
                            {
                                duenio = new Propietario
                                {
                                    Nombre = reader.GetString("propietario_nombre"),
                                    Apellido = reader.GetString("propietario_apellido")
                                }
                            },
                            InquilinoId = reader.GetInt32("inquilino_id"),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("inquilino_nombre"),
                                Apellido = reader.GetString("inquilino_apellido")
                            }
                        };
                        res.Add(r);
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
                string sql = @"SELECT COUNT(id_reserva)
                    FROM Reservas";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
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

        public Reserva? ObtenerPorId(int id)
        {
            Reserva? r = null;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT r.id_reserva, r.estado, r.monto, r.fecha_desde, r.fecha_hasta,
                    r.inmueble_id, r.inquilino_id,
                    p.nombre AS propietario_nombre, p.apellido AS propietario_apellido, 
                    i.nombre AS inquilino_nombre, i.apellido AS inquilino_apellido
                    FROM Reservas r
                    INNER JOIN Inmuebles inm ON r.inmueble_id = inm.id_inmueble
                    INNER JOIN Propietarios p ON inm.propietario_id = p.id_propietario
                    INNER JOIN Inquilinos i ON r.inquilino_id = i.id_inquilino
                    WHERE r.id_reserva = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    connection.Open();
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        r = new Reserva
                        {
                            IdReserva = reader.GetInt32("id_reserva"),
                            Estado = reader.GetString("estado"),
                            Monto = reader.GetDecimal("monto"),
                            FechaDesde = reader.GetDateTime("fecha_desde"),
                            FechaHasta = reader.GetDateTime("fecha_hasta"),
                            InmuebleId = reader.GetInt32("inmueble_id"),
                            Inmueble = new Inmueble
                            {
                                duenio = new Propietario
                                {
                                    Nombre = reader.GetString("propietario_nombre"),
                                    Apellido = reader.GetString("propietario_apellido")
                                }
                            },
                            InquilinoId = reader.GetInt32("inquilino_id"),
                            Inquilino = new Inquilino
                            {
                                Nombre = reader.GetString("inquilino_nombre"),
                                Apellido = reader.GetString("inquilino_apellido")
                            }
                        };
                    }
                }
            }
            return r;
        }

        public List<Reserva> Consultar()
        {
            List<Reserva> res = new List<Reserva>();

            return res;
        }
    }
}