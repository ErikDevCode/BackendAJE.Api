namespace BackEndAje.Api.Application.Actions.Commands.UpdateAction
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateActionHandler : IRequestHandler<UpdateActionCommand, Unit>
    {
        private readonly IActionRepository _actionRepository;
        private readonly IMapper _mapper;

        public UpdateActionHandler(IActionRepository actionRepository, IMapper mapper)
        {
            this._actionRepository = actionRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateActionCommand request, CancellationToken cancellationToken)
        {
            var listActions = await this._actionRepository.GetAllActionsAsync();
            var existingAction = listActions.FirstOrDefault(action => action.ActionId == request.Action.ActionId);
            if (existingAction == null)
            {
                throw new InvalidOperationException($"Action con ID '{request.Action.ActionId}' no existe.");
            }

            this._mapper.Map(request.Action, existingAction);
            await this._actionRepository.UpdateActionAsync(existingAction);
            return Unit.Value;
        }
    }
}
