namespace FlightReservationSystemProject;

public class CustomerMenu
{
    private const string CustomerFile = "./customers.txt";
    private const string BookingsFile = "./bookings.txt";
    public const string RESET = "\u001B[0m";
    public const string RED = "\u001B[31m";
    public const string GREEN = "\u001B[32m";
    public const string YELLOW = "\u001B[33m";
    public const string CYAN = "\u001B[36m";

    
    public void DisplayCustomerMenu()
    {
        bool RUNNING = true;
        while (RUNNING)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(YELLOW + "  ╔═════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║                          Extreme Customer Menu!                         ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════════════════════════╝" + RESET);

            Console.WriteLine(CYAN+"\nPlease select a choice from the options below (Enter 1-4):"+RESET);
            Console.WriteLine(GREEN+"\n 1. Add Customer.");
            Console.WriteLine("\n 2. View all Customers.");
            Console.WriteLine("\n 3. Delete Customer. "+ RESET);
            Console.WriteLine(RED+"\n 4. Back to Main Menu." + RESET);
            Console.Write(CYAN+"\nSelect an Option:  "+RESET);
            string userChoice = Console.ReadLine()?.Trim();
            
            switch (userChoice)
            {
                case "1":
                    AddCustomer();
                    break;
                case "2":
                    ViewCustomers();
                    break;
                case "3":
                    DeleteCustomer();
                    break;
                case "4":
                    if (FileAndMenuHelperMethods.ConfirmReturnToMainMenu())
                    {
                        RUNNING = false;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid option please select an option by inputting a number between 1-4.");
                    FileAndMenuHelperMethods.Pause();
                    break;
            }
        }
    }

    // If customer does not exist on file it should then create a customer object with user input.
    // Customer object to string.
    // string to array. 
    // array to saving in customers.txt. 
    private void AddCustomer()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(YELLOW + "  ╔════════════════════════════════════════════╗");
        Console.WriteLine("  ║              1. Add Customer!              ║");
        Console.WriteLine("  ╚════════════════════════════════════════════╝" + RESET);
        Console.WriteLine();


        string firstName;
        while (true)
        {
            Console.Write(CYAN + " Enter First Name: " + RESET);
            firstName = Console.ReadLine()?.Trim();
            if (!InputValidation.IsValidName(firstName))
            {
                Console.WriteLine(RED + "     Invalid first name. Please use only letters." + RESET);
            }
            else
            {
                break;
            }
        }

        string lastName;
        while (true)
        {
            Console.Write(CYAN + " Enter Last Name: " + RESET);
            lastName = Console.ReadLine()?.Trim();
            if (!InputValidation.IsValidName(lastName))
            {
                Console.WriteLine(RED + "     Invalid last name. Please use only letters." + RESET);
            }
            else
            {
                break;
            }
        }

        string phoneNumber;
        while (true)
        {
            Console.Write(CYAN + " Enter Phone Number: " + RESET);
            phoneNumber = Console.ReadLine()?.Trim();
            if (!InputValidation.IsValidPhoneNumber(phoneNumber))
            {
                Console.WriteLine(RED + "     Invalid phone number. Please use a valid format like (123)456-7890 or 123-456-7890 or 2225550000." + RESET);
            }
            else
            {
                break;
            }
        }

        // Check the next available customer Id

        int nextCustomerId = 1; // If file is empty, counting id starts from 1
        try
        {
            string[] customers = FileAndMenuHelperMethods.ReadFile(CustomerFile); // reads list of customers
            if (customers.Length > 0)
            {
                // Extract the highest customer ID from the file
                nextCustomerId = customers
                    .Select(line => line.Split('|'))
                    .Where(parts => parts.Length > 0 && int.TryParse(parts[0], out _))
                    .Select(parts => int.Parse(parts[0]))
                    .DefaultIfEmpty(0) // Default to 0 if no valid IDs are found
                    .Max() + 1; // Increment the highest ID
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(RED + $"Error reading customer file: {ex.Message}" + RESET);
            FileAndMenuHelperMethods.Pause();
            return;
        }


        try
        {
            CustomerAcc newCustomer = new CustomerAcc(nextCustomerId,firstName, lastName, phoneNumber);
            FileAndMenuHelperMethods.AddCustomer(CustomerFile, newCustomer);
            Console.WriteLine(GREEN + "\n    Customer added successfully!" + RESET);
        }
        catch (Exception ex)
        {
            Console.WriteLine(RED + $"Error: {ex.Message}" + RESET);
        }

        FileAndMenuHelperMethods.Pause();
    }

    // View all customers in the customers.txt file. 
    private void ViewCustomers()
    {
        Console.Clear();

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(YELLOW + "  ╔══════════════════════════════════════════════╗");
        Console.WriteLine("  ║              2. View Customers!              ║");
        Console.WriteLine("  ╚══════════════════════════════════════════════╝" + RESET);
        Console.WriteLine();
        

        try
        {
            string[] customers = FileAndMenuHelperMethods.ReadFile(CustomerFile);
            if (customers.Length == 0)
            {
                Console.WriteLine("No customers found.");
            }
            else
            {
                foreach (string line in customers)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length >= 5)
                    {
                        Console.WriteLine($"ID: {parts[0]}, Name: {parts[1]} {parts[2]}, Phone: {parts[3]}, Bookings: {parts[4]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        FileAndMenuHelperMethods.Pause();
    }

    private void DeleteCustomer()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(YELLOW + "  ╔═════════════════════════════════════════════╗");
        Console.WriteLine("  ║              Delete Customer !              ║");
        Console.WriteLine("  ╚═════════════════════════════════════════════╝" + RESET);
        
        Console.WriteLine(CYAN+"Enter a valid Customer ID to delete: "+RESET);

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid ID");
            FileAndMenuHelperMethods.Pause();
            return;
        }

        try
        {
            // Check for customers existing booking. Provide error if found and return. 
            string[] bookingLines = FileAndMenuHelperMethods.ReadFile(BookingsFile);
            foreach (string bookingLine in bookingLines)
            {
                string[] bookingParts = bookingLine.Split('|');
                if (bookingParts.Length > 2 && int.Parse(bookingParts[2]) == id)
                {
                    Console.WriteLine(RED + "Cannot delete customer with existing booking." + RESET);
                    FileAndMenuHelperMethods.Pause();
                    return;
                }
            }
            
            // Delete customer if no existing booking found. 
            string[] customerLines = FileAndMenuHelperMethods.ReadFile(CustomerFile);
            string[] updatedLines = new string[customerLines.Length];
            int index = 0;
            bool found = false;

            foreach (string line in customerLines)
            {
                string[] parts = line.Split('|');
                if (int.TryParse(parts[0], out int customerID) && customerID == id)
                {
                    found = true;
                }
                else
                {
                    updatedLines[index++] = line;
                }
            }

            if (!found)
            {
                Console.WriteLine("Customer not found");
            }
            else
            {
                string[] finalLines = new string[index];
                Array.Copy(updatedLines, finalLines, index);
                FileAndMenuHelperMethods.WriteFile(CustomerFile, finalLines);
                Console.WriteLine(GREEN + "\n   Customer deleted successfully." + RESET);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        
        FileAndMenuHelperMethods.Pause();
    }
}
