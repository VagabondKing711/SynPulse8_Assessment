using SynPulse8_Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynPulse8_DataAccess.Interfaces
{
    public interface ICustomerProvider
    {
        Task<Customer> GetCustomerByIdAsync(string id);
        Task<List<Customer>> GetCustomerListAsync();
    }
}
