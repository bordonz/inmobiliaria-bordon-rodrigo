using MySql.Data.MySqlClient;

namespace inmobiliaria_airbnb.Models
{
    public class RepositorioPago: RepositorioBase, IRepositorioPago
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {
            
        }
            public int Alta(Pago p)
            {
                int res = -1;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string sql = @"INSERT INTO Pagos
                        (id_pago, concepto, fecha_pago, monto, estado, reserva_id)
                        VALUES (@id_pago, @concepto, @fecha_pago, @monto, estado, @reserva_id);
                        SELECT LAST_INSERT_ID();";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id_pago", p.IdPago);
                        command.Parameters.AddWithValue("@concepto", p.Concepto);
                        command.Parameters.AddWithValue("@fecha_pago", p.FechaPago);
                        command.Parameters.AddWithValue("@monto", p.Monto);
                        command.Parameters.AddWithValue("@estado", p.Estado);
                        command.Parameters.AddWithValue("@reserva_id", p.ReservaId);
                        connection.Open();
                        res = Convert.ToInt32(command.ExecuteScalar());
                        p.IdPago = res;
                    }
                }
                return res;
            }

            public int Baja(int id)
            {
                int res = -1;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string estado = "Anulado";
                    string sql = @"UPDATE Pagos
                        SET estado=@estado 
                        WHERE id_pago = @id";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@estado", estado);
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();
                        res = command.ExecuteNonQuery();
                    }
                }
                return res;
            }

            public int Modificacion(Pago p)
            {
                int res = -1;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string sql = @"UPDATE Pagos
                        SET concepto=@concepto
                        WHERE id_pago = @id";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@concepto", p.Concepto);
                        connection.Open();
                        res = command.ExecuteNonQuery();
                    }
                }
                return res;
            }

            public List<Pago> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
            {
                List<Pago> res = new List<Pago>();
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string sql = @"SELECT p.*
                        FROM Pagos p
                        ORDER BY p.id_pago
                        LIMIT @tamPagina OFFSET @offset";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@tamPagina", tamPagina);
                        command.Parameters.AddWithValue("@offset", (paginaNro -1) * tamPagina);
                        connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            Pago p = new Pago
                            {
                                IdPago = reader.GetInt32("id_pago"),
                                Concepto = reader.GetString("concepto"),
                                FechaPago = reader.GetDateTime("fecha_pago"),
                                Monto = reader.GetDecimal("monto"),
                                Estado = reader.GetString("estado"),
                                ReservaId = reader.GetInt32("reserva_id")
                            };
                            res.Add(p);
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
                    string sql = @"SELECT COUNT(id_pago)
                        FROM Pagos";
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

            public Pago? ObtenerPorId(int id)
            {
                Pago? p = null;
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string sql = @"SELECT p.*
                        FROM Pagos p
                        WHERE p.id_pago = @id";
                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                        connection.Open();
                        var reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            p = new Pago
                            {
                                IdPago = reader.GetInt32("id_pago"),
                                Concepto = reader.GetString("concepto"),
                                FechaPago = reader.GetDateTime("fecha_pago"),
                                Monto = reader.GetDecimal("monto"),
                                Estado = reader.GetString("estado"),
                                ReservaId = reader.GetInt32("reserva_id")
                            };
                        }
                    }
                }
                return p;
            }
        }
}