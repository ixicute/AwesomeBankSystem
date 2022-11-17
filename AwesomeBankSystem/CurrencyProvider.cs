using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    public class CurrencyProvider
    {
        private List<Currency> currencies = new List<Currency>();


        public CurrencyProvider()
        {
            currencies.Add(new Currency("USD", 10.37));
            currencies.Add(new Currency("EUR", 10.79));
            currencies.Add(new Currency("DKK", 1.45));

        }
        public List<Currency> Currencies
        {
            get { return currencies; }
        }

        public void AddNewCurrency(string name, double value)
        {
            var tempCurrency = currencies.Find(x => x.Name.ToLower() == name.ToLower());
            if (tempCurrency != null)
            {
                Console.WriteLine($"Currency {tempCurrency.Name} already exists. Try adding an other currency");
            }
            else
            {
                currencies.Add(new Currency(name, value));
                Console.WriteLine($"Currency {name} was added with value: {value}");
            }
        }

        public void SetNewCurrencyValue(string currencyName, double newRate)
        {
            var tempCurrency = currencies.Find(x => x.Name.ToLower() == currencyName.ToLower());

            if(tempCurrency != null)
            {
                Console.WriteLine($"Changing {tempCurrency.Name} from OLD value: {tempCurrency.Value} to NEW value: {newRate}");
                tempCurrency.Value = newRate;
            }
            else
            {
                Console.WriteLine($"Currency {currencyName} was not found, please try again with other name");
            }
        }

        public double GetCurrencyRate(string name)
        {
            var tempCurrency = currencies.Find(x => x.Name.ToLower() == name.ToLower());
            if(tempCurrency != null)
            {
                return tempCurrency.Value;
            }

            Console.WriteLine($"Did not find any currency with name {name}");
            return 0;
        }

        public Currency GetCurrency(string name)
        {
            var tempCurrency = currencies.Find(x => x.Name.ToLower() == name.ToLower());
            if (tempCurrency != null)
            {
                return tempCurrency;
            }

            Console.WriteLine($"Did not find any currency with name {name}");
            return new Currency();
        }

        public void PrintCurrencyValues()
        {
            int counter = 1;
            foreach (var currency in currencies)
            {
                Console.WriteLine($"Indexnumber {counter}: {currency.Name} has value: {currency.Value}");
                counter++;
            }
        }
    }
}
