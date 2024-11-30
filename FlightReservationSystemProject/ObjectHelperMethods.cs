namespace FlightReservationSystemProject;

using System;
using System.Collections.Generic;

public class ObjectHelperMethods
{
    
    private const string CustomersFile = "./customers.txt";
    private const string FlightsFile = "./flights.txt";
    
    // Find customer by ID
    public static string FindCustomerByID(int id, string[] lines)
    {
        foreach (var line in lines)
        {
            string[] parts = line.Split('|');
            if (int.Parse(parts[0]) == id)
            {
                return line;
            }
        }
        
        return null;
    }

    // Parses customer record from txt file into string
    public static string ParseCustomer(string line_p)
    {
        string[] parts = line_p.Split('|');
        return $"ID: {parts[0]}, Name: {parts[1]} {parts[2]}, Phone: {parts[3]}, Bookings: {parts[4]}";
    }

    public static string FindFlightByNumber(int flightNumber_p, string[] lines)
    {
        foreach (var line in lines)
        {
            string[] parts = line.Split('|');
            if (int.Parse(parts[0]) == flightNumber_p)
            {
                return line; 
            }
        }

        return null; 
    }

    public static bool IsflightNumberUnique(int flightNum_p, string[] lines)
    {
        return FindFlightByNumber(flightNum_p, lines) == null;
    }

    public static string ParseFlight(string line)
    {
        string[] parts = line.Split('|');
        return $"Number: {parts[0]}, Origin: {parts[1]}, Destination: {parts[2]}, Max Seats: {parts[3]}, Passengers: {parts[4]}";
    }
    
    // Method to update customer booking count displayed on view customers. 
    // 1st param takes the customer id to update and the second takes the change (+1 or -1).
    public static void UpdateCustomerBookingCount(int customerId, int d)
    {
        // Read lines from customer.txt and put into array
        string[] customerLines = FileAndMenuHelperMethods.ReadFile(CustomersFile);
        string[] updatedLines = new string[customerLines.Length];
        
        // Loop through array for each customer
        for (int i = 0; i < customerLines.Length; i++)
        {
            // Split line into parts given pipe delimiter
            string[] parts = customerLines[i].Split('|');
            // If line matches customer id, update the count
            if (int.Parse(parts[0]) == customerId)
            {
                int currentBookings = int.Parse(parts[4]);  // Get current count
                int newBookings = Math.Max(currentBookings + d, 0); // Limit to prevent below 0 value.
                parts[4] = newBookings.ToString();
            }

            // Create new string from joining the parts
            updatedLines[i] = string.Join('|', parts);
        }
        
        // Write updated customer lines back to the customers.txt
        FileAndMenuHelperMethods.WriteFile(CustomersFile, updatedLines);
    }

    // Method to update flight passenger count displayed on view flights. 
    // 1st param takes the flight id to update and the second takes the change (+1 or -1).
    public static void UpdateFlightPassengerCount(int flightid, int d)
    {
        string[] flightLines = FileAndMenuHelperMethods.ReadFile(FlightsFile);
        string[] updatedLines = new string[flightLines.Length];

        for (int i = 0; i < flightLines.Length; i++)
        {
            string[] parts = flightLines[i].Split('|');
            if (int.Parse(parts[0]) == flightid)
            {
                parts[4] = (int.Parse(parts[4]) + d).ToString(); // Updating passanger count.
            }

            updatedLines[i] = string.Join('|', parts);
        }
        
        FileAndMenuHelperMethods.WriteFile(FlightsFile, updatedLines);
    }
    
}