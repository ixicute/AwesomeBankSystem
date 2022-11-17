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
        
        CurrencyProvider currencyProvider = new CurrencyProvider();
        List<BankAccount> AllAccountList = new List<BankAccount>();
        
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
                    case "test":
                        MoneyToSelf();
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
                Console.WriteLine($"Account name: {bankAccount.Name} Account number: {bankAccount.AccountNumber} Amount: {bankAccount.Amount} {bankAccount.Currency}");
            }

        }
        
        /// <summary>
        /// Used to open a new bank account (normal or for savings) with a generated account number.
        /// </summary>
        public void OpenBankAccount()
        {            
            //needed variables.
            string accountName = "";
            string type = "";
            int CurrencyInput = 0;
            int input = 0;
            double ammount = 0;
            bool success = false;
            //Currency currency = new Currency();
            string currency = "SEK";
            Customer loggedInCustomer = (Customer)loggedInUser;
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
                        //Ask user for account name + error handling.
                        Console.WriteLine("Choose a name for your new account: ");
                        accountName = Console.ReadLine();

                        if (string.IsNullOrEmpty(accountName) || string.IsNullOrWhiteSpace(accountName))
                        {
                            accountName = "Saving Account";
                        }

                        Console.WriteLine("Alla konton har Svenska krona (SEK) som standard valuta.\n" +
                                          "Tryck 1 och ENTER om du vill välja ett annat valuta (Annars tryck bara ENTER): ");
                        _ = int.TryParse(Console.ReadLine(), out CurrencyInput);
                        
                        if (CurrencyInput == 1)
                        {
                            Console.WriteLine("Välj en valuta från listan:\n" +
                                              "1. USD\n" +
                                              "2. EURO\n" +
                                              "3. Fortsätt med SEK");
                            int temp;
                            bool check = int.TryParse(Console.ReadLine(), out temp);
                            temp = check ? temp : 3;

                            if(temp == 1)
                            {
                                currency = "USD";
                            }
                            else if(temp == 2)
                            {
                                currency = "EURO";
                            }
                            else
                            {
                                currency = "SEK";
                            }
                        }
                        else
                        {
                            currency = "SEK";
                        }

                        //Runs method with a menu to ask user for ammount to input and return to variable.
                        ammount = AddMoney(currency);

                        //This code creates a temp of type Customer and casts contents of "LoggedInUser" to it.                        

                        //The content of the above temp-type is then sent into a list inside of the Customer-class.
                        loggedInCustomer.BankAccounts.Add(new SavingsAccount(accountName, currency, ammount));

                        success = true;
                        break;

                    case 2:
                        //Ask user for account name + error handling.
                        Console.WriteLine("Choose a name for your new account: ");
                        accountName = Console.ReadLine();

                        if (string.IsNullOrEmpty(accountName) || string.IsNullOrWhiteSpace(accountName))
                        {
                            accountName = "Normal Account";
                        }

                        Console.WriteLine("Alla konton har Svenska krona (SEK) som standard valuta.\n" +
                                          "Tryck 1 och ENTER om du vill välja ett annat valuta: ");
                        _ = int.TryParse(Console.ReadLine(), out CurrencyInput);

                        if (CurrencyInput == 1)
                        {
                            Console.WriteLine("Välj en valuta från listan:\n" +
                                              "1. USD\n" +
                                              "2. EURO\n" +
                                              "3. Fortsätt med SEK");
                            int temp;
                            bool check = int.TryParse(Console.ReadLine(), out temp);
                            temp = check ? temp : 3;

                            if (temp == 1)
                            {
                                currency = "USD";
                            }
                            else if (temp == 2)
                            {
                                currency = "EURO";
                            }
                        }

                        //Runs method with a menu to ask user for ammount to input and return to variable.
                        ammount = AddMoney(currency);

                        loggedInCustomer.BankAccounts.Add(new BaseAccount(accountName, currency, ammount));
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
                Customer customer = (Customer)loggedInUser;

                Console.WriteLine($"A {type} has been created with the name {accountName}\n" +
                                  $"bankaccount number is: {customer.BankAccounts.First(x => x.Name == accountName).AccountNumber} with currency {currency}\n" +
                                  $"Current Balance is: {ammount} {currency}");
            }
        }

        /// <summary>
        /// Lets user specify ammount of money to add and returns it as a double-variable.
        /// </summary>
        private double AddMoney(string currency)
        {
            double result;
            Console.WriteLine($"How much money do you want to add to your account in {currency}?");
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

        //Can't get here if you only have one account!!!
        public void MoneyToSelf()
        {
            Customer loggedInCustomer = (Customer)loggedInUser;
            int userInput = 0;
            string sendToAcc = "";
            string sendFromAcc = "";
            double amountToSend;

            foreach (var myAcc in loggedInCustomer.BankAccounts)
            {
                Console.WriteLine($"Account: [{myAcc.Name} - {myAcc.AccountNumber} has {myAcc.Amount} {myAcc.Currency}]");
            }

            Console.WriteLine("Write the name of the account that you want to send money from: ");
            sendFromAcc = Console.ReadLine().ToLower();
            var sendFrom = loggedInCustomer.BankAccounts.Find(x => x.Name.ToLower() == sendFromAcc);

            foreach (var myAcc in loggedInCustomer.BankAccounts)
            {
                Console.WriteLine($"Account: [{myAcc.Name} - {myAcc.AccountNumber} has {myAcc.Amount} {myAcc.Currency}]");
            }

            Console.WriteLine("Write the name of the account you want to send money to: ");
            sendToAcc = Console.ReadLine().ToLower();
            var sendTo = loggedInCustomer.BankAccounts.Find(x => x.Name.ToLower() == sendToAcc);

            Console.WriteLine($"How much money in {sendFrom.Currency} do you want to send?");
            bool check = double.TryParse(Console.ReadLine(), out amountToSend);
            if (!check)
            {
                amountToSend = 0;
            }

            Send(sendFrom, sendTo, amountToSend);
        }

        public void Send(BankAccount from, BankAccount to, double amount)
        {
            Customer loggedInCustomer = (Customer)loggedInUser;
            int indexNum = 0;
            for (int i = 0; i < loggedInCustomer.BankAccounts.Count; i++)
            {
                if (loggedInCustomer.BankAccounts[i].AccountNumber == to.AccountNumber)
                {
                    loggedInCustomer.BankAccounts[i].Amount -= amount;
                    indexNum = i;
                }
            }

            Console.WriteLine($"New balance is: {loggedInCustomer.BankAccounts[indexNum].Amount}");
        }

        /// <summary>
        /// Method used to exchange money to different values before transferring them.
        /// </summary>
        public double ValueExchange()
        {
            double amount = 0;
            int userInput = 0;
            Console.WriteLine("Welcome to our exchange system! Pick an option from the menu:");
            Console.WriteLine("1. I have SEK and want to exchange to USD\n" +
                              "2. I have SEK and want to exchange to EURO\n" +
                              "3. I have EURO and want to exchange to SEK\n" +
                              "4. I have USD and wich to exchange to SEK");
            bool check = int.TryParse(Console.ReadLine(), out userInput);
            userInput = check ? 0 : userInput;

            switch (userInput)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                default:
                    break;
            }
            return amount;
        }

        /// <summary>
        /// Checking and adding up all amounts then approves loan OR user can try entering another loan amount  
        /// </summary>
        public void NewBankLoan()
        {
            double totalBankAmount = 0;
            string theEnd;

            do
            {
                Console.WriteLine("Enter the amount of money you want to loan");
                double inputAmount = Convert.ToDouble(Console.ReadLine());
                Customer savingAcc = (Customer)loggedInUser;

                foreach (var item in savingAcc.BankAccounts)  
                {
                    totalBankAmount += item.Amount;
                }

                double totalLoanAmount = (totalBankAmount * 5.0);  

                if (inputAmount <= totalLoanAmount)  
                {
                    Console.WriteLine($"You can loan the amount {inputAmount} with the interestrate of 10 %.");
                    Console.WriteLine($"Total amount of the loan is {InterestRate(inputAmount)}.");
                    break;
                }
                else 
                {
                    Console.WriteLine($"You can't loan the amount {inputAmount}."); 
                    Console.WriteLine("Do you want to enter another amount: Yes or No");
                    theEnd = Console.ReadLine();
                }
            } while (theEnd.ToLower()!="no");
        }
        /// <summary>
        /// formula for interestrate, returns the hole loan including interest
        /// </summary>
        public double InterestRate(double inputAmount) 
        {
            double amountWithRate = (inputAmount / 100.0) * 10.0 + inputAmount;
            return amountWithRate;
        }
    } 
}
