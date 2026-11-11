using SIA_2026_KIM.Shared.DModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIA_2026_KIM.Shared.Services
{
public  interface IAuthService
    {
        Task<MethodResult> RegisterAsync(RegisterModel model);

        Task<MethodResult<LoggedInUser>> LoginAsync(LoginModel model);

        Task<MethodResult> EventAsync(EventModel model);

        Task<MethodResult> PlatformLoginAsync(LoggedInUser user);

        Task<MethodResult> PlatformLogoutAsync();
    }
}
