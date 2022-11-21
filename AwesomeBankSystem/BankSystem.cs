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
        Customer customer;
        Customer transactionFrom;
        Customer transactionTo;
        List<User> userList = new List<User>();

        CurrencyExchange changeRate = new CurrencyExchange();
        
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
            while (!command.ToLower().Equals("ex"))
            {
                Console.WriteLine("Välj vad du vill göra från menyn (Genom att skriva bokstaven som motsvarar ditt val):");
                Console.WriteLine("SK.  Skriv ut mina konton\n" +
                                  "OB.  Öppna nytt konto\n" +
                                  "SP1. Skicka pengar till konto du äger\n" +
                                  "SP2. Skicka pengar till annan kund\n" +
                                  "NL. Ansök om nytt lån\n" +
                                  "VT.  Visa alla transaktioner till och från dina konton\n" +
                                  "EX.  Logga ut");
                command = Console.ReadLine();
                switch (command.ToLower())
                {
                    case "sk":
                        PrintBankAccounts();
                        break;
                    case "ob":
                        OpenBankAccount();
                        break;
                    case "sp1":
                        MoneyToSelf();
                        break;
                    case "sp2":
                        MoneyToUser();
                        break;
                    case "nl":
                        NewBankLoan();
                        break;
                    case "vt":
                        ShowTransactions();
                        break;
                    case "ex":
                        SignOut();
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                        break;
                }
            }
        }

        public void InitiateUsers()
        {
            userList.Add(new Admin("admin", "password", true));
            userList.Add(new Customer("customer", "password", false));
            userList.Add(new Customer("Aldor", "password", false));
            customer = (Customer)userList.Find(x => x.UserName == "Aldor");
            customer.BankAccounts.Add(new BaseAccount("Baskonto", "SEK", 1000));
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
            double amount = 0;
            bool success = false;
            //Currency currency = new Currency();
            string currency = "SEK";

            //We find the Logged in user from the userList.
            customer = (Customer)userList.Find(x => x.UserName == loggedInUser.UserName);

            //Loop menu for picking account-type to create.
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

                        Console.Clear();
                        Console.WriteLine("Alla konton har Svenska krona (SEK) som standard valuta.\n" +
                                          "Tryck 1 och ENTER om du vill välja ett annat valuta (Annars tryck bara ENTER): ");
                        _ = int.TryParse(Console.ReadLine(), out CurrencyInput);

                        Console.Clear();
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
                        }
                        else
                        {
                            currency = "SEK";
                        }

                        //Runs method with a menu to ask user for ammount to input and return to variable.
                        amount = AddMoney(currency);
                        
                        customer.BankAccounts.Add(new SavingsAccount(accountName, currency, amount));

                        Console.WriteLine($"The current interest-rate is 10%.\n" +
                                          $"Since you have decided to add {amount} {currency} to your account: \n" +
                                          $"It is now {customer.BankAccounts.Last().Amount}!");

                        Console.WriteLine($"{customer.BankAccounts.Last().Name} with account number [{customer.BankAccounts.Last().AccountNumber}] "
                                          +$"has been added to {customer.UserName}s accounts.");
                        Console.ReadKey();

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

                        Console.Clear();
                        Console.WriteLine("Alla konton har Svenska krona (SEK) som standard valuta.\n" +
                                          "Tryck 1 och ENTER om du vill välja ett annat valuta (Annars tryck bara ENTER): ");
                        _ = int.TryParse(Console.ReadLine(), out CurrencyInput);
                        
                        Console.Clear();
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
                        amount = AddMoney(currency);

                        customer.BankAccounts.Add(new BaseAccount(accountName, currency, amount));
                        success = true;
                        break;

                    case 3:

                        Console.Clear();
                        Console.WriteLine("Exiting menu - No accounts were created.");
                        input = 1;                        
                        break;

                    default:

                        Console.Clear();
                        Console.WriteLine("You must choose an option from the menu. Try Again.");
                        break;
                }

            } while (input != 1 && input != 2);

            Console.Clear();
            //If user reaches case 1 or case 2 (hence creating an account)
            if (success)
            {
                
                //If user pressed 1 then type is "Saving Account"
                //otherwise (aka if 2) then its "normal Account".
                type = input == 1 ? "Saving Account" : "Normal Account";

                Console.WriteLine($"A {type} has been created with the name {accountName}\n" +
                                  $"bankaccount number is: {customer.BankAccounts.First(x => x.Name == accountName).AccountNumber} with currency {currency}\n" +
                                  $"Current Balance is: {amount} {currency}");
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
        /// <summary>
        /// Used to send money between owned accounts
        /// </summary>
        public void MoneyToSelf()
        {
            customer = (Customer)userList.Find(x => x.UserName == loggedInUser.UserName);
            string sendToAcc = "";
            string sendFromAcc = "";
            double amountToSend;
            BankAccount sendTo;
            BankAccount sendFrom;

            //Will only work if user exists and has a bank account
            if (customer.BankAccounts.Count > 0 && customer != null)
            {
                while (true)
                {
                    Console.Clear();
                    bool check = false;
                    //Printing out all of the current user's accounts
                    foreach (var myAcc in customer.BankAccounts)
                    {
                        Console.WriteLine($"Account: [{myAcc.Name} - {myAcc.AccountNumber} has {myAcc.Amount} {myAcc.Currency}]");
                    }

                    Console.WriteLine("Write the name of the account that you want to send money from: ");

                    sendFromAcc = Console.ReadLine().ToLower();

                    //Check to see if account exists and copy instance.
                    sendFrom = customer.BankAccounts.Find(x => x.Name.ToLower() == sendFromAcc);

                    if (!string.IsNullOrEmpty(sendFromAcc) && sendFrom != null)
                    {
                        check = customer.BankAccounts.Contains(sendFrom);

                        if (check)
                        {
                            break;
                        }
                    }

                    else if (string.IsNullOrEmpty(sendFromAcc) || !check)
                    {
                        Console.WriteLine("Kontot kunde inte hittas. Försök igen...");
                        Thread.Sleep(5000);
                    }
                }

                while (true)
                {
                    bool check = false;
                    Console.Clear();
                    //Loop through all acounts
                    foreach (var myAcc in customer.BankAccounts)
                    {
                        if (myAcc.AccountNumber == sendFrom.AccountNumber)
                        {
                            //This will skip the account that we want to send money from
                        }
                        else
                        {
                            Console.WriteLine($"Account: [{myAcc.Name} - {myAcc.AccountNumber} has {myAcc.Amount} {myAcc.Currency}]");
                        }
                    }

                    Console.WriteLine("Write the name of the account you want to send money to: ");

                    sendToAcc = Console.ReadLine().ToLower();

                    sendTo = customer.BankAccounts.Find(x => x.Name.ToLower() == sendToAcc);

                    if (!string.IsNullOrEmpty(sendToAcc) && sendTo != null)
                    {
                        check = customer.BankAccounts.Contains(sendTo);

                        if (check)
                        {
                            break;
                        }

                        else
                        {
                            Console.WriteLine("Kontot kunde inte hittas. Försök igen.");
                            Thread.Sleep(5000);
                        }
                    }

                    else if (string.IsNullOrEmpty(sendToAcc) || !check)
                    {
                        Console.WriteLine("Fel värde. Du får försöka igen om några sekunder...");
                        Thread.Sleep(5000);
                    }
                }

                while (true)
                {
                    Console.WriteLine($"How much money in {sendFrom.Currency} do you want to send?");
                    bool check = double.TryParse(Console.ReadLine(), out amountToSend);

                    if (amountToSend <= sendFrom.Amount && amountToSend > 0)
                    {
                        break;
                    }
                }

                Send(sendFrom, sendTo, amountToSend, customer.UserName, customer.UserName);
            }

            else
            {
                Console.WriteLine("Du har inte skapat några konton än.");
            }
        }

        /// <summary>
        /// Used to send money from user account to another user account.
        /// </summary>
        public void MoneyToUser()
        {
            customer = (Customer)userList.Find(x => x.UserName == loggedInUser.UserName);

            //Annoying but is needed. Could probably be done with much less but what the heck, it works! :D
            string senderName = customer.UserName;
            string receiverName = "";
            string sendToAcc = "";
            string sendFromAcc = "";
            BankAccount sendTo;
            BankAccount sendFrom;
            Customer temp;            
            double amountToSend;

            //Will only work if user exists and has a bank account.
            if (customer.BankAccounts.Count > 0 && customer != null)
            {
                //Will loop until user inputs a valid account name to send money from.
                while (true)
                {
                    bool check = false;

                    Console.Clear();

                    //Printing out all of the current user's accounts
                    foreach (var myAcc in customer.BankAccounts)
                    {
                        Console.WriteLine($"Account: [{myAcc.Name} - {myAcc.AccountNumber} has {myAcc.Amount} {myAcc.Currency}]");
                    }

                    Console.WriteLine("Write the name of the account that you want to send money from: ");

                    sendFromAcc = Console.ReadLine().ToLower();

                    //Check to see if account exists and save in instance.
                    sendFrom = customer.BankAccounts.Find(x => x.Name.ToLower() == sendFromAcc);

                    //Error handling
                    if (!string.IsNullOrEmpty(sendFromAcc) && sendFrom != null)
                    {
                        check = customer.BankAccounts.Contains(sendFrom);

                        if (check)
                        {
                            break;
                        }
                    }

                    else if (string.IsNullOrEmpty(sendFromAcc) || !check)
                    {
                        Console.WriteLine("Account was not found. Did you enter the correct account name\n" +
                                          "Press enter to try again!");
                        Console.ReadKey();
                    }
                }

                //Loop until it finds the correct user to send money to.
                while (true)
                {
                    //Printing out all accounts available along with their user name.
                    for (int i = 0; i < userList.Count; i++)
                    {
                        //This prevents it from printing out:
                        //-admins (since they don't have accounts OR
                        //-The accounts of the user that is sending money.
                        if (userList[i].IsAdmin == false && userList[i].UserName != loggedInUser.UserName)
                        {
                            temp = (Customer)userList[i];

                            Console.WriteLine($"{temp.UserName} owns the following accounts: ");

                            for (int j = 0; j < temp.BankAccounts.Count; j++)
                            {
                                Console.WriteLine($"{temp.BankAccounts[j].Name} - {temp.BankAccounts[j].AccountNumber} has {temp.BankAccounts[j].Currency} as Currency.");
                            }

                            //Console.WriteLine(customer.BankAccounts[i]);
                        }
                    }

                    bool check = false;

                    Console.WriteLine("Skriv namnet på [personen] du vill skicka pengar till:");

                    receiverName = Console.ReadLine().ToLower();

                    customer = (Customer)userList.Find(x => x.UserName.ToLower() == receiverName);

                    if (!string.IsNullOrEmpty(receiverName) && customer != null && customer.UserName != loggedInUser.UserName)
                    {
                        check = customer.UserName.ToLower() == receiverName;

                        if (check)
                        {
                            break;
                        }
                    }

                    else if (customer.UserName == loggedInUser.UserName)
                    {
                        Console.WriteLine("Du får inte skriva dig själv som mottagare.");
                    }

                    else if (string.IsNullOrEmpty(sendFromAcc) || !check)
                    {
                        Console.WriteLine("Error.. Try again!");
                    }
                }

                //Loops until it finds the correct account to send money to.
                while (true)
                {
                    bool check = false;
                    Console.Clear();
                    Console.WriteLine("Välj bland följande konton: ");

                    for (int j = 0; j < customer.BankAccounts.Count; j++)
                    {
                        Console.WriteLine($"{customer.BankAccounts[j].Name} - {customer.BankAccounts[j].AccountNumber} " +
                                          $"has {customer.BankAccounts[j].Currency} as Currency.");
                    }

                    Console.WriteLine("Skriv namnet på kontot du vill skicka pengar till: ");

                    sendToAcc = Console.ReadLine().ToLower();
                    sendTo = customer.BankAccounts.Find(x => x.Name.ToLower() == sendToAcc);

                    if (!string.IsNullOrEmpty(sendToAcc) && sendTo != null)
                    {
                        check = sendTo.Name.ToLower() == sendToAcc;

                        if (check)
                        {
                            break;
                        }
                    }

                    else if (string.IsNullOrEmpty(sendFromAcc) || !check)
                    {
                        Console.WriteLine("Fel värde. Du får försöka igen om några sekunder...");
                        Thread.Sleep(5000);
                    }
                }

                //Loops until the amount to send is above 0 and less than what the sender owns.
                while (true)
                {
                    Console.WriteLine($"Hur mycket pengar i {sendFrom.Currency} önskar du skicka?");

                    double.TryParse(Console.ReadLine(), out amountToSend);

                    if (amountToSend <= sendFrom.Amount && amountToSend > 0)
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("Fel värde.. Försök igen..");
                    }
                }

                Send(sendFrom, sendTo, amountToSend, senderName, receiverName);
            }

            else
            {
                Console.WriteLine("Du har inte skapat några konton än.");
            }
        }

        /// <summary>
        /// Method that sends money from and to any user based on recieved parameters
        /// </summary>
        public void Send(BankAccount from, BankAccount to, double amount, string senderName, string receiverName)
        {
            int indexCustomerNum = 0;
            int indexAccNum = 0;
            double currencyChecked = amount;

            //If currencies are not equal
            if (from.Currency != to.Currency)
            {
                currencyChecked = changeRate.ExchangeCurrency(from, to, amount);
            }

            //Money is withdrawn from sender
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].IsAdmin == false)
                {
                    transactionFrom = (Customer)userList[i];
                    
                    for (int j = 0; j < transactionFrom.BankAccounts.Count; j++)
                    {
                        if (transactionFrom.BankAccounts[j].AccountNumber == from.AccountNumber)
                        {
                            customer = (Customer)userList.Find(x => x.UserName == transactionFrom.UserName);
                            customer.BankAccounts[j].Amount -= amount;
                            Console.WriteLine($"New balance is: {customer.BankAccounts[j].Amount} {customer.BankAccounts[j].Currency}");

                            customer.TransactionsSent.Add(new TransactionsSent(from, to, amount, receiverName));
                        }
                    }
                }
            }

            customer = (Customer)userList.Find(x => x.UserName == transactionFrom.UserName);
            //customer.TransactionsSent.Add(new TransactionsSent(from, to, amount, ))
            //Money is sent to reciever account
            for (int i = 0; i < userList.Count; i++)
            {
                if (userList[i].IsAdmin == false)
                {
                    transactionTo = (Customer)userList[i];

                    for (int j = 0; j < transactionTo.BankAccounts.Count; j++)
                    {
                        if (transactionTo.BankAccounts[j].AccountNumber == to.AccountNumber)
                        {
                            customer = (Customer)userList.Find(x => x.UserName == transactionTo.UserName);
                            customer.BankAccounts[j].Amount += currencyChecked;

                            Console.WriteLine($"{amount} {from.Currency} has successfully been sent to {userList[i].UserName} {to.Name}");

                            customer.TransactionsReceived.Add(new TransactionsReceived(from, to, amount, senderName));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Shows transactions done between accounts (Incoming and outgoing).
        /// </summary>
        public void ShowTransactions()
        {
            customer = (Customer)userList.Find(x => x.UserName == loggedInUser.UserName);

            if (customer.TransactionsSent.Count > 0 || customer.TransactionsReceived.Count > 0)
            {
                Console.WriteLine("Utgående transaktioner: ");

                foreach (var item in customer.TransactionsSent)
                {
                    Console.WriteLine($"To [{item.ToUser} - account number {item.To.AccountNumber}] - {item.Amount}");
                }

                if (customer.TransactionsReceived.Count > 0)
                {
                    Console.WriteLine("Inkommande transaktioner: ");
                    foreach (var item in customer.TransactionsReceived)
                    {
                        Console.WriteLine($"From [{item.FromUser} {item.From.AccountNumber}] Amount: {item.Amount}");
                    }
                }

                else
                {
                    Console.WriteLine("Det finns för närvarande inga inkommande transaktioner.");
                }
            }

            else
            {
                Console.WriteLine("Det finns för närvarande inga transaktioner.");
            }
        }

        /// <summary>
        /// Checking and adding up all amounts then approves loan OR customer can try entering another loan amount.
        /// Customer then chooses which account the money transfers to.
        /// </summary>
        /// 
        public void NewBankLoan()
        {
            Console.Clear();
            BankAccount newLoanToAcc;
            string inputAcc = "";
            double totalBankAmount = 0;
            string theEnd;
            double totalLoanAmount = (totalBankAmount * 5.0);
            double inputAmount;
            Customer allAcc = (Customer)loggedInUser;

            do
            {
                Console.WriteLine("Fyll i vilket belopp du vill låna.");
                inputAmount = Convert.ToDouble(Console.ReadLine());

                //loops through all the user accounts - adding up the amounts
                foreach (var item in allAcc.BankAccounts)  
                {
                    totalBankAmount += item.Amount;
                }

                //loan approved when loan is less than the total amount, else the customer can try another amount
                if (inputAmount <= totalLoanAmount)  
                {
                    Console.WriteLine($"Ditt lån på belopp {inputAmount} godkänns. Räntesatsen är 10 procent.");
                    Console.WriteLine($"Det totala lånebeloppet att betala tillbaka är {InterestRate(inputAmount)}.");
                    break;
                }
                else 
                {
                    Console.WriteLine($"Tyvärr kan inte ett lån på beloppet {inputAmount} godkännas."); 
                    Console.WriteLine("Vill du prova att ansöka om ett annat belopp: Ja eller Nej");
                    theEnd = Console.ReadLine();
                }
            } 
            while (theEnd.ToLower()!="nej"); 

            double approvedLoan = InterestRate(inputAmount);

            //customer can choose account to transfer money to
            while (true)
            {
                bool check = false;
                foreach (var item in customer.BankAccounts)
                {
                    Console.WriteLine($"Konto: [{item.Name} - {item.AccountNumber}, belopp {item.Amount} {item.Currency}]");
                }

                Console.WriteLine("Skriv namnet på det konto du vill att ditt nya lån ska överföras till: ");
                inputAcc = Console.ReadLine().ToLower();

                //Check to see if account exists and save in instance
                newLoanToAcc = customer.BankAccounts.Find(x => x.Name.ToLower() == inputAcc);  

                if (!string.IsNullOrEmpty(inputAcc) && newLoanToAcc != null)
                {
                    check = customer.BankAccounts.Contains(newLoanToAcc);  

                    if (check)
                    {
                        break;
                    }
                }
                else if (string.IsNullOrEmpty(inputAcc) || !check) 
                {
                    Console.WriteLine($"Kontot {inputAcc} existerar inte. Klicka på enter för att prova fylla i kontots namn igen.");
                    Console.ReadKey();
                }
            }
            newLoanToAcc.Amount = approvedLoan;
            
            Console.WriteLine($"Ditt lån har nu lagts till i ditt konto [{newLoanToAcc.Name}, kontonummer {newLoanToAcc.AccountNumber}]");
        }

        /// <summary>
        /// Formula for interest rate, returns the loan amount including interest
        /// </summary>
        public double InterestRate(double inputAmount) 
        {
            double amountWithRate = (inputAmount / 100.0) * 10.0 + inputAmount;
            return amountWithRate;
        }
    } 
}
