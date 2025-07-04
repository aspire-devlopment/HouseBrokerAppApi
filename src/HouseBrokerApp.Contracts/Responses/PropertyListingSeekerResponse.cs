using HouseBrokerApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApp.Contracts.Responses;

public class PropertyListingResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PropertyType PropertyType { get; set; }
    public decimal Price { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Features { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal? CommissionAmount { get; set; } 
    public DateTime CreatedAt { get; set; }

    public int? BrokerId { get; set; }

    public string? BrokerFirstName { get; set; }
    public string? BrokerLastName { get; set; }

    public string? BrokerEmail { get; set; }
    public string? BrokerPhoneNumber { get; set; }
}
