using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace agendaSQLite
{
    public class MySQLDatabase : IDatabase
    {
        private MySqlConnection? connection;
        private string dataBaseName;

        public MySQLDatabase(string dataBaseName)
        {
            this.dataBaseName = dataBaseName;

            /*

            if(File.Exists(filePath))
            {
                OpenConnect();
                CloseConnect();
            }
            else
            {
                CreateDataBase();
                CloseConnect();
            }
            */
            OpenConnect();
            CloseConnect();

        }

        public void OpenConnect()
        {
            string server = "localhost"; //string server = "127.0.0.1";
            string dataBaseName = this.dataBaseName;
            string userName = "root";
            string password = "";

            try
            {
                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", server, dataBaseName, userName, password);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void CloseConnect()
        {
            if (connection != null &&
           connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
                Console.WriteLine("Conexión cerrada.");
            }
        }

        public void InsertarContacto(Contacto contacto)
        {
            OpenConnect();
            try
            {
                string query = "INSERT INTO Contactos (nombre, apellido, telefono, direccion, localidad, email, fecha) VALUES (@nombre, @apellido, @telefono, @direccion, @localidad, @email, @fecha);";

                using var cmd = new MySqlCommand(query,connection);

                cmd.Parameters.AddWithValue("@nombre", contacto.Nombre);
                cmd.Parameters.AddWithValue("@apellido", contacto.Apellido);
                cmd.Parameters.AddWithValue("@telefono", contacto.Telefono);
                cmd.Parameters.AddWithValue("@direccion", contacto.Direccion);
                cmd.Parameters.AddWithValue("@localidad", contacto.Localidad);
                cmd.Parameters.AddWithValue("@email", contacto.Email);
                cmd.Parameters.AddWithValue("@fecha", contacto.Fecha?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? (object)DBNull.Value);

                cmd.Prepare();                
                cmd.ExecuteNonQuery();
                
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();
        }

        public void DeleteContacto(int id)
        {
            OpenConnect();
            try
            {
                string query = "DELETE FROM Contactos WHERE id_contactos = @id";

                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
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
                string query = "SELECT * FROM Contactos";
                using var cmd = new MySqlCommand(query, connection);

                cmd.Prepare();

                using var sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    contactos.Add(MapContacto(sdr));
                }
            }
            catch (MySqlException ex)
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
                string query = "SELECT * FROM Contactos WHERE id_contactos = @id";
                using var cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                using var sdr = cmd.ExecuteReader();
                if (sdr.Read())
                {
                    contacto = MapContacto(sdr);
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();

            return contacto;
        }

        



        public List<Contacto> SearchFirtsOrLastName(string name)
        {
            List<Contacto> contactos = new List<Contacto>();
            OpenConnect();
            try
            {
                string query = "SELECT * FROM Contactos WHERE nombre LIKE @name OR apellido LIKE @name";
                using var cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@name", $"%{name}%");
                cmd.Prepare();

                using var sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    contactos.Add(MapContacto(sdr));
                }
            }
            catch (MySqlException ex)
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
                using var cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@apellido", $"%{apellido}%");
                cmd.Prepare();

                using var sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    contactos.Add(MapContacto(sdr));
                }
            }
            catch (MySqlException ex)
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
                using var cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@nombre", contacto.Nombre);
                cmd.Parameters.AddWithValue("@apellido", contacto.Apellido);
                cmd.Parameters.AddWithValue("@telefono", contacto.Telefono);
                cmd.Parameters.AddWithValue("@direccion", contacto.Direccion);
                cmd.Parameters.AddWithValue("@localidad", contacto.Localidad);
                cmd.Parameters.AddWithValue("@email", contacto.Email);
                cmd.Parameters.AddWithValue("@fecha", contacto.Fecha.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                cmd.Parameters.AddWithValue("@id", contacto.Id_contacto);

                cmd.Prepare();
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }
            CloseConnect();
        }

        private static Contacto MapContacto(MySqlDataReader sdr)
        {
            Contacto contacto = new Contacto();
            contacto.Id_contacto = sdr.GetInt32(0);
            contacto.Nombre = sdr.IsDBNull(1) ? "" : sdr.GetString(1);
            contacto.Apellido = sdr.IsDBNull(2) ? "" : sdr.GetString(2);
            contacto.Telefono = sdr.IsDBNull(3) ? "" : sdr.GetString(3);
            contacto.Direccion = sdr.IsDBNull(4) ? "" : sdr.GetString(4);
            contacto.Localidad = sdr.IsDBNull(5) ? "" : sdr.GetString(5);
            contacto.Email = sdr.IsDBNull(6) ? "" : sdr.GetString(6);

            if (!sdr.IsDBNull(7))
            {
                object rawFecha = sdr.GetValue(7);

                if (rawFecha is DateTime dt)
                {
                    contacto.Fecha = DateOnly.FromDateTime(dt);
                }
                else
                {
                    string fechaStr = rawFecha.ToString() ?? "";
                    if (DateOnly.TryParse(fechaStr, out DateOnly fecha))
                    {
                        contacto.Fecha = fecha;
                    }
                }
            }

            return contacto;
        }
    }
}