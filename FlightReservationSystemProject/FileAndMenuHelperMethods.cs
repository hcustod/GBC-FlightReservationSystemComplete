namespace FlightReservationSystemProject;

using System; 
using System.Collections.Generic;
using System.IO;

public static class FileAndMenuHelperMethods
{
    
    // Check if file exists and read all lines from file if true. 
    public static string[] ReadFile(string fileName_p)
    {
        if (!File.Exists(fileName_p))
        {
            File.WriteAllText(fileName_p, string.Empty);
        }

        return File.ReadAllLines(fileName_p);
    }

    // Write all lines to a file
    public static void WriteFile(string fileName_p, string[] lines_p)
    {
        File.WriteAllLines(fileName_p, lines_p);
    }

    // Append single line to file. 
    public static void AppendToFile(string fileName_p, string lines_p)
    {
        using (StreamWriter writer = new StreamWriter(fileName_p, append: true))
        {
            writer.WriteLine(lines_p);
        }
    }

    public static void AddCustomer(string filename_p, CustomerAcc customer)
    {
        string customerLine =
            $"{customer.CustomerID}|{customer.CustomerFirstName}|{customer.CustomerLastName}|{customer.PhoneNumber}|{customer.CustomerNumOfBookings}";
        AppendToFile(filename_p, customerLine);
    }
    
    public static void Pause()
    {
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
    
    public static bool ConfirmReturnToMainMenu()
    {
        Console.Write("Are you sure want to return to the Main Menu? (Y/N): ");
        // ? is for the conversion of null literal and/or value into non-nullable type.
        string confirmation = Console.ReadLine()?.Trim().ToUpper() ?? "N";
        return confirmation == "Y";
    }
}