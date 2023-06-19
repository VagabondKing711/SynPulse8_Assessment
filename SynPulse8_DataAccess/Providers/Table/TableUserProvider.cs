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
    public class TableUserProvider : IUserProvider
    {
        static string tableName = "users";
        string partitionkey; // Will be the env
        CloudTable table;

        public IConfiguration Configuration { get; }

        public TableUserProvider(IConfiguration configuration)
        {
            Configuration = configuration;
            partitionkey = Configuration.GetValue<string>("Environment");

            string connectionStr = Configuration.GetValue<string>(partitionkey + "::SynPulse8:AzureStorageAccount");

            CloudStorageAccount storageAcc = CloudStorageAccount.Parse(connectionStr);
            CloudTableClient tblclient = storageAcc.CreateCloudTableClient(new TableClientConfiguration());
            table = tblclient.GetTableReference(tableName);
        }

        public Task<string> GetPasswordHashAsync(string id)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return Task.Run(() => {
                TableOperation tableOperation = TableOperation.Retrieve<UserEntity>(partitionkey, id);
                TableResult tableResult = table.Execute(tableOperation);
                UserEntity? entity = tableResult.Result as UserEntity;

                if (entity == null)
                {
                    return null;
                }

                return entity.PasswordHash;
            });
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }

        public Task<User> GetUserByIdAsync(string id)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return Task.Run(() => {
                TableOperation tableOperation = TableOperation.Retrieve<UserEntity>(partitionkey, id);
                TableResult tableResult = table.Execute(tableOperation);
                UserEntity? entity = tableResult.Result as UserEntity;

                if (entity == null)
                {
                    return null;
                }

                return entity.ToVdo();
            });
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
            return Task.Run(() =>
            {

                var query = from user in table.CreateQuery<UserEntity>()
                            where user.PartitionKey == partitionkey &&
                                  user.EMailAddress == email
                            select user;

                UserEntity? entity = query.FirstOrDefault<UserEntity>();
                if (entity == null)
                {
                    return null;
                }


                return entity.ToVdo();
            });
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
        }

        public Task<List<User>> GetUsersAsync()
        {
            return Task.Run(() =>
            {

                var query = from user in table.CreateQuery<UserEntity>()
                            where user.PartitionKey == partitionkey
                            select user;

                List<User> userList = new List<User>();

                foreach (var user in query.ToList())
                {
                    userList.Add(user.ToVdo());
                }

                return userList;
            });
        }
    }
}
