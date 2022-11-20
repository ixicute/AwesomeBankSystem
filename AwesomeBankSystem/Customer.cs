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

        private List<TransactionsSent> transactionsSent = new List<TransactionsSent>();

        private List<TransactionsReceived> transactionsReceived = new List<TransactionsReceived>();

        public Customer(string name, string password, bool isAdmin) : base(name, password, isAdmin)
        {
        }

        public List<BankAccount> BankAccounts
        {
            get { return bankAccounts; }

            set { bankAccounts = value; }
        }

        public List<TransactionsSent> TransactionsSent
        {
            get { return transactionsSent; }
            set { transactionsSent = value; }
        }
        
        public List<TransactionsReceived> TransactionsReceived
        {
            get { return transactionsReceived; }
            set { transactionsReceived = value; }
        }
    }
}
