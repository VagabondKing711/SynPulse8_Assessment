using SynPulse8_Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynPulse8_DataAccess.Interfaces
{
    public interface ITransactionsProvider
    {
        Task<List<Transactions>> GetTransactionsByIdAsync(string id);
        Task<List<Transactions>> GetTransactionsByAccAsync(string accNum);
    }
}
