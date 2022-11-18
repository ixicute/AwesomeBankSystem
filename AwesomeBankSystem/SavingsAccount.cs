using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class SavingsAccount : BankAccount
    {
        public string AccountType { get; init; } = "Savings Account";
        public SavingsAccount(string name, string currency, double amount = 0) : base(name, currency, amount)
        {
        }
    }
}
