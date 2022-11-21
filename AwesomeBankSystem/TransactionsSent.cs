using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    
    internal class TransactionsSent : Transactions
    {
        private string toUser;
        public TransactionsSent(BankAccount fromAcc, BankAccount toAcc, double amount, string toUser = "unknown")
        {
            this.fromAcc = fromAcc;
            this.toAcc = toAcc;
            this.toUser = toUser;
            this.amount = amount;
        }

        public BankAccount From
        {
            get { return fromAcc; }
        }

        public BankAccount To
        {
            get { return toAcc; }
        }

        public double Amount
        {
            get { return amount; }
        }

        public string ToUser
        {
            get { return toUser; }
        }
    }
}
