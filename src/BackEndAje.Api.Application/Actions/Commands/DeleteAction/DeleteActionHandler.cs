namespace BackEndAje.Api.Application.Actions.Commands.DeleteAction
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class DeleteActionHandler : IRequestHandler<DeleteActionCommand, bool>
    {
        private readonly IActionRepository _actionRepository;

        public DeleteActionHandler(IActionRepository actionRepository)
        {
            this._actionRepository = actionRepository;
        }

        public async Task<bool> Handle(DeleteActionCommand request, CancellationToken cancellationToken)
        {
            var existingAction = await this._actionRepository.GetActionByIdAsync(request.DeleteAction.ActionId);
            if (existingAction == null)
            {
                throw new InvalidOperationException($"ActionId '{request.DeleteAction.ActionId}' not exists.");
            }

            await this._actionRepository.DeleteActionAsync(existingAction);
            return true;
        }
    }
}
