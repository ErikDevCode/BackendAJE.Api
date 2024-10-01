namespace BackEndAje.Api.Application.Actions.Queries.GetAllActions
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetAllActionsHandler : IRequestHandler<GetAllActionsQuery, List<GetAllActionsResult>>
    {
        private readonly IActionRepository _actionRepository;
        private readonly IMapper _mapper;

        public GetAllActionsHandler(IActionRepository actionRepository, IMapper mapper)
        {
            this._actionRepository = actionRepository;
            this._mapper = mapper;
        }

        public async Task<List<GetAllActionsResult>> Handle(GetAllActionsQuery request, CancellationToken cancellationToken)
        {
            var action = await this._actionRepository.GetAllActionsAsync();
            var result = this._mapper.Map<List<GetAllActionsResult>>(action);
            return result;
        }
    }
}