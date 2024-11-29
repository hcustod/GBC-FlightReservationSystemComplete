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
    
    
    public static void UpdateCustomerBookingCount(int customerId, int d)
    {
        string[] customerLines = FileAndMenuHelperMethods.ReadFile(CustomersFile);
        string[] updatedLines = new string[customerLines.Length];
        
        for (int i = 0; i < customerLines.Length; i++)
        {
            string[] parts = customerLines[i].Split('|');
            if (int.Parse(parts[0]) == customerId)
            {
                int currentBookings = int.Parse(parts[4]);
                int newBookings = Math.Max(currentBookings + d, 0);
                parts[4] = newBookings.ToString();
            }

            updatedLines[i] = string.Join('|', parts);
        }
        
        FileAndMenuHelperMethods.WriteFile(CustomersFile, updatedLines);
    }

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