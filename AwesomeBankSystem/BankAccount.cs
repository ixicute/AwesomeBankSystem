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
        private Currency currency;

        public BankAccount(string name, Currency currency, double amount = 0)
        {
            this.name = name;
            this.currency = currency;
            this.amount = amount;
            this.accountNumber = GenerateBankAccountNumber();
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

        public Currency Currency
        {
            get { return currency; }
            set { currency = value; }
        }

        public string GenerateBankAccountNumber()
        {
            Random random = new Random();
            string bankaccount = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bankaccount = bankaccount + random.Next(0, 10).ToString();
                }
                bankaccount = bankaccount + "-";
            }
            return bankaccount.Trim('-');
        }
    }
}
