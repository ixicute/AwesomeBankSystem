using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal abstract class User
    {
        protected bool isAdmin;
        protected string username;
        protected string password;

        public User(string name, string password, bool isAdmin)
        {
            this.username = name;
            this.password = password;
            this.isAdmin = isAdmin;
        }
        public bool IsAdmin
        {
            get { return isAdmin; }
        }
        public string Password
        {
            get { return password; }
        }
        public string UserName
        {
            get { return username; }
        }
    }
}
