/*
 *  Group Members:
 *  Henrique Custodio (101497015)
 *  Fitsum Asgedom (101510623)
 *  Jinah Ahn (100902591) 
 *
 */

using System;
using System.IO;
using System.Collections.Generic;
using FlightReservationSystemProject;

// ****** Initial Menu and creation/use of submenu objects. *******
class Program
{
    public const string RESET = "\u001B[0m";
    public const string RED = "\u001B[31m";
    public const string GREEN = "\u001B[32m";
    public const string YELLOW = "\u001B[33m";
    public const string CYAN = "\u001B[36m";

    static void Main(string[] args)
    {
        
        
        CustomerMenu customerMenu = new CustomerMenu();
        FlightMenu flightMenu = new FlightMenu();
        BookingMenu bookingMenu = new BookingMenu();
        
        bool RUNNING = true;

        while (RUNNING)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(YELLOW + "  ╔═════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("  ║              Welcome to Extreme Flight Reservation System!              ║");
            Console.WriteLine("  ╚═════════════════════════════════════════════════════════════════════════╝" + RESET);
           
            Console.WriteLine(GREEN+"\n 1. Customers.");
            Console.WriteLine("\n 2. Flights.");
            Console.WriteLine("\n 3. Bookings. "+RESET);
            Console.WriteLine(RED+"\n 4. Exit."+RESET);
            Console.Write(CYAN+"\nSelect an option by entering 1-4: "+RESET);
            
            // ? is for the conversion of null literal and/or value into non-nullable type. 
            string userChoice = Console.ReadLine()?.Trim();
            switch (userChoice)
            {
                case "1":
                    customerMenu.DisplayCustomerMenu();
                    break;
                case "2":
                    flightMenu.ShowMenu();
                    break;
                case "3":
                    bookingMenu.ShowMenu();
                    break;
                case "4":
                    if (ConfirmExit())
                    {
                        RUNNING = false;
                    }
                    Console.WriteLine(RED+"Exiting the Program, Thanks for Visting... "+RESET);
                    break;
                default:
                    Console.WriteLine(RED+"Invalid option please select an option by inputting a number between 1-4."+RESET);
                    break;
            }
            
            static bool ConfirmExit()
            {
                Console.Write(RED+"Exit the Program? (Y/N): "+RESET);
                // ? is for the conversion of null literal and/or value into non-nullable type.
                string confirmation = Console.ReadLine()?.Trim().ToUpper() ?? "N";
               return confirmation.Equals("Y");
                
            }
        }
    }
}




