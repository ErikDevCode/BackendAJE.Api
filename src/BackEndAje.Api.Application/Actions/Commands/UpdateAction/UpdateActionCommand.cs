namespace BackEndAje.Api.Application.Actions.Commands.UpdateAction
{
    using BackEndAje.Api.Application.Dtos.Actions;
    using MediatR;

    public class UpdateActionCommand : IRequest<Unit>
    {
        public UpdateActionDto Action { get; set; }
    }
}
