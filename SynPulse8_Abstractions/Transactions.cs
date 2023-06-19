using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynPulse8_Abstractions
{
    public class Transactions
    {
        public string AccountIBAN { get; set; }
        public string Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }
}
