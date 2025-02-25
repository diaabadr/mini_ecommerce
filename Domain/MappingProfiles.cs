using AutoMapper;
using Domain.Entities;
namespace Domain
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, Product>().ForMember(m => m.Id, opt => opt.Ignore());
        }
    }
}
