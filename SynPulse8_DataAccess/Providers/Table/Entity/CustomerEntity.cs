using Microsoft.Azure.Cosmos.Table;
using SynPulse8_Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynPulse8_DataAccess.Providers.Table.Entity
{
    public class CustomerEntity : TableEntity
    {
        public string Name { get; set; }

        public static CustomerEntity? FromVdo(Customer customer)
        {
            if (customer == null)
            {
                return null;
            }

            var result = new CustomerEntity()
            {
                RowKey = customer.Id,
                Name = customer.Name
            };

            return result;
        }

        public Customer ToVdo()
        {
            var result = new Customer
            {
                Id = RowKey,
                Name = Name
            };

            return result;
        }
    }
}
