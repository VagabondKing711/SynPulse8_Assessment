using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SynPulse8_Abstractions;
using SynPulse8_DataAccess.Interfaces;
using SynPulse8_Identity;

namespace SynPulse8_Assessment.Controllers
{
    [Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = UserClaimTypes.AccountStatus)]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountProvider _accountProvider;

        public AccountController(
            IAccountProvider accountProvider
        )
        {
            _accountProvider = accountProvider;
        }

        [HttpGet("transactions/{accNum}")]
        public async Task<IActionResult> GetAccountByIBAN([FromRoute] string accNum) 
        {
            var account = await _accountProvider.GetAccountByIBANasync(accNum).ConfigureAwait(false);

            return Ok(account);
        }

        [HttpGet("transactions/{id}")]
        public async Task<IActionResult> GetAccountById([FromRoute] string id)
        {
            var account = await _accountProvider.GetAccountListByIdAsync(id).ConfigureAwait(false);

            return Ok(account);
        }
    }
}
