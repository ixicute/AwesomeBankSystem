using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class Transactions
    {
        protected BankAccount fromAcc;
        protected BankAccount toAcc;
        protected string fromUser;
        protected double amount;
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
