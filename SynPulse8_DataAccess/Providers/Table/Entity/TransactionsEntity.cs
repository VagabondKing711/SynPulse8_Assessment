using Microsoft.Azure.Cosmos.Table;
using SynPulse8_Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynPulse8_DataAccess.Providers.Table.Entity
{
    public class TransactionsEntity : TableEntity
    {
        public string Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }

        public static TransactionsEntity? FromVdo(Transactions transactions)
        {
            var result = new TransactionsEntity()
            {
                RowKey = transactions.AccountIBAN,
                Amount = transactions.Amount,
                TransactionDate = transactions.TransactionDate,
                Description = transactions.Description,
            };

            return result;
        }

        public Transactions? ToVdo()
        {
            var result = new Transactions()
            {
                AccountIBAN = RowKey,
                Amount = Amount,
                TransactionDate = TransactionDate,
                Description = Description,
            };

            return result;
        }
    }
}
