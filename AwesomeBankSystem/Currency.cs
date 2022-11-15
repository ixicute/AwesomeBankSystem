using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    public class Currency
    {
        private string name;
        private double value;

        public Currency(string name = "No Name", double value = 0)
        {
            Name = name;
            Value = value;
        }

        public double Value
        {
            get { return value; }
            set { this.value = value; }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public double CurrencyToSek(double amountOfCurrency)
        {
            return amountOfCurrency * value;
        }

        public double SekToCurrency(double amountOfSeK)
        {
            return amountOfSeK / value;
        }



    }
}
