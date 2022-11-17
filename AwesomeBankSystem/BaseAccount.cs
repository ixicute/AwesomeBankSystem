using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class BaseAccount : BankAccount
    {
        public string AccountType { get; init; } = "Normal Account";
        public BaseAccount(string name, string currency, double amount = 0) : base(name, currency, amount)
        {
        }
    }
}
