namespace FlightReservationSystemProject;

using System; 
using System.Linq;
// The System.Linq namespace provides methods Where, Select, and ToArray for querying and manipulating data/arrays. 

// Data structure
public class CustomerAcc
{
    private int customerID;
    private string customerFirstName;
    private string customerLastName;
    private string customerPhoneNum;
    private int customerNumOfBookings;
    
    public int CustomerID
    {
        get { return customerID; }
    }

    public string CustomerFirstName
    {
        get
        {
            return customerFirstName;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("First name cannot be empty");
            }

            customerFirstName = value.Trim();
        }
    }

    public string CustomerLastName
    {
        get
        {
            return customerLastName;
        }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Last name cannot be empty.");
            }

            customerLastName = value.Trim();
        }
    }

    public string PhoneNumber
    {
        get
        {
            return customerPhoneNum;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new AggregateException("Phone number cannot be empty or null.");
            }

            customerPhoneNum = CleanPhonenumber(value);
        }
    }

    public int CustomerNumOfBookings
    {
        get
        {
            return customerNumOfBookings;
        }
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Number of bokings cannot be negative");
            }

            customerNumOfBookings = value;
        }
    }
    
    private string CleanPhonenumber(string customerphone_p)
    {
        return string.Concat(PhoneNumber.Where(char.IsDigit));
    }
    
    public void AddBookingCount()
    {
        customerNumOfBookings++;
    }

    
    // Constructor for Customer account object
    public CustomerAcc( int customerID,string customerFirstName_p, string customerLastName_p, string customerPhoneNum_p)
    {
        this.customerID = customerID;
        CustomerFirstName = customerFirstName_p;
        customerLastName = customerLastName_p;
        customerPhoneNum = customerPhoneNum_p;
        customerNumOfBookings = 0;
    }
}
