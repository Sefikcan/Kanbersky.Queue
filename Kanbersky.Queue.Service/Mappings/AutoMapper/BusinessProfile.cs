using AutoMapper;
using Kanbersky.Queue.Entities.Concrete;
using Kanbersky.Queue.Service.DTO.Request;
using Kanbersky.Queue.Service.DTO.Response;

namespace Kanbersky.Queue.Service.Mappings.AutoMapper
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<Customer, CreateCustomerRequest>().ReverseMap();
            CreateMap<Customer, CreateCustomerResponse>().ReverseMap();
        }
    }
}
