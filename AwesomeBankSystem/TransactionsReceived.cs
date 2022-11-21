using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AwesomeBankSystem
{
    internal class TransactionsReceived : Transactions
    {
        public TransactionsReceived(BankAccount fromAcc, BankAccount toAcc, double amount, string fromUser = "unknown")
        {
            this.fromAcc = fromAcc;
            this.toAcc = toAcc;
            this.amount = amount;
            this.fromUser = fromUser;
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

        public string FromUser
        {
            get { return fromUser; }
        }
    }
}
