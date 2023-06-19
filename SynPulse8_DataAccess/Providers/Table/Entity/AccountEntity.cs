using Microsoft.Azure.Cosmos.Table;
using SynPulse8_Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SynPulse8_DataAccess.Providers.Table.Entity
{
    public class AccountEntity : TableEntity
    {
        public string AccountIBAN { get; set; }
        public string Currency { get; set; }

        public static AccountEntity? FromVdo(Account account) 
        {
            var result = new AccountEntity()
            {
                RowKey = account.CustomerId,
                AccountIBAN = account.AccountIBAN,
                Currency = account.Currency,
            };

            return result;
        }

        public Account ToVdo()
        {
            var result = new Account()
            {
                CustomerId = RowKey,
                AccountIBAN = AccountIBAN,
                Currency = Currency,
            };

            return result;
        }
    }
}
