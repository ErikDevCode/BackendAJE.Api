namespace BackEndAje.Api.Application.Census.Queries.GetAnswerByClientId
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Entities;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAnswerByClientIdHandler : IRequestHandler<GetAnswerByClientIdQuery, PaginatedResult<ClientAssetWithCensusAnswersDto>>
    {
        private readonly ICensusRepository _censusRepository;
        private readonly IMapper _mapper;

        public GetAnswerByClientIdHandler(ICensusRepository censusRepository, IMapper mapper)
        {
            this._censusRepository = censusRepository;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<ClientAssetWithCensusAnswersDto>> Handle(GetAnswerByClientIdQuery request, CancellationToken cancellationToken)
        {
            var currentMonthPeriod = request.MonthPeriod ?? DateTime.Now.ToString("yyyyMM");

            // Obtener ClientAssets y sus respuestas de censo
            var (items, totalCount) = await this._censusRepository.GetClientAssetsWithCensusAnswersAsync(
                request.PageNumber,
                request.PageSize,
                request.AssetId,
                request.ClientId,
                currentMonthPeriod);

            // Mapear los resultados al DTO esperado
            var mappedResult = this._mapper.Map<List<ClientAssetWithCensusAnswersDto>>(items);

            // Calcular la paginación
            var paginatedResult = new PaginatedResult<ClientAssetWithCensusAnswersDto>
            {
                TotalCount = totalCount,
                PageNumber = request.PageNumber ?? 1,
                PageSize = request.PageSize ?? totalCount,
                Items = mappedResult,
            };

            return paginatedResult;
        }
    }
}