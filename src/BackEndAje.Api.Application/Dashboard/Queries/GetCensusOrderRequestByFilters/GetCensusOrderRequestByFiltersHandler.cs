namespace BackEndAje.Api.Application.Dashboard.Queries.GetCensusOrderRequestByFilters
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetCensusOrderRequestByFiltersHandler  : IRequestHandler<GetCensusOrderRequestByFiltersQuery, GetCensusOrderRequestByFiltersResult>
    {
        private readonly ICensusRepository _censusRepository;

        public GetCensusOrderRequestByFiltersHandler(ICensusRepository censusRepository)
        {
            this._censusRepository = censusRepository;
        }

        public async Task<GetCensusOrderRequestByFiltersResult> Handle(GetCensusOrderRequestByFiltersQuery request, CancellationToken cancellationToken)
        {
            var census = await this._censusRepository.GetCensusCountAsync(
                request.regionId,
                request.cediId,
                request.zoneId,
                request.route,
                request.month,
                request.year,
                request.UserId);

            var result = new GetCensusOrderRequestByFiltersResult
            {
                Type = "Censos",
                Value = census,
            };
            return result;
        }
    }
}
