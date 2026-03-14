using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
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
        }

        private void CreateDataBase()
        {
            string filePath = @"./crearDBagenda.sql";

            OpenConnect();

            if(File.Exists(filePath))
            {
                string sqlScript = File.ReadAllText(filePath);
                try
                {
                    using var cmd = new SqliteCommand(sqlScript, connection);
                    cmd.ExecuteNonQuery();
                }
                catch(SqliteException ex)
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
            catch(SqliteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CloseConnect()
        {
            if(connection != null)
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
                cmd.Parameters.AddWithValue("@fecha", contacto.Fecha);

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

                while(sdr.Read())
                {
                    Contacto contacto = new Contacto();
                    contacto.Id_contacto = sdr.GetInt32(0);
                    contacto.Nombre = sdr.GetString(1);
                    contacto.Apellido = sdr.GetString(2);
                    contacto.Telefono = sdr.GetString(3);
                    contacto.Direccion = sdr.GetString(4);
                    contacto.Localidad = sdr.GetString(5);
                    contacto.Email = sdr.GetString(6);
                    /*
                    if(sdr.GetString(7) != String.Empty)
                    {
                        DateOnly.FromDateTime(DateTime.ParseExact(sdr.GetString(7), "dd-MM-yyyy", null));
                    }
                    */
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

        public void DeleteContacto(int id)
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