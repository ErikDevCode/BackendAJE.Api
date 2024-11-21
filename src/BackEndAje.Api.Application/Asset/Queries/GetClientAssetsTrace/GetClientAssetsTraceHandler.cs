namespace BackEndAje.Api.Application.Asset.Queries.GetClientAssetsTrace
{
    using AutoMapper;
    using BackEndAje.Api.Application.Abstractions.Common;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetClientAssetsTraceHandler : IRequestHandler<GetClientAssetsTraceQuery, PaginatedResult<GetClientAssetsTraceResult>>
    {
        private readonly IClientAssetRepository _clientAssetRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public GetClientAssetsTraceHandler(IClientAssetRepository clientAssetRepository, IMapper mapper, IClientRepository clientRepository)
        {
            this._clientAssetRepository = clientAssetRepository;
            this._mapper = mapper;
            this._clientRepository = clientRepository;
        }

        public async Task<PaginatedResult<GetClientAssetsTraceResult>> Handle(GetClientAssetsTraceQuery request, CancellationToken cancellationToken)
        {
            var traces = await this._clientAssetRepository.GetClientAssetTracesByAssetId(request.PageNumber, request.PageSize, request.AssetId);

            var totalTrace = await this._clientAssetRepository.GetTotalClientAssetsTrace(request.AssetId);
            var results = new List<GetClientAssetsTraceResult>();

            foreach (var trace in traces)
            {
                var traceResult = this._mapper.Map<GetClientAssetsTraceResult>(trace);
                if (trace.PreviousClientId.HasValue)
                {
                    var previousClient = await this._clientRepository.GetClientById(trace.PreviousClientId.Value);
                    traceResult.PreviousClientName = previousClient?.CompanyName ?? "N/A";
                }

                if (trace.NewClientId.HasValue)
                {
                    var newClient = await this._clientRepository.GetClientById(trace.NewClientId!.Value);
                    traceResult.NewClientName = newClient?.CompanyName ?? "N/A";
                }

                results.Add(traceResult);
            }

            var paginatedResult = new PaginatedResult<GetClientAssetsTraceResult>
            {
                TotalCount = totalTrace,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Items = results,
            };
            return paginatedResult;
        }
    }
}