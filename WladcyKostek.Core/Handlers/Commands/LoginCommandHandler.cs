using Google.Apis.Auth;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;
using WladcyKostek.Core.Requests;
using WladcyKostek.Core.Requests.Commands;

namespace WladcyKostek.Core.Handlers.Commands
{
    internal class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse<UserDTO?>>
    {
        private readonly IUserRepository _userRepository;

        public LoginCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<UserDTO?>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetUserByLoginAsync(request.Login);
                if (user is not null)
                {
                    if (user.Password == request.Password)
                    {
                        return BaseResponse<UserDTO?>.CreateResult(user);
                    }
                }
                return BaseResponse<UserDTO?>.CreateResult(null);
            }
            catch (Exception ex)
            {
                return BaseResponse<UserDTO?>.CreateError(ex.Message);
            }
        }
    }
}
