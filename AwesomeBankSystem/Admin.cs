using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class Admin : User
    {
        private double sekToUSD;
        private double sekToEURO;
        private double euroToSEK;
        private double usdToSEK;

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

        public Admin(string name, string password, bool isAdmin) : base(name, password, isAdmin)
        {
            this.sekToUSD = 0.094;
            this.sekToEURO = 0.091;
            this.euroToSEK = 11.01;
            this.usdToSEK = 10.66;
        }

        public void ExchangeSekToUSD()
        {

        }

        public void ExchangeSekToEURO()
        {

        }

        public void ExchangeEuroToSEK()
        {

        }

        public void ExchangeUsdToSEK()
        {

        }
    }
}
