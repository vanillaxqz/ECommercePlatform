using Application.DTOs;
using Application.UseCases.Commands.OrderCommands;
using Application.UseCases.Commands.ProductCommands;
using Application.UseCases.Commands.UserCommands;
using Application.UseCases.Commands.PaymentCommands;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Result<User>, Result<UserDto>>().ReverseMap();
            CreateMap<Result<IEnumerable<User>>, Result<IEnumerable<UserDto>>>().ReverseMap();
            CreateMap<CreateUserCommand, User>().ReverseMap();
            CreateMap<UpdateUserCommand, User>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Result<Product>, Result<ProductDto>>().ReverseMap();
            CreateMap<Result<IEnumerable<Product>>, Result<IEnumerable<ProductDto>>>().ReverseMap();
            CreateMap<CreateProductCommand, Product>().ReverseMap();
            CreateMap<UpdateProductCommand, Product>().ReverseMap();

            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Result<Payment>, Result<PaymentDto>>().ReverseMap();
            CreateMap<Result<IEnumerable<Payment>>, Result<IEnumerable<PaymentDto>>>().ReverseMap();
            CreateMap<CreatePaymentCommand, Payment>().ReverseMap();
            CreateMap<UpdatePaymentCommand, Payment>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Result<Order>, Result<OrderDto>>().ReverseMap();
            CreateMap<Result<IEnumerable<Order>>, Result<IEnumerable<OrderDto>>>().ReverseMap();
            CreateMap<CreateOrderCommand, Order>().ReverseMap();
            CreateMap<UpdateOrderCommand, Order>().ReverseMap();
        }
    }
}
