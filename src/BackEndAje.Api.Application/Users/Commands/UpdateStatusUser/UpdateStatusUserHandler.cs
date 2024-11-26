namespace BackEndAje.Api.Application.Users.Commands.UpdateStatusUser
{
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class UpdateStatusUserHandler : IRequestHandler<UpdateStatusUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateStatusUserHandler(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateStatusUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await this._userRepository.GetUserByIdAsync(request.UserId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Usuario con ID '{request.UserId}' no existe.");
            }

            existingUser.IsActive = existingUser.IsActive is false;
            existingUser.UpdatedBy = request.UpdatedBy;
            await this._userRepository.UpdateUserAsync(existingUser);
            return true;
        }
    }
}

