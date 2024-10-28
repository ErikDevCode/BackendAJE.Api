namespace BackEndAje.Api.Application.Actions.Commands.CreateAction
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;
    using Action = BackEndAje.Api.Domain.Entities.Action;

    public class CreateActionHandler : IRequestHandler<CreateActionCommand, Unit>
    {
        private readonly IActionRepository _actionRepository;
        private readonly IMapper _mapper;

        public CreateActionHandler(IActionRepository actionRepository, IMapper mapper)
        {
            this._actionRepository = actionRepository;
            this._mapper = mapper;
        }

        public async Task<Unit> Handle(CreateActionCommand request, CancellationToken cancellationToken)
        {
            var listActions = await this._actionRepository.GetAllActionsAsync();
            var existingAction = listActions.FirstOrDefault(r => r.ActionName.Equals(request.Action.ActionName, StringComparison.OrdinalIgnoreCase));

            if (existingAction != null)
            {
                throw new InvalidOperationException($"Action '{request.Action.ActionName}' ya existe.");
            }

            var newAction = this._mapper.Map<Action>(request.Action);
            await this._actionRepository.AddActionAsync(newAction);
            return Unit.Value;
        }
    }
}
