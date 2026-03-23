using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace agendaSQLite
{
    public class SQLiteDatabase : IDatabase
    {
        private SqliteConnection? connection;
        private string dataBaseName;

        public SQLiteDatabase()
        {
            this.dataBaseName = "";
        }

        public SQLiteDatabase(string dataBaseName)
        {
            this.dataBaseName = dataBaseName;
            string filePath = @"./" + dataBaseName;

            if (File.Exists(filePath))
            {
                OpenConnect();
                CloseConnect();
            }
            else
            {
                CreateDataBase();
                CloseConnect();
            }
        }

        private void CreateDataBase()
        {
            string filePath = @"./crearDBagenda.sql";

            OpenConnect();

            if (File.Exists(filePath))
            {
                string sqlScript = File.ReadAllText(filePath);
                try
                {
                    using var cmd = new SqliteCommand(sqlScript, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine("No es posible leer el archivo de script");
            }

            CloseConnect();
        }

        public void OpenConnect()
        {
            try
            {
                string dataSource = @"Data Source=./" + dataBaseName;
                connection = new SqliteConnection(dataSource);
                connection.Open();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CloseConnect()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }

        public void InsertarContacto(Contacto contacto)
        {
            OpenConnect();
            try
            {
                string query = "INSERT INTO Contactos (nombre, apellido, telefono, direccion, localidad, email, fecha) VALUES (@nombre, @apellido, @telefono, @direccion, @localidad, @email, @fecha);";

                using var cmd = new SqliteCommand(query, connection);

                cmd.Parameters.AddWithValue("@nombre", contacto.Nombre);
                cmd.Parameters.AddWithValue("@apellido", contacto.Apellido);
                cmd.Parameters.AddWithValue("@telefono", contacto.Telefono);
                cmd.Parameters.AddWithValue("@direccion", contacto.Direccion);
                cmd.Parameters.AddWithValue("@localidad", contacto.Localidad);
                cmd.Parameters.AddWithValue("@email", contacto.Email);
                cmd.Parameters.AddWithValue("@fecha", contacto.Fecha?.ToString("dd/MM/yyyy") ?? (object)DBNull.Value);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();
        }

        public List<Contacto> GetAllContactos()
        {
            List<Contacto> contactos = new List<Contacto>();
            OpenConnect();
            try
            {
                string query = "SELECT * From Contactos;";

                using var cmd = new SqliteCommand(query, connection);
                cmd.Prepare();

                SqliteDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Contacto contacto = new Contacto();
                    contacto.Id_contacto = sdr.GetInt32(0);
                    contacto.Nombre = sdr.IsDBNull(1) ? "" : sdr.GetString(1);
                    contacto.Apellido = sdr.IsDBNull(2) ? "" : sdr.GetString(2);
                    contacto.Telefono = sdr.IsDBNull(3) ? "" : sdr.GetString(3);
                    contacto.Direccion = sdr.IsDBNull(4) ? "" : sdr.GetString(4);
                    contacto.Localidad = sdr.IsDBNull(5) ? "" : sdr.GetString(5);
                    contacto.Email = sdr.IsDBNull(6) ? "" : sdr.GetString(6);
                    if (!sdr.IsDBNull(7) && TryParseFecha(sdr.GetValue(7), out DateOnly fecha))
                    {
                        contacto.Fecha = fecha;
                    }

                    contactos.Add(contacto);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();
            return contactos;
        }

        public Contacto? GetContacto(int id)
        {
            Contacto? contacto = null;
            OpenConnect();
            try
            {
                string query = "SELECT * FROM Contactos WHERE id_contactos = @id;";

                using var cmd = new SqliteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                SqliteDataReader sdr = cmd.ExecuteReader();

                if (sdr.Read())
                {
                    contacto = new Contacto();
                    contacto.Id_contacto = sdr.GetInt32(0);
                    contacto.Nombre = sdr.IsDBNull(1) ? "" : sdr.GetString(1);
                    contacto.Apellido = sdr.IsDBNull(2) ? "" : sdr.GetString(2);
                    contacto.Telefono = sdr.IsDBNull(3) ? "" : sdr.GetString(3);
                    contacto.Direccion = sdr.IsDBNull(4) ? "" : sdr.GetString(4);
                    contacto.Localidad = sdr.IsDBNull(5) ? "" : sdr.GetString(5);
                    contacto.Email = sdr.IsDBNull(6) ? "" : sdr.GetString(6);
                    if (!sdr.IsDBNull(7) && TryParseFecha(sdr.GetValue(7), out DateOnly fecha))
                    {
                        contacto.Fecha = fecha;
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();

            return contacto;
        }

        public void DeleteContacto(int id)
        {
            OpenConnect();
            try
            {
                string query = "DELETE FROM Contactos WHERE id_contactos = @id";

                using var cmd = new SqliteCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();
        }

        public List<Contacto> SearchFirtsOrLastName(string name)
        {
            List<Contacto> contactos = new List<Contacto>();
            OpenConnect();
            try
            {
                string query = "SELECT * FROM Contactos WHERE nombre LIKE @name OR apellido LIKE @name";

                using var cmd = new SqliteCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", $"%{name}%");

                cmd.Prepare();
                SqliteDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Contacto contacto = new Contacto();
                    contacto.Id_contacto = sdr.GetInt32(0);
                    contacto.Nombre = sdr.IsDBNull(1) ? "" : sdr.GetString(1);
                    contacto.Apellido = sdr.IsDBNull(2) ? "" : sdr.GetString(2);
                    contacto.Telefono = sdr.IsDBNull(3) ? "" : sdr.GetString(3);
                    contacto.Direccion = sdr.IsDBNull(4) ? "" : sdr.GetString(4);
                    contacto.Localidad = sdr.IsDBNull(5) ? "" : sdr.GetString(5);
                    contacto.Email = sdr.IsDBNull(6) ? "" : sdr.GetString(6);

                    if (!sdr.IsDBNull(7) && TryParseFecha(sdr.GetValue(7), out DateOnly fecha))
                    {
                        contacto.Fecha = fecha;
                    }

                    contactos.Add(contacto);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();

            return contactos;
        }

        public List<Contacto> SearchByLastName(string apellido)
        {
            List<Contacto> contactos = new List<Contacto>();
            OpenConnect();
            try
            {
                string query = "SELECT * FROM Contactos WHERE apellido LIKE @apellido";

                using var cmd = new SqliteCommand(query, connection);
                cmd.Parameters.AddWithValue("@apellido", $"%{apellido}%");
                cmd.Prepare();

                SqliteDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    Contacto contacto = new Contacto();
                    contacto.Id_contacto = sdr.GetInt32(0);
                    contacto.Nombre = sdr.IsDBNull(1) ? "" : sdr.GetString(1);
                    contacto.Apellido = sdr.IsDBNull(2) ? "" : sdr.GetString(2);
                    contacto.Telefono = sdr.IsDBNull(3) ? "" : sdr.GetString(3);
                    contacto.Direccion = sdr.IsDBNull(4) ? "" : sdr.GetString(4);
                    contacto.Localidad = sdr.IsDBNull(5) ? "" : sdr.GetString(5);
                    contacto.Email = sdr.IsDBNull(6) ? "" : sdr.GetString(6);

                    if (!sdr.IsDBNull(7) && TryParseFecha(sdr.GetValue(7), out DateOnly fecha))
                    {
                        contacto.Fecha = fecha;
                    }

                    contactos.Add(contacto);
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();
            return contactos;
        }

        public void UpdateContacto(Contacto contacto)
        {
            OpenConnect();
            try
            {
                string query = "UPDATE Contactos SET nombre = @nombre, apellido = @apellido, telefono = @telefono, direccion = @direccion, localidad = @localidad, email = @email, fecha = @fecha WHERE id_contactos = @id";

                using var cmd = new SqliteCommand(query, connection);

                cmd.Parameters.AddWithValue("@nombre", contacto.Nombre);
                cmd.Parameters.AddWithValue("@apellido", contacto.Apellido);
                cmd.Parameters.AddWithValue("@telefono", contacto.Telefono);
                cmd.Parameters.AddWithValue("@direccion", contacto.Direccion);
                cmd.Parameters.AddWithValue("@localidad", contacto.Localidad);
                cmd.Parameters.AddWithValue("@email", contacto.Email);
                cmd.Parameters.AddWithValue("@fecha", contacto.Fecha?.ToString("dd/MM/yyyy") ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id", contacto.Id_contacto);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();
        }

        private static bool TryParseFecha(object rawFecha, out DateOnly fecha)
        {
            if (rawFecha is DateOnly d)
            {
                fecha = d;
                return true;
            }

            if (rawFecha is DateTime dt)
            {
                fecha = DateOnly.FromDateTime(dt);
                return true;
            }

            string fechaStr = rawFecha?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(fechaStr))
            {
                fecha = default;
                return false;
            }

            string[] formatos = ["dd/MM/yyyy", "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss"];
            return DateOnly.TryParseExact(fechaStr, formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha)
                || DateOnly.TryParse(fechaStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha)
                || DateOnly.TryParse(fechaStr, CultureInfo.CurrentCulture, DateTimeStyles.None, out fecha);
        }
    }
}