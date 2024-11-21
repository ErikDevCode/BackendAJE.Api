namespace BackEndAje.Api.Application.Users.Commands.UploadUsers
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class UploadUsersCommand : IRequest<UploadUsersResult>, IHasAuditInfo
    {
        public byte[] FileBytes { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
