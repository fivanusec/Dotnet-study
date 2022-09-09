using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

using Dotnetrest.Data;
using Dotnetrest.Model;
using Dotnetrest.Dtos;
using Dotnetrest.Helper;
using Microsoft.AspNetCore.Authorization;

namespace Dotnetrest.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDatabaseContext _applicationDbContext;
        private IMapper _mapper;

        public AuthController(ApplicationDatabaseContext applicationDatabaseContext)
        {
            _applicationDbContext = applicationDatabaseContext;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(Logindto login)
        {
            var user = await _applicationDbContext.User.Where(_ => _.Email.ToLower() == login.Email.ToLower() && _.Password == login.Password && _.ExternalLoginName == null).FirstOrDefaultAsync();

            if (user == null)
                return BadRequest("Invalid credentials");

            var claims = new List<Claim>
            {
                new Claim("userid", user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties());

            return Ok("Success");
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Ok("Success");
        }

        [Authorize]
        [HttpGet]
        [Route("user-profile")]
        public async Task<IActionResult> UserProfileAsync()
        {
            int userId = HttpContext.User.Claims
            .Where(_ => _.Type == "userid")
            .Select(_ => Convert.ToInt32(_.Value))
            .First();

            var userProfile = await _applicationDbContext
            .User
            .Where(_ => _.Id == userId)
            .Select(_ => new UserDto
            {
                Id = _.Id,
                Email = _.Email,
                FirstName = _.FirstName,
                LastName = _.LastName
            }).FirstOrDefaultAsync();

            return Ok(userProfile);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto registerData)
        {
            if (_applicationDbContext.User.Any(x => x.Email == registerData.Email))
                throw new AppException("Email '" + registerData.Email + "' is already taken");

            // map model to new user object
            var user = _mapper.Map<UserModel>(registerData);

            // hash password
            user.PasswordHash = BCrypt.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }
}