namespace BackEndAje.Api.Application.Clients.Queries.GetAllClients
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Application.Dtos.Cedi;
    using BackEndAje.Api.Application.Dtos.DocumentType;
    using BackEndAje.Api.Application.Dtos.PaymentMethod;
    using BackEndAje.Api.Application.Dtos.Users;
    using BackEndAje.Api.Application.Dtos.Zone;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllClientsHandler : IRequestHandler<GetAllClientsQuery, PaginatedResult<GetAllClientsResult>>
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUbigeoRepository _ubigeoRepository;
        private readonly ICediRepository _cediRepository;
        private readonly IZoneRepository _zoneRepository;
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetAllClientsHandler(IClientRepository clientRepository, IMapper mapper, IUserRepository userRepository, IUbigeoRepository ubigeoRepository, ICediRepository cediRepository, IZoneRepository zoneRepository, IMastersRepository mastersRepository)
        {
            this._clientRepository = clientRepository;
            this._mapper = mapper;
            this._userRepository = userRepository;
            this._ubigeoRepository = ubigeoRepository;
            this._cediRepository = cediRepository;
            this._zoneRepository = zoneRepository;
            this._mastersRepository = mastersRepository;
        }

        public async Task<PaginatedResult<GetAllClientsResult>> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var clients = await this._clientRepository.GetClients(request.PageNumber, request.PageSize);

            var result = this._mapper.Map<List<GetAllClientsResult>>(clients);
            foreach (var clientResult in result)
            {
                var user = await this._userRepository.GetUserByEmailOrRouteAsync(clientResult.Route.ToString());
                var userDto = this._mapper.Map<UserDto>(user);
                clientResult.Seller = userDto;

                var documentType = await this._mastersRepository.GetDocumentTypeById(clientResult.DocumentTypeId);
                var documentTypeDto = this._mapper.Map<DocumentTypeDto>(documentType);
                clientResult.DocumentType = documentTypeDto;
                var paymentMethod = await this._mastersRepository.GetPaymentMethodById(clientResult.PaymentMethodId);
                var paymentMethodDto = this._mapper.Map<PaymentMethodDto>(paymentMethod);
                clientResult.PaymentMethod = paymentMethodDto;
                var cedi = await this._cediRepository.GetCediByCediIdAsync(userDto.CediId);
                var cediDto = this._mapper.Map<CediDto>(cedi);
                clientResult.Seller.Cedi = cediDto;
                var zone = await this._zoneRepository.GetZoneByZoneIdAsync(userDto.ZoneId);
                var zoneDto = this._mapper.Map<ZoneDto>(zone);
                clientResult.Seller.Zone = zoneDto;
                var district = await this._ubigeoRepository.GetDistrictByDistrictId(clientResult.DistrictId);
                clientResult.District = district!;
            }

            var totalClients = await this._clientRepository.GetTotalClients();

            var paginatedResult = new PaginatedResult<GetAllClientsResult>
            {
                TotalCount = totalClients,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = result,
            };

            return paginatedResult;
        }
    }
}
