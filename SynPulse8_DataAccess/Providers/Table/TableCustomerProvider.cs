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
    public class TableCustomerProvider : ICustomerProvider
    {
        static string tableName = "customers";
        string partitionkey;
        CloudTable table;

        public IConfiguration Configuration { get; }

        public TableCustomerProvider(IConfiguration configuration)
        {
            Configuration = configuration;
            partitionkey = Configuration.GetValue<string>("Environment");

            string connectionStr = Configuration.GetValue<string>(partitionkey + "::SynPulse8:AzureStorageAccount");

            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(connectionStr);
            CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            table = tblclient.GetTableReference(tableName);
        }

        public Task<Customer> GetCustomerByIdAsync(string id)
        {
            return Task.Run(() => {

                TableOperation tableOperation = TableOperation.Retrieve<CustomerEntity>(partitionkey, id);
                TableResult tableResult = table.Execute(tableOperation);
                CustomerEntity? entity = tableResult.Result as CustomerEntity;

                if (entity == null)
                {
                    return null;
                }

                return entity.ToVdo();
            });
        }

        public Task<List<Customer>> GetCustomerListAsync()
        {
            return Task.Run(() => {

                var query = from customer in table.CreateQuery<CustomerEntity>()
                            where customer.PartitionKey == partitionkey
                            select customer;

                List<Customer> customerList = new List<Customer>();

                foreach (var customer in query.ToList())
                {
                    customerList.Add(customer.ToVdo());
                }

                return customerList;
            });
        }
    }
}
