using HouseBrokerApp.Contracts.Requests;
using HouseBrokerApp.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApp.Application.Interfaces;

public interface IPropertyListingService
{
    Task<int> CreateAsync(CreatePropertyListingRequest request, int brokerId);
    Task<bool> UpdateAsync(UpdatePropertyListingRequest request, int brokerId);
    Task<bool> DeleteAsync(int id, int brokerId);
    Task<List<PropertyListingSeekerResponse>> GetAllAsync(ClaimsPrincipal user);
    Task<List<PropertyListingSeekerResponse>> SearchAsync(string? location, decimal? minPrice, decimal? maxPrice, int? propertyType, ClaimsPrincipal user);

    Task<List<PropertyListingResponse>> GetAllByBrokerUserName(int brokerId);
    Task<PropertyListingResponse?> GetByIdAsync(int id, ClaimsPrincipal user);

}
