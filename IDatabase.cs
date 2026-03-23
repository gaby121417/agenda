using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Xml.Serialization;

namespace agendaSQLite
{
    public interface IDatabase
    {
        void OpenConnect();
        void InsertarContacto(Contacto contacto);
        List<Contacto> GetAllContactos();
        List<Contacto> SearchFirtsOrLastName(string name);
        List<Contacto> SearchByLastName(string apellido);
        Contacto? GetContacto(int id);
        void UpdateContacto(Contacto contacto);
        void DeleteContacto(int id);
        void CloseConnect();
        
    }

}