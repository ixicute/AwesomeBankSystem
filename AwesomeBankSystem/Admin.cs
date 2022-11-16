using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class Admin : User
    {
        public Admin(string name, string password, bool isAdmin) : base(name, password, isAdmin)
        {

        }
    }
}
