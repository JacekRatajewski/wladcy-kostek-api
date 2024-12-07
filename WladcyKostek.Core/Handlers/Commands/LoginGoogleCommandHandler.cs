using Google.Apis.Auth;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;
using WladcyKostek.Core.Requests;
using WladcyKostek.Core.Requests.Commands;

namespace WladcyKostek.Core.Handlers.Commands
{
    internal class LoginGoogleCommandHandler : IRequestHandler<LoginGoogleCommand, BaseResponse<UserDTO?>>
    {
        private readonly IUserRepository _userRepository;

        public LoginGoogleCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<UserDTO?>> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByTokenAsync(request.Id);
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token);
                if (user is not null)
                {
                    if (payload is not null)
                        return BaseResponse<UserDTO?>.CreateResult(user);
                    return BaseResponse<UserDTO?>.CreateResult(null);
                }

                var newUser = new UserDTO
                {
                    Login = payload.Name,
                    Email = payload.Email,
                    FromGoogle = true,
                    AvatarUrl = payload.Picture,
                    Token = request.Id,
                };
                var createdUser = await _userRepository.Register(newUser);
                if (createdUser is not null)
                    return BaseResponse<UserDTO?>.CreateResult(createdUser);
                return BaseResponse<UserDTO?>.CreateResult(null);
            }
            catch (Exception ex)
            {
                return BaseResponse<UserDTO?>.CreateError(ex.Message);
            }
        }
    }
}
