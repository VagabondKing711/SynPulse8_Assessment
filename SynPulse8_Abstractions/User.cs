using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynPulse8_Abstractions
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        public string CustomerId { get; set; }
        public string EMailAddress { get; set; }
        public AccountStatus Status { get; set; }
        public enum AccountStatus
        {
            Locked = 0,
            Active = 1
        }
    }
}
