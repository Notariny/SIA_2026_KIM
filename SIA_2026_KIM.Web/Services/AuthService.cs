using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIA_2026_KIM.Shared.DModel;
using SIA_2026_KIM.Shared.Services;
using SIA_2026_KIM.Web.Data;
using SIA_2026_KIM.Web.Data.Entities;
using System.Security.Claims;

namespace SIA_2026_KIM.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDbContextFactory<DataContext> _contextFactory;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IDbContextFactory<DataContext> contextFactory, 
            IPasswordHasher<User> passwordHasher, IHttpContextAccessor httpContextAccessor)
        {
            _contextFactory = contextFactory;
            _passwordHasher = passwordHasher;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<MethodResult<LoggedInUser>> LoginAsync(LoginModel model)
        {
            var context = _contextFactory.CreateDbContext();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user is null)
                return MethodResult<LoggedInUser>.Fail("Account does not exist");


            var passwordVerificationResult =  _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            if (passwordVerificationResult != PasswordVerificationResult.Success)
                return MethodResult<LoggedInUser>.Fail("Incorrect Password");

            var loggedInUser = new LoggedInUser(user.Id, user.Name);

            return MethodResult<LoggedInUser>.Ok(loggedInUser);
        }


        public async Task<MethodResult> PlatformLoginAsync(LoggedInUser user)
        {
            //Login using Cookies in HttpContext
            var httpContext = _httpContextAccessor.HttpContext;

            Claim[] claims = [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
                ];
            var identity = new ClaimsIdentity(claims, WebConstants.WebAuthScheme);
            var principal = new ClaimsPrincipal(identity);

            await httpContext.SignInAsync(WebConstants.WebAuthScheme, principal);

            return MethodResult.Ok();
        }


        public async Task<MethodResult> RegisterAsync(RegisterModel model)
        {
               
               var context = _contextFactory.CreateDbContext();
               var isEmailExist = await context.Users.AnyAsync(u => u.Email == model.Email);
               if(isEmailExist)
               {
                return MethodResult.Fail("Email exists already");
               }

               var user = new User
               {
                Name = model.Name,
                Email = model.Email,
               };

               user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

               context.Users.Add(user);
               await context.SaveChangesAsync();


               return MethodResult.Ok();
              
        }


        public async Task<MethodResult> EventAsync(EventModel model)
        {
            var context = _contextFactory.CreateDbContext();

            var newEvent = new Event
            {
                Title = model.Title,
                Date = model.Date,
                Time = model.Time,
                Venue = model.Venue,
                Details = model.Details
            };

            context.Event.Add(newEvent);
            await context.SaveChangesAsync();

            return MethodResult.Ok();
        }


    }
}
