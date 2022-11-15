using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    public abstract class BankAccount
    {
        private double amount;
        private string accountNumber;
        private string name;


        public BankAccount(double amount, string accountNumber, string name)
        {
            this.amount = amount;
            this.accountNumber = accountNumber;
            this.name = name;
        }
        public string AccountNumber
        {
            get { return accountNumber; }
            set { accountNumber = value; }
        }

        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
