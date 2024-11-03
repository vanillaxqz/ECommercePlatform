using Application.DTOs;
using Application.UseCases.Queries.PaymentQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.PaymentQueryHandlers
{
    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Result<PaymentDto>>
    {
        private readonly IPaymentRepository repository;
        private readonly IMapper mapper;
        public GetPaymentByIdQueryHandler(IPaymentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<PaymentDto>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = await repository.GetPaymentByIdAsync(request.Id);
            return mapper.Map<Result<PaymentDto>>(payment);
        }
    }
}
