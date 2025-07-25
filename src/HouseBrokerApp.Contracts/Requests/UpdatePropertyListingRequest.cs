﻿using HouseBrokerApp.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApp.Contracts.Requests;

public class UpdatePropertyListingRequest
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public PropertyType PropertyType { get; set; }
    public decimal Price { get; set; }
    public string? Location { get; set; }
    public string? Features { get; set; }
    public IFormFileCollection? NewImages { get; set; }
}
