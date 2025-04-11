using System;
using AutoMapper;
using WebRest.EF.Models;

namespace WebRestAPI.Code;

 public class MappingProfile : Profile {
     public MappingProfile() {
        CreateMap<Address, Address>();
        CreateMap<AddressType, AddressType>();
        CreateMap<CustomerAddress, CustomerAddress>();
        CreateMap<Customer, Customer>();
        CreateMap<Gender, Gender>();
        CreateMap<Orders, Orders>();
        CreateMap<OrdersLine, OrdersLine>();
        CreateMap<OrderState, OrderState>();
        CreateMap<OrderStatus, OrderStatus>();
        CreateMap<Product, Product>();
        CreateMap<ProductPrice, ProductPrice>();
        CreateMap<ProductStatus, ProductStatus>();
        
     }
 }
