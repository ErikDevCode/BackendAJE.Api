namespace BackEndAje.Api.Application.Users.Commands.UploadUsers
{
    using MediatR;

    public class UploadUsersCommand : IRequest<UploadUsersResult>
    {
        public byte[] FileBytes { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
