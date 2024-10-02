namespace BackEndAje.Api.Application.Actions.Commands.DeleteAction
{
    using BackEndAje.Api.Application.Dtos.Actions;
    using MediatR;

    public class DeleteActionCommand : IRequest<bool>
    {
        public DeleteActionDto DeleteAction { get; set; }
    }
}
