namespace BackEndAje.Api.Application.Masters.Queries.GetAllPaymentMethod
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllPaymentMethodHandler : IRequestHandler<GetAllPaymentMethodQuery, List<GetAllPaymentMethodResult>>
    {
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetAllPaymentMethodHandler(IMastersRepository mastersRepository, IMapper mapper)
        {
            this._mastersRepository = mastersRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllPaymentMethodResult>> Handle(GetAllPaymentMethodQuery request, CancellationToken cancellationToken)
        {
            var paymentMethods = await this._mastersRepository.GetAllPaymentMethods();
            var result = this._mapper.Map<List<GetAllPaymentMethodResult>>(paymentMethods);
            return result;
        }
    }
}
