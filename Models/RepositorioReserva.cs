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
            if (r.FechaDesde >= r.FechaHasta)
                throw new Exception("La fecha de inicio debe ser anterior a la fecha de fin.");

            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string checkSql = @"SELECT COUNT(*) 
                    FROM Reservas
                    WHERE inmueble_id = @inmueble_id
                    AND estado = 'Confirmada'
                    AND (fecha_desde <= @fecha_desde AND fecha_hasta >= @fecha_hasta');
";
                using (MySqlCommand checkCommand = new MySqlCommand(checkSql, connection))
                {
                    checkCommand.Parameters.AddWithValue("@inmueble_id", r.InmuebleId);
                    checkCommand.Parameters.AddWithValue("@fecha_desde", r.FechaDesde);
                    checkCommand.Parameters.AddWithValue("@fecha_hasta", r.FechaHasta);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        throw new Exception("El inmueble ya está reservado en esas fechas.");
                    }
                }

                string sql = @"INSERT INTO Reservas
                    (estado, monto, fecha_desde, fecha_hasta, inmueble_id, inquilino_id, id_usuario_creador)
                    VALUES (@estado, @monto, @fecha_desde, @fecha_hasta, @inmueble_id, @inquilino_id, @id_usuario_creador);
                    SELECT LAST_INSERT_ID();";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@estado", r.Estado);
                    command.Parameters.AddWithValue("@monto", r.Monto);
                    command.Parameters.AddWithValue("@fecha_desde", r.FechaDesde);
                    command.Parameters.AddWithValue("@fecha_hasta", r.FechaHasta);
                    command.Parameters.AddWithValue("@inmueble_id", r.InmuebleId);
                    command.Parameters.AddWithValue("@inquilino_id", r.InquilinoId);
                    command.Parameters.AddWithValue("@id_usuario_creador", r.IdUsuarioCreador);

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
                        inmueble_id=@inmueble_id, inquilino_id=@inquilino_id, id_usuario_finalizador=@id_usuario_finalizador
                    WHERE id_reserva = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@estado", r.Estado);
                    command.Parameters.AddWithValue("@monto", r.Monto);
                    command.Parameters.AddWithValue("@fecha_desde", r.FechaDesde);
                    command.Parameters.AddWithValue("@fecha_hasta", r.FechaHasta);
                    command.Parameters.AddWithValue("@inmueble_id", r.InmuebleId);
                    command.Parameters.AddWithValue("@inquilino_id", r.InquilinoId);
                    command.Parameters.AddWithValue("@id_usuario_finalizador", r.IdUsuarioFinalizador);
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
                string sql = @"SELECT r.id_reserva, r.estado, r.monto, r.fecha_desde, r.fecha_hasta, r.fecha_anticipada,
                    r.inmueble_id, r.inquilino_id,
                    p.nombre AS propietario_nombre, p.apellido AS propietario_apellido, 
                    i.nombre AS inquilino_nombre, i.apellido AS inquilino_apellido, pa.id_pago,
                    IFNULL(pa.concepto, 'Sin concepto') AS concepto,
                    IFNULL(pa.fecha_pago, '1970-01-01') AS fecha_pago,
                    IFNULL(pa.monto, 0) AS monto_pago
                    FROM Reservas r
                    INNER JOIN Inmuebles inm ON r.inmueble_id = inm.id_inmueble
                    INNER JOIN Propietarios p ON inm.propietario_id = p.id_propietario
                    INNER JOIN Inquilinos i ON r.inquilino_id = i.id_inquilino
                    LEFT JOIN Pagos pa ON r.pago_id = pa.id_pago
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
                            FechaAnticipada = reader.IsDBNull(reader.GetOrdinal("fecha_anticipada"))
                            ? (DateTime?)null
                            : reader.GetDateTime("fecha_anticipada"),
                            InmuebleId = reader.GetInt32("inmueble_id"),
                            Inmueble = new Inmueble
                            {
                                Duenio = new Propietario
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
                            },
                            Pago = reader.IsDBNull(reader.GetOrdinal("id_pago"))
                            ? null
                            : new Pago
                            {
                                Concepto = reader.GetString("concepto"),
                                FechaPago = reader.GetDateTime("fecha_pago"),
                                Monto = reader.GetDecimal("monto_pago")
                            }
                        };
                        res.Add(r);
                    }
                }
            }
            return res;
        }

        public List<Reserva> ObtenerPagos(int id, int paginaNro = 1, int tamPagina = 10)
        {
            List<Reserva> res = new List<Reserva>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT r.id_reserva, r.estado, r.monto, r.fecha_desde, r.fecha_hasta,
                    r.fecha_anticipada, r.inmueble_id, r.inquilino_id,
                    p.nombre AS propietario_nombre, p.apellido AS propietario_apellido, 
                    i.nombre AS inquilino_nombre, i.apellido AS inquilino_apellido, pa.id_pago,
                    IFNULL(pa.concepto, 'Sin concepto') AS concepto,
                    IFNULL(pa.fecha_pago, '1970-01-01') AS fecha_pago,
                    IFNULL(pa.monto, 0) AS monto_pago
                    FROM Reservas r
                    INNER JOIN Inmuebles inm ON r.inmueble_id = inm.id_inmueble
                    INNER JOIN Propietarios p ON inm.propietario_id = p.id_propietario
                    INNER JOIN Inquilinos i ON r.inquilino_id = i.id_inquilino
                    LEFT JOIN Pagos pa ON pa.reserva_id = r.id_reserva
                    WHERE r.id_reserva = @id
                    ORDER BY pa.id_pago
                    LIMIT @tamPagina OFFSET @offset";
;
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
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
                            FechaAnticipada = reader.IsDBNull(reader.GetOrdinal("fecha_anticipada"))
                            ? (DateTime?)null
                            : reader.GetDateTime("fecha_anticipada"),
                            InmuebleId = reader.GetInt32("inmueble_id"),
                            Inmueble = new Inmueble
                            {
                                Duenio = new Propietario
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
                            },
                            Pago = new Pago
                            {
                                Concepto = reader.GetString("concepto"),
                                FechaPago = reader.GetDateTime("fecha_pago"),
                                Monto = reader.GetDecimal("monto_pago")
                            }
                        };
                        res.Add(r);
                    }
                }
            }
            return res;
        }

        public int ObtenerCantidadPagos(int id)
        {
            int res = 0;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT COUNT(id_reserva)
                    FROM Reservas
                    WHERE id_reserva = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
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
                string sql = @"SELECT r.id_reserva, r.estado, r.monto, r.fecha_desde, r.fecha_hasta, r.fecha_anticipada,
                    r.inmueble_id, r.inquilino_id, r.id_usuario_creador, r.id_usuario_finalizador,
                    p.nombre AS propietario_nombre, p.apellido AS propietario_apellido, 
                    i.nombre AS inquilino_nombre, i.apellido AS inquilino_apellido, pa.concepto,
                    pa.fecha_pago, pa.monto
                    FROM Reservas r
                    INNER JOIN Inmuebles inm ON r.inmueble_id = inm.id_inmueble
                    INNER JOIN Propietarios p ON inm.propietario_id = p.id_propietario
                    INNER JOIN Inquilinos i ON r.inquilino_id = i.id_inquilino
                    LEFT JOIN Pagos pa ON r.pago_id = pa.id_pago
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
                            FechaAnticipada = reader.IsDBNull(reader.GetOrdinal("fecha_anticipada"))
                            ? (DateTime?)null
                            : reader.GetDateTime("fecha_anticipada"),
                            InmuebleId = reader.GetInt32("inmueble_id"),
                            IdUsuarioCreador = reader["id_usuario_creador"] == DBNull.Value 
                                ? null : (int?)Convert.ToInt32(reader["id_usuario_creador"]),
                            IdUsuarioFinalizador = reader["id_usuario_finalizador"] == DBNull.Value 
                            ? null 
                            : (int?)Convert.ToInt32(reader["id_usuario_finalizador"]),
                            Inmueble = new Inmueble
                            {
                                Duenio = new Propietario
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
                            },
                            Pago = new Pago
                            {
                                Concepto = reader.IsDBNull(reader.GetOrdinal("concepto")) 
                                    ? "Sin concepto" 
                                    : reader.GetString("concepto"),
                                FechaPago = reader.IsDBNull(reader.GetOrdinal("fecha_pago")) 
                                    ? DateTime.MinValue 
                                    : reader.GetDateTime("fecha_pago"),
                                Monto = reader.IsDBNull(reader.GetOrdinal("monto")) 
                                    ? 0 
                                    : reader.GetDecimal("monto")
                            }
                        };
                    }
                }
            }
            return r;
        }

        public List<Reserva> FiltrarPorFecha(DateTime fechaDesde, DateTime fechaHasta, int paginaNro, int tamPagina)
        {
            List<Reserva> res = new List<Reserva>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"SELECT r.id_reserva, r.estado, r.monto, r.fecha_desde, r.fecha_hasta, r.fecha_anticipada,
                    r.inmueble_id, r.inquilino_id,
                    p.nombre AS propietario_nombre, p.apellido AS propietario_apellido, 
                    i.nombre AS inquilino_nombre, i.apellido AS inquilino_apellido, pa.id_pago, pa.concepto,
                    pa.fecha_pago, pa.monto
                    FROM Reservas r
                    INNER JOIN Inmuebles inm ON r.inmueble_id = inm.id_inmueble
                    INNER JOIN Propietarios p ON inm.propietario_id = p.id_propietario
                    INNER JOIN Inquilinos i ON r.inquilino_id = i.id_inquilino
                    LEFT JOIN Pagos pa ON r.pago_id = pa.id_pago
                    WHERE r.fecha_desde >= @fechaDesde
                    AND r.fecha_hasta <= @fechaHasta
                    AND r.estado = 'Confirmada'
                    ORDER BY r.id_reserva
                    LIMIT @tamPagina OFFSET @offset;";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    command.Parameters.AddWithValue("@fechaHasta", fechaHasta);
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
                            FechaAnticipada = reader.IsDBNull(reader.GetOrdinal("fecha_anticipada"))
                            ? (DateTime?)null
                            : reader.GetDateTime("fecha_anticipada"),
                            InmuebleId = reader.GetInt32("inmueble_id"),
                            Inmueble = new Inmueble
                            {
                                Duenio = new Propietario
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
                            },
                            Pago = new Pago
                            {
                                Concepto = reader.GetString("concepto"),
                                FechaPago = reader.GetDateTime("fecha_pago"),
                                Monto = reader.GetDecimal("monto")
                            }
                        };
                        res.Add(r);
                    }
                }
            }
            return res;
        }

        public List<Reserva> SinReservasEn(int dias, int paginaNro, int tamPagina)
        {
            List<Reserva> res = new List<Reserva>();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                var fechaLimite = DateTime.Today.AddDays(dias);
                string sql = @"SELECT r.id_reserva, r.estado, r.monto, r.fecha_desde, r.fecha_hasta, r.fecha_anticipada,
                    r.inmueble_id, r.inquilino_id,
                    p.nombre AS propietario_nombre, p.apellido AS propietario_apellido, 
                    i.nombre AS inquilino_nombre, i.apellido AS inquilino_apellido, pa.id_pago, pa.concepto,
                    pa.fecha_pago, pa.monto
                    FROM Reservas r
                    INNER JOIN Inmuebles inm ON r.inmueble_id = inm.id_inmueble
                    INNER JOIN Propietarios p ON inm.propietario_id = p.id_propietario
                    INNER JOIN Inquilinos i ON r.inquilino_id = i.id_inquilino
                    LEFT JOIN Pagos pa ON r.pago_id = pa.id_pago
                    WHERE r.fecha_hasta BETWEEN CURDATE() AND @fechaLimite
                    AND r.estado = 'Confirmada'
                    ORDER BY r.id_reserva
                    LIMIT @tamPagina OFFSET @offset;";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@fechaLimite", fechaLimite);
                    command.Parameters.AddWithValue("@tamPagina", tamPagina);
                    command.Parameters.AddWithValue("offset", (paginaNro - 1) * tamPagina);

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
                            FechaAnticipada = reader.IsDBNull(reader.GetOrdinal("fecha_anticipada"))
                            ? (DateTime?)null
                            : reader.GetDateTime("fecha_anticipada"),
                            InmuebleId = reader.GetInt32("inmueble_id"),
                            Inmueble = new Inmueble
                            {
                                Duenio = new Propietario
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
                            },
                            Pago = new Pago
                            {
                                Concepto = reader.GetString("concepto"),
                                FechaPago = reader.GetDateTime("fecha_pago"),
                                Monto = reader.GetDecimal("monto")
                            }
                        };
                        res.Add(r);
                    }
                }
            }
            return res;
        }

        public int EditFechaAnticipada(Reserva r)
        {
            int res = -1;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string sql = @"UPDATE Reservas
                    SET fecha_anticipada=@FechaAnticipada
                    WHERE id_reserva = @id";
                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FechaAnticipada", r.FechaAnticipada);
                    command.Parameters.AddWithValue("@id", r.IdReserva);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
            }
            return res;
        }
    }
}