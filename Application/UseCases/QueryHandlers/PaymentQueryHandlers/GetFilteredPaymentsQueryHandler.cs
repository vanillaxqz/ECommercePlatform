using Application.DTOs;
using Application.Utils;
using AutoMapper;
using Domain.Common;
using Domain.Repositories;
using MediatR;
using Gridify;
using Application.UseCases.Queries.PaymentQueries;

namespace Application.UseCases.QueryHandlers.PaymentQueryHandlers
{
    public class GetFilteredPaymentsQueryHandler(IPaymentRepository repository, IMapper mapper) :
        IRequestHandler<GetFilteredPaymentsQuery, Result<PagedResult<PaymentDto>>>
    {
        public async Task<Result<PagedResult<PaymentDto>>> Handle(GetFilteredPaymentsQuery request, CancellationToken cancellationToken)
        {
            var payments = await repository.GetAllPaymentsAsync();
            var query = payments.Data.AsQueryable();

            // Apply filters
            if (request.PaymentDate.HasValue)
            {
                query = query.Where(p => p.PaymentDate.Date == request.PaymentDate.Value.Date);
            }
            if (request.UserId.HasValue)
            {
                query = query.Where(p => p.UserId == request.UserId.Value);
            }

            // Apply paging
            var pagedPayments = query.ApplyPaging(request.Page, request.PageSize);
            var paymentDtos = mapper.Map<List<PaymentDto>>(pagedPayments);
            var pagedResult = new PagedResult<PaymentDto>(paymentDtos, query.Count());
            return Result<PagedResult<PaymentDto>>.Success(pagedResult);
        }
    }
}
