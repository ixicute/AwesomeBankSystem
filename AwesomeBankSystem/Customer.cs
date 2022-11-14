using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class Customer : User
    {
        private List<BankAccount> bankAccounts = new List<BankAccount>();
        public Customer(string name, string password, bool isAdmin) : base(name, password, isAdmin)
        {

        }

        public List<BankAccount> BankAccounts
        {
            get { return bankAccounts; }

            set { bankAccounts = value; }
        }
    }
}
