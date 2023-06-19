using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynPulse8_Abstractions
{
    public class Account
    {
        public string CustomerId { get; set; }
        public string AccountIBAN { get; set; }
        public string Currency { get; set; }
    }
}
