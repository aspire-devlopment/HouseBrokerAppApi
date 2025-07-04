using AutoMapper;
using HouseBrokerApp.Contracts.Requests;
using HouseBrokerApp.Contracts.Responses;
using HouseBrokerApp.Domain.Entities;

namespace HouseBrokerApp.Application.Mapper
{
    public class PropertyListingProfile : Profile
    {
        public PropertyListingProfile()
        {
            CreateMap<CreatePropertyListingRequest, PropertyListing>();
            CreateMap<UpdatePropertyListingRequest, PropertyListing>();
            CreateMap<PropertyListing, PropertyListingResponse>();
            CreateMap<PropertyListing, PropertyListingSeekerResponse>().ReverseMap();

    
        }
    }
}
