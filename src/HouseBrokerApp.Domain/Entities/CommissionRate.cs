using HouseBrokerApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApp.Domain.Entities;

public class CommissionRate : BaseEntity
{
    public decimal? MinPrice { get; set; }  // Optional: null for "no lower limit"
    public decimal? MaxPrice { get; set; }  // Optional: null for "no upper limit"
    public decimal? Rate { get; set; }       // e.g., 0.02 for 2%
   
}
