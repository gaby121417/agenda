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

        SQLiteDatabase database = new SQLiteDatabase("agenda.db");

        //Contacto contacto = new Contacto("Juan Carlos", "Flores Paulín", "5513612622", "San Cristobal", "Ecatepec","juan.flores@sancarlos.edu.mx", new DateOnly(2026, 03, 07));

        //database.InsertarContacto(contacto);

        List<Contacto> contactosSelect = database.GetAllContactos();

        foreach(Contacto con in contactosSelect)
        {
            Console.WriteLine(con.ToString());
        }

        Console.WriteLine("¡Termine!");
    }
}