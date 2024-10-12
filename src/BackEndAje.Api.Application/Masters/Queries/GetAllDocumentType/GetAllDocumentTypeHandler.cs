namespace BackEndAje.Api.Application.Masters.Queries.GetAllDocumentType
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllDocumentTypeHandler : IRequestHandler<GetAllDocumentTypeQuery, List<GetAllDocumentTypeResult>>
    {
        private readonly IMastersRepository _mastersRepository;
        private readonly IMapper _mapper;

        public GetAllDocumentTypeHandler(IMastersRepository mastersRepository, IMapper mapper)
        {
            this._mastersRepository = mastersRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllDocumentTypeResult>> Handle(GetAllDocumentTypeQuery request, CancellationToken cancellationToken)
        {
            var documentTypes = await this._mastersRepository.GetAllDocumentType();
            var result = this._mapper.Map<List<GetAllDocumentTypeResult>>(documentTypes);
            return result;
        }
    }
}
