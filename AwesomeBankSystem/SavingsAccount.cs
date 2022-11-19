using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class SavingsAccount : BankAccount
    {
        
        private double interest = 0.1;

        public double Interest
        {
            get { return interest; }
        }

        public SavingsAccount(string name, string currency, double amount = 0) : base(name, currency, amount)
        {
            this.Amount += amount * interest;
            isSavingAccount = true;
        }
    }
}
