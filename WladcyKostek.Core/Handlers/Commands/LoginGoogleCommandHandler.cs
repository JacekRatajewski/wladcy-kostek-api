using Google.Apis.Auth;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;
using WladcyKostek.Core.Requests;
using WladcyKostek.Core.Requests.Commands;

namespace WladcyKostek.Core.Handlers.Commands
{
    internal class LoginGoogleCommandHandler : IRequestHandler<LoginGoogleCommand, BaseResponse<bool>>
    {
        private readonly IUserRepository _userRepository;

        public LoginGoogleCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<bool>> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByTokenAsync(request.Token);
                var payload = await GoogleJsonWebSignature.ValidateAsync(request.Token, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { "785762715682-uf4drlm36f5ohrvgtbhd3aae7o3lcbhe.apps.googleusercontent.com" }
                });
                if (user is not null)
                {
                    if (payload is not null)
                        return BaseResponse<bool>.CreateResult(true);
                    return BaseResponse<bool>.CreateResult(false);
                }

                var newUser = new UserDTO
                {
                    Login = payload.Name,
                    Email = payload.Email,
                    FromGoogle = true,
                    Token = request.Token,
                };
                var created = await _userRepository.Register(newUser);
                if (created)
                    return BaseResponse<bool>.CreateResult(true);
                return BaseResponse<bool>.CreateResult(false);
            }
            catch (Exception ex)
            {
                return BaseResponse<bool>.CreateError(ex.Message);
            }
        }
    }
}
