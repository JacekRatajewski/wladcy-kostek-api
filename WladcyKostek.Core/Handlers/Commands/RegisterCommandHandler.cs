using Google.Apis.Auth;
using WladcyKostek.Core.Interfaces;
using WladcyKostek.Core.Models;
using WladcyKostek.Core.Requests;
using WladcyKostek.Core.Requests.Commands;

namespace WladcyKostek.Core.Handlers.Commands
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, BaseResponse<bool>>
    {
        private readonly IUserRepository _userRepository;

        public RegisterCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var newUser = new UserDTO
                {
                    Login = request.Login,
                    Email = request.Email,
                    Password = request.Password,
                    FromGoogle = false
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
