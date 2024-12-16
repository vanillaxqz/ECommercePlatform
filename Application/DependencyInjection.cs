using Application.UseCases.Commands.OrderCommands;
using Application.UseCases.Commands.PaymentCommands;
using Application.UseCases.Commands.ProductCommands;
using Application.UseCases.Commands.UserCommands;
using Application.Utils;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateOrderCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<DeleteOrderCommandValidator>();

            services.AddValidatorsFromAssemblyContaining<CreatePaymentCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdatePaymentCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<DeletePaymentCommandValidator>();

            services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateProductCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<DeleteProductCommandValidator>();

            services.AddValidatorsFromAssemblyContaining<CreateUserCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateUserCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<DeleteUserCommandValidator>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddHttpContextAccessor();
            return services;
        }
    }
}
