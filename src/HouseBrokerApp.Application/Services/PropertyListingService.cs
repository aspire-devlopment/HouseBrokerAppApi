using AutoMapper;
using HouseBrokerApp.Application.Interfaces;
using HouseBrokerApp.Application.IRepository;
using HouseBrokerApp.Contracts.Requests;
using HouseBrokerApp.Contracts.Responses;
using HouseBrokerApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace HouseBrokerApp.Application.Services;

public class PropertyListingService : IPropertyListingService
{
    private readonly IGenericRepository<PropertyListing> _repository;
    private readonly IGenericRepository<ApplicationUser> _userRepository;   
    private readonly ICommissionService _commissionService;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    public PropertyListingService(
        IGenericRepository<PropertyListing> repository,
        ICommissionService commissionService,
        IMapper mapper, IMemoryCache cache, 
        IGenericRepository<ApplicationUser> userRepository)
    {
        _repository = repository;
        _commissionService = commissionService;
        _mapper = mapper;
        _cache = cache;
        _userRepository = userRepository;
    }

    public async Task<int> CreateAsync(CreatePropertyListingRequest request, int brokerId)
    {
        var entity = new PropertyListing
        {
            Title = request.Title,
            Description = request.Description,
            PropertyType = request.PropertyType,
            Price = request.Price,
            Location = request.Location,
            Features = request.Features,
            BrokerId = brokerId,
            CommissionAmount = _commissionService.CalculateCommission(request.Price),
            Images = new List<PropertyImage>()
        };


        if (request.Images is { Count: > 0 })
        {
            var imagePaths = await SaveImagesAsync(request.Images);
            entity.Images = imagePaths.Select(path => new PropertyImage { ImageUrl = path }).ToList();
        }

        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();
        return entity.Id;
    }
    public async Task<PropertyListingResponse?> GetByIdAsync(int id, ClaimsPrincipal user)
    {
        var listing = await _repository.GetByIdAsync(id);
        if (listing == null) return null;

        var response = _mapper.Map<PropertyListingResponse>(listing);

        // Seeker user: hide commission and include broker contact info
        if (!user.IsInRole("Broker"))
        {
            response.CommissionAmount = null;
            var brokerData = await _userRepository.FindAsync(x => x.BrokerId == response.BrokerId);

            var brokerDetail = brokerData.FirstOrDefault();
           
            response.BrokerFirstName = brokerDetail.FirstName;       
            response.BrokerLastName = brokerDetail.LastName;
            response.BrokerPhoneNumber = brokerDetail.PhoneNumber;
            response.BrokerEmail = brokerDetail.Email;
        }

        return response;
    }

    //Get the List of Records Posted By Broker.

    public async Task<List<PropertyListingResponse>> GetAllByBrokerUserName(int brokerId)
    {
        var listing = await _repository.FindAsync(x => x.BrokerId == brokerId);

        var response = _mapper.Map<List<PropertyListingResponse>>(listing);


        return response;
    }

    //Get the Full Records For the Seekers Posted By All Brokes

    public async Task<List<PropertyListingSeekerResponse>> GetAllAsync(ClaimsPrincipal user)
    {
      var entities = await _repository.GetAllAsync();

      return _mapper.Map<List<PropertyListingSeekerResponse>>(entities);
      
        
    }


    public async Task<List<PropertyListingSeekerResponse>> SearchAsync(
        string? location, decimal? minPrice, decimal? maxPrice, int? propertyType, ClaimsPrincipal user)
    {
        // Generate a unique cache key based on the query params
        var cacheKey = $"PropertySearch_{location}_{minPrice}_{maxPrice}_{propertyType}";

        // Try to get from cache
        if (_cache.TryGetValue(cacheKey, out List<PropertyListingSeekerResponse> cachedResult))
        {
            return cachedResult!;
        }

        // If not cached, fetch from DB
        var query = await _repository.FindAsync(x =>
            (string.IsNullOrEmpty(location) || x.Location.Contains(location)) &&
            (!minPrice.HasValue || x.Price >= minPrice.Value) &&
            (!maxPrice.HasValue || x.Price <= maxPrice.Value) &&
            (!propertyType.HasValue || (int)x.PropertyType == propertyType.Value)
        );

        var response = _mapper.Map<List<PropertyListingSeekerResponse>>(query);

        // Cache the result for 5 minutes
        _cache.Set(cacheKey, response, TimeSpan.FromMinutes(5));

        return response;
    }



    public async Task<bool> DeleteAsync(int id, int brokerId)
    {
        var listing = await _repository.FindAsync(x => x.Id == id && x.BrokerId == brokerId);
        var entity = listing.FirstOrDefault();
        if (entity == null) return false;

        await _repository.DeleteAsync(entity);
        await _repository.SaveChangesAsync();
        return true;
    }


    public async Task<bool> UpdateAsync(UpdatePropertyListingRequest request, int brokerId)
    {
        var existing = await _repository.GetByIdAsync(request.Id);
        if (existing == null || existing.BrokerId != brokerId) return false;

        existing.Title = request.Title;
        existing.Description = request.Description;
        existing.PropertyType = request.PropertyType;
        existing.Price = request.Price;
        existing.Location = request.Location;
        existing.Features = request.Features;
        existing.CommissionAmount = _commissionService.CalculateCommission(existing.Price);

        if (request.NewImages is { Count: > 0 })
        {
            var paths = await SaveImagesAsync(request.NewImages);
            existing.Images.AddRange(paths.Select(p => new PropertyImage { ImageUrl = p }));
        }
        await _repository.UpdateAsync(existing);
        await _repository.SaveChangesAsync();
        return true;
    }

    // Method to Save the Image Uploaded while Inserting The Record In the Property
    private async Task<List<string>> SaveImagesAsync(IFormFileCollection images)
    {
        var paths = new List<string>();
        var folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

        foreach (var image in images)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var path = Path.Combine(folder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await image.CopyToAsync(stream);

            paths.Add($"/Images/{fileName}");
        }

        return paths;
    }

}
