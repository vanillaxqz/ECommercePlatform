using Application.DTOs;
using Application.UseCases.Queries.PaymentQueries;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;

namespace Application.UseCases.QueryHandlers.PaymentQueryHandlers
{
    public class GetPaymentByUserIdQueryHandler : IRequestHandler<GetPaymentByUserIdQuery, Result<IEnumerable<PaymentDto>>>
    {
        private readonly IPaymentRepository repository;
        private readonly IMapper mapper;
        public GetPaymentByUserIdQueryHandler(IPaymentRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<Result<IEnumerable<PaymentDto>>> Handle(GetPaymentByUserIdQuery request, CancellationToken cancellationToken)
        {
            var payments = await repository.GetPaymentsByUserIdAsync(request.Id);
            return mapper.Map<Result<IEnumerable<PaymentDto>>>(payments);
        }
    }
}
