using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBankSystem
{
    internal class BankSystem
    {
        User loggedInUser;
        List<User> userList = new List<User>();
        List<BankAccount> AllAccountsList = new List<BankAccount>();

        //user logging in: menu
        public void Run()
        {
            InitiateUsers();
            int check = 0;
            Console.WriteLine("Welcome to the Bank");
            Console.WriteLine("Log In Below");

            do
            {
                Console.WriteLine("Username: ");
                string name = Console.ReadLine();

                Console.WriteLine("Password: ");
                string pass = Console.ReadLine();

                User temp = userList.Find(x => x.UserName == name && x.Password == pass);

                if (userList.Contains(temp))
                {
                    loggedInUser = temp;
                    InLoggad();
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Fel Användarnamn eller Lösenord.");
                    check++;
                }
            } while (check < 3);

            if (check == 3)
            {
                Console.WriteLine("Du har misslyckats 3 gånger och blev utlåst!");
            }

        }

        public void InLoggad()
        {
            Console.WriteLine($"Du är nu inloggad som {loggedInUser.UserName}");

            if (loggedInUser.IsAdmin)
            {
                AdminMenu();
            }
            else
            {
                UserMenu();
            }
        }

        public void AdminMenu()
        {
            Console.WriteLine("This is the admin menu");
            string command = "";
            while (!command.ToLower().Equals("exit"))
            {
                Console.WriteLine("Write what commnand you want to do");
                Console.WriteLine("Commands: AC = Adds a new customer, PC = Prints all customers, Exit = Exits the program)");
                command = Console.ReadLine();
                switch (command.ToLower())
                {
                    case "ac":
                        AddCustomer();
                        break;
                    case "pc":
                        PrintAllCustomers();
                        break;
                    case "exit":
                        SignOut();
                        break;
                    default:
                        Console.WriteLine("Command not recognized");
                        break;
                }
            }
        }

        public void UserMenu()
        {
            Console.WriteLine("This is the Customer menu");
            string command = "";
            while (!command.ToLower().Equals("exit"))
            {
                Console.WriteLine("Write what commnand you want to do");
                Console.WriteLine("Commands: PB Print Bankaccount information, OB = Open new bankaccount, Exit = Exists the program)");
                command = Console.ReadLine();
                switch (command.ToLower())
                {
                    case "pb":
                        PrintBankAccounts();
                        break;
                    case "ob":
                        OpenBankAccount();
                        break;
                    case "exit":
                        SignOut();
                        break;
                    default:
                        Console.WriteLine("Command not recognized");
                        break;
                }
            }
        }

        public void InitiateUsers()
        {
            userList.Add(new Admin("admin", "password", true));
            userList.Add(new Customer("customer", "password", false));
        }

        public void AddCustomer()
        {
            Console.WriteLine("Initiate add customer");
            Console.WriteLine("Write the username of the customer");
            string username = Console.ReadLine();
            Console.WriteLine("Write the password of the customer");
            string password = Console.ReadLine();
            Customer tempCustomer = new Customer(username, password, false);
            userList.Add(tempCustomer);
            Console.WriteLine($"Customer {tempCustomer.UserName} added successfully");
        }

        public void PrintAllCustomers()
        {
            Console.WriteLine("Printing all customers..");
            foreach (User user in userList)
            {
                Console.WriteLine(user.UserName);
            }
        }

        public void SignOut()
        {
            //Todo
            Console.WriteLine("Signing off and closing application");
        }

        public void PrintBankAccounts()
        {
            Customer loggedInCustomer = (Customer)loggedInUser;
            if (loggedInCustomer.BankAccounts.Count < 1)
            {
                Console.WriteLine("You dont have any bankaccounts, please create one with the 'OB' command");
            }
            foreach (BankAccount bankAccount in loggedInCustomer.BankAccounts)
            {
                Console.WriteLine($"Account name: {bankAccount.Name} Account number: {bankAccount.AccountNumber} Amount: {bankAccount.Amount}");
            }

        }

        /// <summary>
        /// Used to open a new bank account (normal or for savings) with a generated account number.
        /// </summary>
        public void OpenBankAccount()
        {            
            //needed variables.
            string accNumber = "";
            string type = "";
            int input = 0;
            double ammount = 0;
            bool success = false;

            //Loop menu for picking account-type.
            do
            {
                Console.WriteLine("Which account type do you wish to open? (Press a number)\n" +
                  "1. Savings Account\n" +
                  "2. Normal Accoount\n" +
                  "3. Cancel");

                //Error handling
                int.TryParse(Console.ReadLine(), out input);

                switch (input)
                {
                    case 1:
                        //Runs method with a menu to ask user for ammount to input and return to variable.
                        ammount = AddMoney();

                        //Runs method to generate a random account number for the user.
                        accNumber = GenerateBankAccountNumber();

                        //This code creates a temp of type Customer and casts contents of "LoggedInUser" to it.
                        Customer savingAcc = (Customer)loggedInUser;

                        //The content of the above temp-type is then sent into a list inside of the Customer-class.
                        savingAcc.BankAccounts.Add(new SavingsAccount(ammount, accNumber, "Saving Account"));

                        success = true;
                        break;
                    /*
                     * Code above (with lists) should be revisited. Should we instead add the list in this class (I.e BankSystem.cs)
                     * If we choose to do that then the list created in banksystems.cs (row 13) should be referensed as follows:
                     * AllAccountsList.Add(new SavingsAccount(ammount, accNumber, "Saving Account"));
                     * 
                     */

                    case 2:

                        ammount = AddMoney();
                        accNumber = GenerateBankAccountNumber();

                        Customer baseAcc = (Customer)loggedInUser;

                        baseAcc.BankAccounts.Add(new SavingsAccount(ammount, accNumber, "Normal Account"));
                        success = true;
                        break;

                    case 3:

                        Console.WriteLine("Exiting menu - No accounts were created.");
                        input = 1;
                        break;

                    default:

                        Console.Clear();
                        Console.WriteLine("You must choose an option from the menu. Try Again.");
                        break;
                }

            } while (input != 1 && input != 2);

            //If user reaches case 1 or case 2 (hence creating an account)
            if (success)
            {
                //If user pressed 1 then type is "Saving Account"
                //otherwise (aka if 2) then its "normal Account".
                type = input == 1 ? "Saving Account" : "Normal Account";

                Console.WriteLine($"A {type} has been created\n" +
                                  $"bankaccount number is: {accNumber}\n" +
                                  $"Current Balance is: {ammount}");
            }
        }

        /// <summary>
        /// Lets user specify ammount of money to add and returns it as a double-variable.
        /// </summary>
        private double AddMoney()
        {
            double result;
            Console.WriteLine("How much money do you want to add to your account? ");
            do
            {
                double.TryParse(Console.ReadLine(), out result);

                //If user enters a number less than 0 it will not be approved.
                if (result < 0)
                {
                    Console.WriteLine("The ammount can not be less than 0, try again!");
                }

            } while (result < 0);

            return result;
        }
        public string GenerateBankAccountNumber()
        {
            Random random = new Random();
            string bankaccount = "";
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bankaccount = bankaccount + random.Next(0, 10).ToString();
                }
                bankaccount = bankaccount + "-";
            }
            return bankaccount.Trim('-');
        }

    }
}
