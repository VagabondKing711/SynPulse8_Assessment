using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynPulse8_Abstractions;
using SynPulse8_DataAccess.Interfaces;
using SynPulse8_Identity;
using static SynPulse8_Abstractions.User;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;

namespace SynPulse8_Assessment.Controllers
{
    [Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = UserClaimTypes.AccountStatus)]
    [ApiController]
    [Route("[controller]")]
    public class IdentityController : Controller
    {
        private readonly IUserProvider _userProvider;
        private readonly ILogger<IdentityController> _logger;
        private readonly ICustomerProvider _customerProvider;
        private IConfiguration _configuration;

        public IdentityController(
            ILogger<IdentityController> logger,
            IUserProvider userProvider,
            IConfiguration configuration,
            ICustomerProvider customerProvider
        )
        {
            _logger = logger;
            _userProvider = userProvider;
            _configuration = configuration;
            _customerProvider = customerProvider;
        }

        [AllowAnonymous]
        [HttpGet("accesstoken/user/{emailAddress}")]
        public async Task<IActionResult> GetAccessTokenFromUserAsync([FromRoute] string emailAddress, [FromQuery] string pwd)
        {
            // Try to get user
            User user = await _userProvider.GetUserByEmailAsync(emailAddress).ConfigureAwait(false);
            if (user == null || emailAddress == null || pwd == null)
            {
                return BadRequest(new BaseErrorResponse<string>()
                {
                    ErrorCode = SynPulse8_Identity.StatusCode.Unauthorized
                });
            }

            // Get current customer
            Customer customer = await _customerProvider.GetCustomerByIdAsync(user.CustomerId).ConfigureAwait(false);

            // Get password hash
            string passwordHash = await _userProvider.GetPasswordHashAsync(user.Id).ConfigureAwait(false);
            PasswordHasher<User> ph = new PasswordHasher<User>();

            if (ph.VerifyHashedPassword(user, passwordHash, pwd) != PasswordVerificationResult.Success)
            {
                return BadRequest(new BaseErrorResponse<string>()
                {
                    ErrorCode = SynPulse8_Identity.StatusCode.Unauthorized
                });
            }

            if (user.Status != AccountStatus.Active)
            {
                return BadRequest(new BaseErrorResponse<AccountStatus>()
                {
                    ErrorCode = SynPulse8_Identity.StatusCode.Forbidden,
                    Data = user.Status
                });
            }

            List<Claim> list = new List<Claim>();
            list.Add(new Claim(UserClaimTypes.AccountStatus, user.Status.ToString()));
            list.Add(new Claim(JwtRegisteredClaimNames.Sub, user.EMailAddress));
            list.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            list.Add(new Claim(UserClaimTypes.UserId, user.Id));
            list.Add(new Claim(UserClaimTypes.CustomerId, user.CustomerId));

            var token = IdentityConfigurator.CreateAccessToken(list);
            return Ok(token);
        }

        [HttpGet("accesstoken/user")]
        public async Task<IActionResult> GetRefreshToken()
        {
            string userId = HttpContext.User.Claims.Where(c => c.Type == UserClaimTypes.UserId)
                                                    .Select(c => c.Value).SingleOrDefault();

            var user = await _userProvider.GetUserByIdAsync(userId).ConfigureAwait(false);

            // Check if the user's account is active
            if (user.Status != AccountStatus.Active)
            {
                return BadRequest(new BaseErrorResponse<AccountStatus>()
                {
                    ErrorCode = SynPulse8_Identity.StatusCode.Forbidden,
                    Data = user.Status
                });
            }

            Customer customer = await _customerProvider.GetCustomerByIdAsync(user.CustomerId).ConfigureAwait(false);

            // User is Validated
            List<Claim> list = new List<Claim>();
            list.Add(new Claim(UserClaimTypes.AccountStatus, user.Status.ToString()));
            list.Add(new Claim(JwtRegisteredClaimNames.Sub, user.EMailAddress));
            list.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            list.Add(new Claim(UserClaimTypes.UserId, user.Id));
            list.Add(new Claim(UserClaimTypes.CustomerId, user.CustomerId));

            var token = IdentityConfigurator.CreateAccessToken(list);
            return Ok(token);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserAsync()
        {
            // First check if the id is for the currently logged in user
            string userId = HttpContext.User.Claims.Where(c => c.Type == UserClaimTypes.UserId)
                                                .Select(c => c.Value).SingleOrDefault();

            if (userId == null)
            {
                return BadRequest(new BaseErrorResponse<string>()
                {
                    ErrorCode = SynPulse8_Identity.StatusCode.Forbidden,
                    ErrorMsg = "User id does match logged in user"
                });
            }

            User user = await _userProvider.GetUserByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return BadRequest(new BaseErrorResponse<string>()
                {
                    ErrorCode = SynPulse8_Identity.StatusCode.NotFound,
                    ErrorMsg = "User not found"
                });
            }

            return Ok(user);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync()
        {
            var list = await _userProvider.GetUsersAsync().ConfigureAwait(false);
            return Ok(list);
        }
    }
}
