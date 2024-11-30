namespace FlightReservationSystemProject;

using System; 
using System.Collections.Generic;
using System.IO;

public static class FileAndMenuHelperMethods
{
    public const string RESET = "\u001B[0m";
    public const string CYAN = "\u001B[36m";
    public const string RED = "\u001B[31m";
 
    
    // Check if file exists and read all lines from file if it does. 
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

    // Append; writes single line to file. 
    public static void AppendToFile(string fileName_p, string lines_p)
    {
        using (StreamWriter writer = new StreamWriter(fileName_p, append: true))
        {
            writer.WriteLine(lines_p);
        }
    }

    // Creates single customer string and appends single line to given file.
    public static void AddCustomer(string filename_p, CustomerAcc customer)
    {
        string customerLine =
            $"{customer.CustomerID}|{customer.CustomerFirstName}|{customer.CustomerLastName}|{customer.PhoneNumber}|{customer.CustomerNumOfBookings}";
        AppendToFile(filename_p, customerLine);
    }
    
    // Asks customer for input to continue. 
    public static void Pause()
    {
        Console.WriteLine(CYAN + "\nPress Enter to continue..." + RESET);
        Console.ReadLine();
    }
    
    // Asks customer for 'y/Y' or 'n/N' to return to main menu.
    public static bool ConfirmReturnToMainMenu()
    {
        Console.Write(RED + "\n Are you sure want to return to the Main Menu? (Y/N): " + RESET);
        // ? is for the conversion of null literal and/or value into non-nullable type.
        string confirmation = Console.ReadLine()?.Trim().ToUpper() ?? "N";
        return confirmation == "Y";
    }
}