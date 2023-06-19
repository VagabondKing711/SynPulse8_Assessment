using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SynPulse8_Abstractions;
using SynPulse8_DataAccess.Interfaces;
using SynPulse8_Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynPulse8_Assessment.Controllers
{
    [Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Policy = UserClaimTypes.AccountStatus)]
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerProvider _customerProvider;

        public CustomerController(
            ICustomerProvider customerProvider
        )
        {
            _customerProvider = customerProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomer()
        {
            string customerId = HttpContext.User.Claims.Where(c => c.Type == UserClaimTypes.CustomerId)
                                                .Select(c => c.Value).SingleOrDefault();

            var customer = await _customerProvider.GetCustomerByIdAsync(customerId).ConfigureAwait(false);

            return Ok(customer);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetCustomerList()
        {

            var customers = await _customerProvider.GetCustomerListAsync().ConfigureAwait(false);

            return Ok(customers);
        }
    }
}
