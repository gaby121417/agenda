using agendaSQLite;

internal class Program
{
    private static void Main(string[] args)
    {
        /*
        MySQLDatabase database = new MySQLDatabase("agendaCS");

        Contacto contacto = new Contacto("Juan Carlos", "Flores Paulín", "5513612622", "San Cristobal", "Ecatepec","juan.flores@sancarlos.edu.mx", new DateOnly(2026, 03, 07));

        database.InsertarContacto(contacto);
        */

        SQLiteDatabase database = new SQLiteDatabase("agenda_nueva.db");

        Console.WriteLine("--- Todos los contactos ---");
        List<Contacto> contactosSelect = database.GetAllContactos();

        foreach (Contacto con in contactosSelect)
        {
            Console.WriteLine(con.ToString());
        }

        Console.WriteLine("\n--- Buscar por nombre/apellido: 'Juan' ---");
        List<Contacto> coincidencias = database.SearchFirtsOrLastName("Juan");
        foreach (Contacto con in coincidencias)
        {
            Console.WriteLine(con.ToString());
        }

        int id = 1;
        Contacto? contacto = database.GetContacto(id);

        if (contacto != null)
        {
            Console.WriteLine("\n--- Contacto por id ---");
            Console.WriteLine(contacto.ToString());

            contacto.Telefono = "5550000000";
            contacto.Fecha = DateOnly.FromDateTime(DateTime.ParseExact("14/03/2026", "dd/MM/yyyy", null));
            database.UpdateContacto(contacto);
            Console.WriteLine($"Contacto con id {id} actualizado.");

            // Cambia este id por uno real para probar borrado si lo necesitas.
            // database.DeleteContacto(id);
            // Console.WriteLine($"Contacto con id {id} eliminado.");
        }
        else
        {
            Console.WriteLine($"No se encontró el contacto con id {id}");
        }

        List<Contacto> contactos = database.SearchByLastName("Flores");
        Console.WriteLine("\n--- Contactos con apellido 'Flores' ---");
        foreach (Contacto con in contactos)
        {
            Console.WriteLine(con.ToString());
        }

        Console.WriteLine("¡Termine!");
    }
}