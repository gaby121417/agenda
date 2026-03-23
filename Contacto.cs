using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agendaSQLite
{
    public class Contacto
    {
        public int Id_contacto {set; get;}
        public string Nombre {set; get;}
        public string Apellido {set; get;}
        public string Telefono {set; get;}
        public string Direccion {set; get;}
        public string Localidad { set; get;}
        public string Email {set; get;}
        public DateOnly? Fecha {set; get;}

        public Contacto()
        {
            this.Nombre = "";
            this.Apellido = "";
            this.Telefono = "";
            this.Direccion = "";
            this.Localidad = "";
            this.Email = "";
            this.Fecha = DateOnly.FromDateTime(DateTime.Today);
        }

        public Contacto(string nombre, string apellido, string telefono, 
        string direccion, string localidad, string email, DateOnly fecha)
        {
            this.Nombre = nombre;
            this.Apellido = apellido;
            this.Telefono = telefono;
            this.Direccion = direccion;
            this.Localidad = localidad;
            this.Email = email;
            this.Fecha = fecha;
            
        }
        public override string ToString()
        {
            return "\nContacto: " + Id_contacto + ", " + Apellido + ", " + Nombre + ", " + Email + ", " + Fecha;
        }
        
    }
}