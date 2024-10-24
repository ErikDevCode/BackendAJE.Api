namespace BackEndAje.Api.Application.Positions.Queries.GetAllPositions
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllPositionsHandler : IRequestHandler<GetAllPositionsQuery, PaginatedResult<GetAllPositionsResult>>
    {
        private readonly IPositionRepository _positionRepository;
        private readonly IMapper _mapper;

        public GetAllPositionsHandler(IPositionRepository positionRepository, IMapper mapper)
        {
            this._positionRepository = positionRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<GetAllPositionsResult>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
        {
            var roles = await this._positionRepository.GetAllPaginatePositionsAsync(request.PageNumber, request.PageSize);
            var totalRoles = await this._positionRepository.GetTotalPositionsCountAsync();
            var result = this._mapper.Map<List<GetAllPositionsResult>>(roles);
            var paginatedResult = new PaginatedResult<GetAllPositionsResult>
            {
                TotalCount = totalRoles,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = result,
            };

            return paginatedResult;
        }
    }
}
