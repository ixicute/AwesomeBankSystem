using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class CurrencyProvider
    {
        private List<Currency> currencies = new List<Currency>();

        public List<Currency> Currencies
        {
            get { return currencies; }
            set { currencies = value; }
        }
    }
}
