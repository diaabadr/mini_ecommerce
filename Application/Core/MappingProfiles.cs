using Application.DTOs;
using Application.Products.Commands;
using AutoMapper;
using Domain.Entities;
namespace Domain
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //TODO: there is an error on updating product
            CreateMap<Product, Product>().ForMember(m => m.Id, opt => opt.Ignore());
            CreateMap<Product, ProductDto>().ForMember(d => d.Suppliers, o => o.MapFrom(s => s.Suppliers))
            .ForMember(d => d.Creator, o => o.MapFrom(s => s.Creator));


            CreateMap<ProductSupplier, SupplierDto>().ForMember(d => d.Id, o => o.MapFrom(s => s.User.Id)).
            ForMember(d => d.Name, o => o.MapFrom(s => s.User.DisplayName));
            CreateMap<AddProductSupplier.Command, ProductSupplier>().
            ForMember(d => d.ProductId, o => o.MapFrom(r => r.ProductId)).
            ForMember(d => d.UserId, o => o.MapFrom(r => r.SupplierId));

            CreateMap<User, UserDto>().ForMember(d => d.Id, o => o.MapFrom(s => s.Id)).ForMember(d => d.Name, o => o.MapFrom(s => s.DisplayName));

            CreateMap<Review, ReviewDto>().ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id));
        }
    }
}
