﻿using System.IO.Pipes;
using System.Reflection.Metadata;

namespace FlightReservationSystemProject;

public class BookingMenu
{
    public const string RESET = "\u001B[0m";
    public const string RED = "\u001B[31m";
    public const string GREEN = "\u001B[32m";
    public const string YELLOW = "\u001B[33m";
    public const string BLUE = "\u001B[34m";
    public const string PURPLE = "\u001B[35m";
    public const string CYAN = "\u001B[36m";

    private const string CustomersFile = "./customers.txt";
    private const string FlightsFile = "./flights.txt";
    private const string BookingsFile = "./bookings.txt";

    private void AddBooking()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(YELLOW + "  ╔═══════════════════════════════════════════════════════════════╗");
        Console.WriteLine("  ║                      Add Flight Booking!                      ║");
        Console.WriteLine("  ╚═══════════════════════════════════════════════════════════════╝" + RESET);
        Console.WriteLine("");

        // Load Customers
        string[] customerLines = FileAndMenuHelperMethods.ReadFile(CustomersFile);
        if (customerLines.Length == 0)
        {
            Console.WriteLine(RED + " ! No customer available. Add a customer first." + RESET);
            FileAndMenuHelperMethods.Pause();
            return;
        }

        Console.WriteLine(CYAN + "Available Customers:" + RESET);
        foreach (var line in customerLines)
        {
            Console.WriteLine(ObjectHelperMethods.ParseCustomer(line));
        }

        Console.Write(CYAN + "Enter Customer ID: " + RESET);
        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine(RED + " ! Invalid Customer ID." + RESET);
            FileAndMenuHelperMethods.Pause();
            return;
        }

        // Validate customer
        string customer = ObjectHelperMethods.FindCustomerByID(customerId, customerLines);
        if (customer == null)
        {
            Console.WriteLine(RED + " ! Customer not found." + RESET);
            FileAndMenuHelperMethods.Pause();
            return;
        }

        // Load flights
        string[] flightLines = FileAndMenuHelperMethods.ReadFile(FlightsFile);
        if (flightLines.Length == 0)
        {
            Console.WriteLine(RED + " ! No flights available. Add a flight first." + RESET);
            FileAndMenuHelperMethods.Pause();
            return;
        }

        Console.WriteLine(CYAN + "\n Available Flights: " + RESET);
        foreach (var line in flightLines)
        {
            Console.WriteLine(ObjectHelperMethods.ParseFlight(line));
        }

        // Ask for flight number and check if valid
        Console.Write(CYAN + "Enter Flight Number: " + RESET);
        if (!int.TryParse(Console.ReadLine(), out int flightNum))
        {
            Console.WriteLine(RED + " ! Invalid Flight Number." + RESET);
            FileAndMenuHelperMethods.Pause();
            return;
        }

        // Validate flight
        string flight = ObjectHelperMethods.FindFlightByNumber(flightNum, flightLines);
        if (flight == null)
        {
            Console.WriteLine(RED + " ! Flight not found." + RESET);
            FileAndMenuHelperMethods.Pause();
            return;
        }

        string[] flightParts = flight.Split('|');
        int maxSeats = int.Parse(flightParts[3]);
        int currentPassengers = int.Parse(flightParts[4]);
        if (currentPassengers >= maxSeats)
        {
            Console.WriteLine(RED + " ! Flight is fully booked." + RESET);
            FileAndMenuHelperMethods.Pause();
            return;
        }

        // Check for conflicting bookings
        try
        {
            string[] bookingLines = FileAndMenuHelperMethods.ReadFile(BookingsFile);
            DateTime now = DateTime.Now;

            foreach (var bookingLine in bookingLines)
            {
                if (string.IsNullOrWhiteSpace(bookingLine)) continue;

                string[] bookingParts = bookingLine.Split('|');
                if (bookingParts.Length < 4) continue;

                int existingCustomerId = int.Parse(bookingParts[2]);
                int existingFlightNum = int.Parse(bookingParts[3]);
                DateTime existingBookingTime = DateTime.Parse(bookingParts[1]);

                // Check if the same customer has a booking within one hour
                if (existingCustomerId == customerId && Math.Abs((existingBookingTime - now).TotalHours) < 1)
                {
                    Console.WriteLine(RED + " ! Customer already has a booking within the same time period." + RESET);
                    FileAndMenuHelperMethods.Pause();
                    return;
                }
            }

            // Proceed with booking
            int bookingID = bookingLines.Length + 1;
            string newBookingLine = $"{bookingID}|{DateTime.Now}|{customerId}|{flightNum}";
            FileAndMenuHelperMethods.AppendToFile(BookingsFile, newBookingLine);

            // Update Flight passenger count
            string[] updatedFlights = new string[flightLines.Length];
            for (int i = 0; i < flightLines.Length; i++)
            {
                if (flightLines[i].StartsWith(flightNum.ToString()))
                {
                    string updatedFlight =
                        $"{flightParts[0]}|{flightParts[1]}|{flightParts[2]}|{flightParts[3]}|{currentPassengers + 1}";
                    updatedFlights[i] = updatedFlight;
                }
                else
                {
                    updatedFlights[i] = flightLines[i];
                }
            }

            FileAndMenuHelperMethods.WriteFile(FlightsFile, updatedFlights);
            Console.WriteLine(GREEN + $"Booking created successfully! Booking ID: {bookingID}" + RESET);
        }
        catch (Exception ex)
        {
            Console.WriteLine(RED + $"Error: {ex.Message}" + RESET);
        }

        FileAndMenuHelperMethods.Pause();
    }


    private void ViewAllBookings()
    {
        Console.Clear();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine(YELLOW + "  ╔════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("  ║                      View All Flight Booking!                      ║");
        Console.WriteLine("  ╚════════════════════════════════════════════════════════════════════╝" + RESET);
        Console.WriteLine("");

        try
        {
            string[] lines = FileAndMenuHelperMethods.ReadFile(BookingsFile);
            if (lines.Length == 0)
            {
                Console.WriteLine(RED + " No bookings available." + RESET);
            }
            else
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue; // Skip empty lines

                    string[] parts = line.Split('|');
                    if (parts.Length < 4)
                    {
                        Console.WriteLine(RED + $" Invalid booking entry: {line}" + RESET);
                        continue;
                    }

                    Console.WriteLine(
                        $"Booking ID: {parts[0]}, Date: {parts[1]}, Customer ID: {parts[2]}, Flight Number: {parts[3]}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(RED + $"Error reading bookings: {ex.Message}" + RESET);
        }

        FileAndMenuHelperMethods.Pause();
    }




    public void ShowMenu()
    {
        bool RUNNING = true;
        while (RUNNING)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(YELLOW + "  ╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║                   Extreme Booking Menu!                  ║");
            Console.WriteLine("  ╚══════════════════════════════════════════════════════════╝" + RESET);
            Console.WriteLine("");
            Console.WriteLine(CYAN+"\n Please select a choice from the options below (1-4):"+RESET);
            Console.WriteLine(GREEN+"\n 1. Add Booking.");
            Console.WriteLine("\n 2. View All Bookings."+RESET);
            Console.WriteLine(RED+"\n 3. Back to Main Menu. "+RESET);
            Console.Write(CYAN+"Select an option: "+RESET);
            
            string userChoice = Console.ReadLine()?.Trim();
            switch (userChoice)
            {
                case "1":
                    AddBooking();
                    break;
                case "2":
                    ViewAllBookings();
                    break;
                case "3":
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
}