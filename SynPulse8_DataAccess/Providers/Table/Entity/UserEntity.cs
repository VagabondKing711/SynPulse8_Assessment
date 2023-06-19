using Microsoft.Azure.Cosmos.Table;
using Newtonsoft.Json;
using SynPulse8_Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SynPulse8_Abstractions.User;

namespace SynPulse8_DataAccess.Providers.Table.Entity
{
    public class UserEntity : TableEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string FamilyName { get; set; }
        public string CustomerId { get; set; }
        public string EMailAddress { get; set; }
        public AccountStatus Status { get; set; }
        public string StatusSerialized
        {
            get
            {
                var result = Status.ToString();
                return result;
            }

            set
            {
                AccountStatus status = AccountStatus.Locked;
                if (value != null)
                {
                    status = (AccountStatus)Enum.Parse(typeof(AccountStatus), value, true);
                }

                Status = status;
            }
        }
        public string PasswordHash { get; set; }

        public static UserEntity? FromVdo(User user)
        {
            if (user == null)
            {
                return null;
            }

            var result = new UserEntity()
            {
                RowKey = user.Id,
                FirstName = user.FirstName,
                FamilyName = user.FamilyName,
                CustomerId = user.CustomerId,
                EMailAddress = user.EMailAddress,
                Status = user.Status
            };

            return result;
        }

        public User ToVdo()
        {
            var result = new User
            {
                Id = RowKey,
                FirstName = FirstName,
                FamilyName = FamilyName,
                CustomerId = CustomerId,
                Status = Status
            };

            return result;
        }
    }
}
