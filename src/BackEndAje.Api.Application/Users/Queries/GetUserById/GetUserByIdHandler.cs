namespace BackEndAje.Api.Application.Users.Queries.GetUserById
{
    using AutoMapper;
    using BackEndAje.Api.Domain.Repositories;
    using MediatR;

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, GetUserByIdResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var users = await this._userRepository.GetUserByIdAsync(request.userId);
            if (users == null)
            {
                throw new KeyNotFoundException($"USuario con ID '{request.userId}' no existe.");
            }

            var result = this._mapper.Map<GetUserByIdResult>(users);
            return result;
        }
    }
}
