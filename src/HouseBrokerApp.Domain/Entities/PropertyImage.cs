using HouseBrokerApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApp.Domain.Entities
{
    public class PropertyImage :BaseEntity
    {
        public int PropertyListingId { get; set; }
        public string ImageUrl { get; set; } = null!;

        public PropertyListing PropertyListing { get; set; } = null!;
    }

}
