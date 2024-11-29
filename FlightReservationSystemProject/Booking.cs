namespace FlightReservationSystemProject;

public class Booking
{
    private static int bookingNumCounter = 1;
    
    private int bookingNum;
    private string bookingDate;
    private CustomerAcc customer;
    private Flight flight;

    public Booking(CustomerAcc customer, Flight flight)
    {
        this.bookingNum = bookingNumCounter++;
        this.bookingDate = DateTime.Now.ToString("MM/dd/yyyy hh:mm tt");
        this.customer = customer;
        this.flight = flight;
        
        customer.AddBookingCount();
        flight.AddPassangerToFlight();
    }
    
    public override string ToString()
    {
        return
            $"Booking Number: {bookingNum}, Date: {bookingDate}, Customer: {customer.CustomerFirstName} {customer.CustomerLastName}, Flight: {flight.FlightNum}";
    }
    
}
