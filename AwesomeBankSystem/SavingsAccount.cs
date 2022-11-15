using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class SavingsAccount : BankAccount
    {
        public SavingsAccount(double amount, string accountNumber, string name) : base(amount, accountNumber, name)
        {

        }
    }
}
