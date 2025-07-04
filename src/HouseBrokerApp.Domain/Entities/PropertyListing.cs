using HouseBrokerApp.Domain.Common;
using HouseBrokerApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApp.Domain.Entities;

public class PropertyListing : BaseEntity
{

    public string? Title { get; set; }
    public string? Description { get; set; }
    public PropertyType PropertyType { get; set; }
    public decimal Price { get; set; }
    public string? Location { get; set; }
    public string? Features { get; set; }
    public int BrokerId { get; set; }
    public decimal CommissionAmount { get; set; }
    public List<PropertyImage>? Images { get; set; } 



}
