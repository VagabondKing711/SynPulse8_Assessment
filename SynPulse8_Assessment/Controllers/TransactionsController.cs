using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SynPulse8_DataAccess.Interfaces;
using SynPulse8_Identity;

namespace SynPulse8_Assessment.Controllers
{
    [Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = UserClaimTypes.AccountStatus)]
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly ITransactionsProvider _transactionsProvider;

        public TransactionsController(
            ITransactionsProvider transactionsProvider
        )
        {
            _transactionsProvider = transactionsProvider;
        }

        [HttpGet("transactions/{id}")]
        public async Task<IActionResult> GetTransactionsById([FromRoute] string id)
        {
            var account = await _transactionsProvider.GetTransactionsByIdAsync(id).ConfigureAwait(false);

            return Ok(account);
        }

        [HttpGet("transactions/{accNum}")]
        public async Task<IActionResult> GetTransactionsByIBAN([FromRoute] string accNum)
        {
            var account = await _transactionsProvider.GetTransactionsByAccAsync(accNum).ConfigureAwait(false);

            return Ok(account);
        }
    }
}
