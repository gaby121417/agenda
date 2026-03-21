using agendaSQLite;

internal class Program
{
    private static void Main(string[] args)
    {
        /*
        
        */
        /*
        MySQLDatabase database = new MySQLDatabase("agendaCS");

        Contacto contacto = new Contacto("Juan Carlos", "Flores Paulín", "5513612622", "San Cristobal", "Ecatepec","juan.flores@sancarlos.edu.mx", new DateOnly(2026, 03, 07));

        database.InsertarContacto(contacto);
        */

        SQLiteDatabase database = new SQLiteDatabase("agenda_nueva.db");

        //Contacto contacto = new Contacto("Juan Carlos", "Flores Paulín", "5513612622", "San Cristobal", "Ecatepec","juan.flores@sancarlos.edu.mx", new DateOnly(2026, 03, 07));

        //database.InsertarContacto(contacto);

        List<Contacto> contactosSelect = database.GetAllContactos();

        foreach(Contacto con in contactosSelect)
        {
            Console.WriteLine(con.ToString());
        }

        int id = 1;
        Contacto? contacto;

        contacto = database.GetContacto(id);

        if(contacto != null)
        {
            Console.WriteLine(contacto.ToString());
            contacto.Fecha = DateOnly.FromDateTime(DateTime.ParseExact("14/03/2026", "dd/MM/yyyy", null));
            database.UpdateContacto(contacto);
            Console.WriteLine(contacto.Fecha.ToString());
        }
        else
        {
            Console.WriteLine("No se encontró el contacto");
        }

        



        Console.WriteLine("¡Termine!");
    }
}