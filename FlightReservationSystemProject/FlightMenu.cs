using System.Reflection.Metadata;
using System.Globalization;
namespace FlightReservationSystemProject;

public class FlightMenu
{
    public const string RESET = "\u001B[0m";
    public const string RED = "\u001B[31m";
    public const string GREEN = "\u001B[32m";
    public const string YELLOW = "\u001B[33m";
    public const string CYAN = "\u001B[36m";

    private const string FlightsFile = "./flights.txt";
    
    public void ShowMenu()
    {
        bool RUNNING = true;
        while (RUNNING)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(YELLOW + "  ╔══════════════════════════════════════════════════╗");
            Console.WriteLine("  ║                 Extreme Flight Menu!             ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════════╝" + RESET);
            Console.WriteLine("");
            Console.WriteLine(CYAN + "\n Please select a choice from the options below (Enter 1-5):" + RESET);
            Console.WriteLine(GREEN + "\n 1. Add flight");
            Console.WriteLine("\n 2. View Flights");
            Console.WriteLine("\n 3. View Specific Flight");
            Console.WriteLine("\n 4. Delete Flight" + RESET);
            Console.WriteLine(RED + "\n 5. Back to Main Menu" + RESET);
            Console.Write(CYAN + "\nSelect an Option:  " + RESET);

            string userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "1":
                    AddFlight();
                    break;
                case "2":
                    ViewAllFlights();
                    break;
                case "3":
                    ViewSpecificFlight();
                    break;
                case "4":
                    DeleteFight();
                    break;
                case "5":
                    if (FileAndMenuHelperMethods.ConfirmReturnToMainMenu())
                    {
                        RUNNING = false;
                    }
                    break;
                default:
                    Console.WriteLine(RED+"   Invalid option please select an option by inputting a number between 1-4."+RESET);
                    FileAndMenuHelperMethods.Pause();
                    break;
            }
        }
    }

    private void AddFlight()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(YELLOW + "  ╔════════════════════════════════════════════╗");
        Console.WriteLine("  ║                 Add A Flight!              ║");
        Console.WriteLine("  ╚════════════════════════════════════════════╝" + RESET);
        Console.WriteLine("");

        // Flight Number Input
        int flightNumber;
        while (true)
        {
            Console.Write(CYAN + "Enter Flight Number: " + RESET);
            if (int.TryParse(Console.ReadLine(), out flightNumber))
            {
                break; // Valid input, exit loop
            }
            Console.WriteLine(RED + "  ! Invalid flight number. Please enter a valid number." + RESET);
        }

        // Origin Input
        string flightOrigin;
        while (true)
        {
            Console.Write(CYAN + "Enter Origin City: " + RESET);
            flightOrigin = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(flightOrigin) && InputValidation.IsValidName(flightOrigin))
            {
                flightOrigin = InputValidation.CapitalizeEachWord(flightOrigin);
                break; // Valid input, exit loop
            }
            Console.WriteLine(RED + " ! Origin must only contain alphabetic characters and cannot be empty." + RESET);
        }

        // Destination Input
        string flightDestination;
        while (true)
        {
            Console.Write(CYAN + "Enter Destination City: " + RESET);
            flightDestination = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(flightDestination) && InputValidation.IsValidName(flightDestination))
            {
                flightDestination = InputValidation.CapitalizeEachWord(flightDestination);
                break; // Valid input, exit loop
            }
            Console.WriteLine(RED + " ! Destination must only contain alphabetic characters and cannot be empty." + RESET);
        }

        // Maximum Seats Input
        int flightMaxSeats;
        while (true)
        {
            Console.Write(CYAN + "Enter Maximum Seats: " + RESET);
            if (int.TryParse(Console.ReadLine(), out flightMaxSeats) && flightMaxSeats > 40 && flightMaxSeats<851)
            {
                break; // Valid input, exit loop
            }
            Console.WriteLine(RED + " ! Invalid max seats. Please enter max seats between 40 and 850." + RESET);
        }

        // File Operations
        try
        {
            string[] lines = FileAndMenuHelperMethods.ReadFile(FlightsFile);

            // Checking for duplicate flight numbers
            if (!ObjectHelperMethods.IsflightNumberUnique(flightNumber, lines))
            {
                Console.WriteLine(RED + " Error: Flight number already exists." + RESET);
                FileAndMenuHelperMethods.Pause();
                return;
            }

            string newFlightLine = $"{flightNumber}|{flightOrigin}|{flightDestination}|{flightMaxSeats}|0";
            FileAndMenuHelperMethods.AppendToFile(FlightsFile, newFlightLine);

            Console.WriteLine(GREEN + $"\n  Flight added successfully! Number: {flightNumber}" + RESET);
        }
        catch (Exception ex)
        {
            Console.WriteLine(RED + $"Error: {ex.Message}" + RESET);
        }

        FileAndMenuHelperMethods.Pause();
    }
    
    // View all flights
    private void ViewAllFlights()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(YELLOW + "  ╔═══════════════════════════════════════════════╗");
        Console.WriteLine("  ║                 View All Flights!             ║");
        Console.WriteLine("  ╚═══════════════════════════════════════════════╝" + RESET);
        Console.WriteLine("");
        
        try
        {
            string[] lines = FileAndMenuHelperMethods.ReadFile(FlightsFile);
            if (lines.Length == 0)
            {
                Console.WriteLine(RED+"No flights available."+RED);
            }
            else
            {
                foreach (var line in lines)
                {
                    Console.WriteLine(ObjectHelperMethods.ParseFlight(line));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(RED+$"Error: {ex.Message}"+RESET);
        }
        
        FileAndMenuHelperMethods.Pause();
    }
    
    // View single Flight via ID. 
    private void ViewSpecificFlight()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(YELLOW + "  ╔═══════════════════════════════════════════════════╗");
        Console.WriteLine("  ║                 View Specific Flight!             ║");
        Console.WriteLine("  ╚═══════════════════════════════════════════════════╝" + RESET);
        Console.WriteLine("");
       
        
        Console.Write(CYAN+"Enter valid Flight Number: "+RESET);
        if (!int.TryParse(Console.ReadLine(), out int flightNumResult))
        {
            Console.WriteLine(RED + " ! Invalid flight number.");
            FileAndMenuHelperMethods.Pause();
            return;
        }

        try
        {
            string[] lines = FileAndMenuHelperMethods.ReadFile(FlightsFile);
            string flight = ObjectHelperMethods.FindFlightByNumber(flightNumResult, lines);

            if (flight != null)
            {
                Console.WriteLine(ObjectHelperMethods.ParseFlight(flight));
            }
            else
            {
                Console.WriteLine(RED + " ! Flight not found."+RESET);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(RED + $"Error: {ex.Message}"+RESET);
        }
        
        FileAndMenuHelperMethods.Pause();
    }

    // Deletes flight if it has no passengers. 
    private void DeleteFight()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(RED + "  ╔══════════════════════════════════════════════╗");
        Console.WriteLine("  ║                 Delete A Flight!             ║");
        Console.WriteLine("  ╚══════════════════════════════════════════════╝" + RESET);
        Console.WriteLine("");
        
        Console.Write(CYAN+"Enter valid Flight Number to delete: "+RESET);

        if (!int.TryParse(Console.ReadLine(), out int flightNum))
        {
            Console.WriteLine(RED+" ! Invalid flight number."+RESET);
            FileAndMenuHelperMethods.Pause();
            return;
        }

        try
        {
            string[] lines = FileAndMenuHelperMethods.ReadFile(FlightsFile);
            string[] updatedLines = new string[lines.Length - 1];
            int index = 0;
            bool found = false;

            foreach (var line in lines)
            {
                string[] parts = line.Split('|');
                int currentFlightNumber = int.Parse(parts[0]);
                int passangerCount = int.Parse(parts[4]);

                if (currentFlightNumber == flightNum)
                {
                    if (passangerCount > 0)
                    {
                        Console.WriteLine(YELLOW+"Cannot delete a flight with booked passangers."+RESET);
                        FileAndMenuHelperMethods.Pause();
                        return;
                    }

                    found = true;
                    continue; 
                }
                
                if (index < updatedLines.Length)
                {
                    updatedLines[index++] = line;
                }
            }

            if (!found)
            {
                Console.WriteLine(RED+" ! Flight not found."+RESET);
            }
            else
            {
                FileAndMenuHelperMethods.WriteFile(FlightsFile, updatedLines);
                Console.WriteLine(GREEN+"Flight deleted successfully."+RESET);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(RED+$"Error deleting flight: {ex.Message}"+RESET);
        }
        
        FileAndMenuHelperMethods.Pause();
    }
}