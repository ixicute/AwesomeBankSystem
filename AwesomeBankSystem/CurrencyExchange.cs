using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class CurrencyExchange
    {
        private double sekToUSD;
        private double sekToEURO;
        private double euroToSEK;
        private double usdToSEK;

        public CurrencyExchange()
        {
            this.sekToUSD = 0.094;
            this.sekToEURO = 0.091;
            this.euroToSEK = 11.01;
            this.usdToSEK = 10.66;
        }

        public double SekToUSD
        {
            get { return sekToUSD; }
            private set { sekToUSD = value; }
        }

        public double SekToEURO
        {
            get { return sekToEURO; }
            private set { sekToEURO = value; }
        }

        public double EuroToSEK
        {
            get { return euroToSEK; }
            private set { euroToSEK = value; }
        }

        public double UsdToSEK
        {
            get { return usdToSEK; }
            private set { usdToSEK = value; }
        }

        /// <summary>
        /// Only Admin-class can access this method.
        /// </summary>
        public void ChangeCurrencyExchange(Admin admin)
        {
            if (admin.IsAdmin)
            {
                double userInput = 0;
                bool check;
                Console.WriteLine("Current exchange rates are: ");
                Console.WriteLine($"SEK to USD:  {sekToUSD}\n" +
                                  $"SEK to EURO: {sekToEURO}\n" +
                                  $"EURO to SEK: {euroToSEK}\n" +
                                  $"USD to SEK:  {usdToSEK}");

                Console.WriteLine("Enter the new exchange rate for: ");
                Console.WriteLine("SEK to USD: ");
                check = double.TryParse(Console.ReadLine(), out userInput);
                if (check)
                {
                    sekToUSD = userInput;
                }

                Console.WriteLine("SEK to EURO: ");
                check = double.TryParse(Console.ReadLine(), out userInput);
                if (check)
                {
                    sekToEURO = userInput;
                }

                Console.WriteLine("EURO to SEK: ");
                check = double.TryParse(Console.ReadLine(), out userInput);
                if (check)
                {
                    euroToSEK = userInput;
                }

                Console.WriteLine("USD to SEK: ");
                check = double.TryParse(Console.ReadLine(), out userInput);
                if (check)
                {
                    usdToSEK = userInput;
                }
                Console.Clear();
                Console.WriteLine("Done!" +
                                  "Exchange rates have been updated to: ");
                Console.WriteLine($"SEK to USD:  {sekToUSD}\n" +
                                  $"SEK to EURO: {sekToEURO}\n" +
                                  $"EURO to SEK: {euroToSEK}\n" +
                                  $"USD to SEK:  {usdToSEK}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            else
            {
                Console.WriteLine("You must be an admin to change the currency exchange rate.");
            }
            
        }
    }
}
