namespace BackEndAje.Api.Application.Users.Commands.UpdateStatusUser
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UpdateStatusUserCommand : IRequest<bool>, IHasUpdatedByInfo
    {
        public int UserId { get; set; }

        public int UpdatedBy { get; set; }
    }
}

