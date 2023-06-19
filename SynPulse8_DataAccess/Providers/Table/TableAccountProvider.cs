using Microsoft.Extensions.Configuration;
using SynPulse8_Abstractions;
using SynPulse8_DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using System.Linq;
using SynPulse8_DataAccess.Providers.Table.Entity;

namespace SynPulse8_DataAccess.Providers.Table
{
    public class TableAccountProvider : IAccountProvider
    {
        static string tableName = "account";
        string partitionkey;
        CloudTable table;

        public IConfiguration Configuration { get; }

        public TableAccountProvider(IConfiguration configuration)
        {
            Configuration = configuration;
            partitionkey = Configuration.GetValue<string>("Environment");

            string connectionStr = Configuration.GetValue<string>(partitionkey + "::SynPulse8:AzureStorageAccount");

            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(connectionStr);
            CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            table = tblclient.GetTableReference(tableName);
        }

        public Task<Account> GetAccountByIBANasync(string accNum)
        {
            return Task.Run(() => {

                TableOperation tableOperation = TableOperation.Retrieve<AccountEntity>(partitionkey, accNum);
                TableResult tableResult = table.Execute(tableOperation);
                AccountEntity? entity = tableResult.Result as AccountEntity;

                return entity.ToVdo();
            });
        }

        public Task<List<Account>> GetAccountListByIdAsync(string id)
        {
            return Task.Run(() => {

                var query = from account in table.CreateQuery<AccountEntity>()
                            where account.PartitionKey == partitionkey && 
                                account.RowKey == id
                            select account;

                List<Account> accountList = new List<Account>();

                foreach (var account in query.ToList())
                {
                    accountList.Add(account.ToVdo());
                }

                return accountList;
            });
        }
    }
}
