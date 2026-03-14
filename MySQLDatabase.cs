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
                cmd.Parameters.AddWithValue("@fecha", contacto.Fecha.ToString("o", CultureInfo.InvariantCulture));

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
            throw new NotImplementedException();
        }

        public List<Contacto> GetAllContactos()
        {
            throw new NotImplementedException();
        }

        public Contacto? GetContacto(int id)
        {
            throw new NotImplementedException();
        }

        



        public List<Contacto> SearchFirtsOrLastName(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateContacto(Contacto contacto)
        {
            throw new NotImplementedException();
        }
    }
}