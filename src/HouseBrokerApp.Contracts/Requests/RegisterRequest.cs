using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApp.Contracts.Requests;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }

    public string PhoneNumber { get; set; }

    public string Address { get; set; }
    public string LastName { get; set; }

    public string UserType { get; set; }
}
