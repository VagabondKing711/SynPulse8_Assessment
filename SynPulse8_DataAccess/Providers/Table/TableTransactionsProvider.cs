using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using SynPulse8_Abstractions;
using SynPulse8_DataAccess.Interfaces;
using SynPulse8_DataAccess.Providers.Table.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynPulse8_DataAccess.Providers.Table
{
    public class TableTransactionsProvider : ITransactionsProvider
    {
        static string tableName = "transactions";
        string partitionkey;
        CloudTable table;

        public IConfiguration Configuration { get; }

        public TableTransactionsProvider(IConfiguration configuration)
        {
            Configuration = configuration;
            partitionkey = Configuration.GetValue<string>("Environment");

            string connectionStr = Configuration.GetValue<string>(partitionkey + "::SynPulse8:AzureStorageAccount");

            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(connectionStr);
            CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            table = tblclient.GetTableReference(tableName);
        }

        public Task<List<Transactions>> GetTransactionsByIdAsync(string id)
        {
            return Task.Run(() => {

                var query = from transactions in table.CreateQuery<TransactionsEntity>()
                            where transactions.PartitionKey == partitionkey &&
                                transactions.RowKey == id
                            select transactions;

                List<Transactions> transactionsList = new List<Transactions>();

                foreach (var transactions in query.ToList())
                {
                    transactionsList.Add(transactions.ToVdo());
                }

                return transactionsList;
            });
        }

        public Task<List<Transactions>> GetTransactionsByAccAsync(string accNum)
        {
            return Task.Run(() => {

                var query = from transactions in table.CreateQuery<TransactionsEntity>()
                            where transactions.PartitionKey == partitionkey &&
                                transactions.RowKey == accNum
                            select transactions;

                List<Transactions> transactionsList = new List<Transactions>();

                foreach (var transactions in query.ToList())
                {
                    transactionsList.Add(transactions.ToVdo());
                }

                return transactionsList;
            });
        }
    }
}
