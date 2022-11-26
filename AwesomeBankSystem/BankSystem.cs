using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Figgle;

namespace AwesomeBankSystem
{
    internal class BankSystem
    {
        User loggedInUser;
        Customer customer;
        Customer transactionFrom;
        Customer transactionTo;
        Admin admin;
        List<User> userList = new List<User>();
        CurrencyExchange changeRate = new CurrencyExchange();

        //user logging in: menu
        public void Run()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(FiggleFonts.Kban.Render("Awesome Bank"));
            InitiateUsers();
            int check = 0;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Välkommen till Awesome Bank!");
            Console.WriteLine("Logga in nedan\n");

            do
            {
                Console.WriteLine("Användarnamn: ");
                string name = Console.ReadLine();

                Console.WriteLine("Lösenord: ");
                string pass = Console.ReadLine();

                User temp = userList.Find(x => x.UserName == name && x.Password == pass);

                if (userList.Contains(temp))
                {
                    loggedInUser = temp;
                    LogIn();
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
                Console.WriteLine("Du har misslyckats tre gånger och blivit utlåst från systemet!");
            }

        }

        public void LogIn()
        {
            Console.WriteLine($"Du är nu inloggad som {loggedInUser.UserName}\n");
            Thread.Sleep(2000);
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
            Console.Clear();
            admin = (Admin)userList.Find(x => x.UserName == loggedInUser.UserName);
            Console.WriteLine(FiggleFonts.Kban.Render("Awesome Bank"));
            Console.WriteLine("Admin meny");
            string command = "";
            while (!command.ToLower().Equals("exit"))
            {
                Console.WriteLine("Skriv in vilket kommando du vill utföra.\n");
                Console.WriteLine("LK = Lägger till ny kund\n" +
                                  "SK = Skriva ut kundlista\n" +
                                  "VK = Ändra växelkurs\n" +
                                  "Exit = Avsluta programmet");
                command = Console.ReadLine();
                switch (command.ToLower())
                {
                    case "lk":
                        AddCustomer();
                        break;
                    case "sk":
                        PrintAllCustomers();
                        break;
                    case "vk":
                        changeRate.ChangeCurrencyExchange(admin);
                        break;
                    case "exit":
                        SignOut();
                        break;
                    default:
                        Console.WriteLine("Otillgängligt kommando");
                        break;
                }
            }
        }

        public void UserMenu()
        {
            
            string command = "";
            while (!command.ToLower().Equals("ex"))
            {
                Console.Clear();
                Console.WriteLine(FiggleFonts.Kban.Render("Awesome Bank"));
                Console.WriteLine("Kund meny");

                Console.WriteLine("\nVälj menyval genom att skriva in de bokstäver som motsvarar det du vill göra.\n");
                Console.WriteLine("SK - Skriv ut mina konton\n" +
                                  "OB - Öppna nytt konto\n" +
                                  "SP1 - Skicka pengar till konto du äger\n" +
                                  "SP2 - Skicka pengar till annan kund\n" +
                                  "NL - Ansök om nytt lån\n" +
                                  "VT - Visa alla transaktioner till och från dina konton\n" +
                                  "EX - Logga ut");
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
            customer = (Customer)userList.Find(x => x.UserName == "customer");
            customer.BankAccounts.Add(new BaseAccount("Baskonto", "SEK", 3500));
            userList.Add(new Customer("Aldor", "password", false));
            customer = (Customer)userList.Find(x => x.UserName == "Aldor");
            customer.BankAccounts.Add(new BaseAccount("Baskonto", "SEK", 1000));
        }

        public void AddCustomer()
        {
            Console.Clear();
            Console.WriteLine("Fyll i uppgifter nedan för att lägga till ny kund.");
            Console.WriteLine("Uppge användarnamn:");
            string username = Console.ReadLine();
            Console.WriteLine("Uppge lösenord:");
            string password = Console.ReadLine();
            Customer tempCustomer = new Customer(username, password, false);
            userList.Add(tempCustomer);
            Console.WriteLine($"\nKunden {tempCustomer.UserName} är nu tillagd i systemet!\n");
        }

        public void PrintAllCustomers()
        {
            Console.Clear();
            Console.WriteLine("Utskrift av kundlista:\n");
            foreach (User user in userList)
            {
                Console.WriteLine(user.UserName);
            }
        }

        public void SignOut()
        {
            //Todo
            Console.Clear();
            Console.WriteLine("\nDu loggas nu ut från Awesome Bank. Välkommen åter!");
            Thread.Sleep(2000);
        }

        public void PrintBankAccounts()
        {
            Console.Clear();
            Customer loggedInCustomer = (Customer)loggedInUser;
            if (loggedInCustomer.BankAccounts.Count < 1)
            {
                Console.WriteLine("Du har för tillfället inga bankkonton, vänligen skapa ett nytt konto genom att ange bokstäverna OB.\n");
            }
            foreach (BankAccount bankAccount in loggedInCustomer.BankAccounts)
            {
                Console.WriteLine($"Kontonamn: {bankAccount.Name} Kontonummer: {bankAccount.AccountNumber} Belopp: {bankAccount.Amount} {bankAccount.Currency}");
            }
            Console.WriteLine("Tryck ENTER för att fortsätta...");
            Console.ReadKey();
            Console.Clear();

        }

        /// <summary>
        /// Used to open a new bank account (normal or for savings) with a generated account number.
        /// </summary>
        public void OpenBankAccount()
        {
            Console.Clear();
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
                Console.WriteLine("Vilken typ av konto vill du öppna? Uppge ett nummer enligt lista nedan.\n" +
                  "1. Sparkonto\n" +
                  "2. Privatkonto\n" +
                  "3. Avbryt");

                //Error handling
                int.TryParse(Console.ReadLine(), out input);

                switch (input)
                {
                    case 1:
                        //Method that returns name of account.
                        accountName = SetAccountName(input);

                        //Method that returns name of currency.
                        currency = SetCurrency();

                        //Runs method with a menu to ask user for ammount to input and return to variable.
                        amount = AddMoney(currency);

                        customer.BankAccounts.Add(new SavingsAccount(accountName, currency, amount));

                        Console.WriteLine($"\nDen nuvarande räntan ligger på 10%.\n" +
                                          $"Du har valt att föra över {amount} {currency} till ditt nya konto. \n" +
                                          $"Med räntan inkluderad är det totala beloppet {customer.BankAccounts.Last().Amount} {currency}.");

                        Console.WriteLine($"Nytt konto med namn {customer.BankAccounts.Last().Name}, med tillhörande kontonummer [{customer.BankAccounts.Last().AccountNumber}] "
                                          + $"har lagts till i {customer.UserName}s kontolista.");
                        Console.ReadKey();

                        success = true;
                        break;

                    case 2:
                        //Method that returns name of account.
                        accountName = SetAccountName(input);

                        //Method that returns name of currency.
                        currency = SetCurrency();

                        //Runs method with a menu to ask user for ammount to input and return to variable.
                        amount = AddMoney(currency);

                        customer.BankAccounts.Add(new BaseAccount(accountName, currency, amount));
                        success = true;
                        break;

                    case 3:

                        Console.Clear();
                        Console.WriteLine("Avbryter menyval - Inga konton har skapats.");
                        input = 1;
                        break;

                    default:

                        Console.Clear();
                        Console.WriteLine("Vänligen välj ett menyval och försök igen.");
                        break;
                }

            } while (input != 1 && input != 2);

            Console.Clear();
            //If user reaches case 1 or case 2 (hence creating an account)
            if (success)
            {

                //If user pressed 1 then type is "Saving Account"
                //otherwise (aka if 2) then its "normal Account".
                type = input == 1 ? "Sparkonto" : "Privatkonto";

                Console.WriteLine($"Ett {type} har skapats med namn {accountName}.\n" +
                                  $"Kontonummer: {customer.BankAccounts.First(x => x.Name == accountName).AccountNumber}, i valuta {currency}.\n" +
                                  $"Nuvarande totalbelopp: {amount} {currency}\n");
            }
        }

        /// <summary>
        /// Used to set a name for a bank account, with default values.
        /// </summary>
        private string SetAccountName(int type)
        {
            string accountName = "";
            Console.WriteLine("\nVälj och fyll i namn för ditt nya konto: ");
            accountName = Console.ReadLine();

            if (string.IsNullOrEmpty(accountName) || string.IsNullOrWhiteSpace(accountName))
            {
                accountName = type == 1 ? "Sparkonto" : "Privatkonto";
            }
            Console.Clear();
            Console.WriteLine($"[{accountName}] har skapats.");

            return accountName;
        }

        /// <summary>
        /// Used to select currency-name from a menu.
        /// </summary>
        private string SetCurrency()
        {
            int currencyInput;
            string currencyName = "SEK";

            Console.WriteLine($"Alla konton har Svenska krona {currencyName} som standard valuta.\n" +
                              "Tryck 1 och ENTER om du vill välja ett annat valuta. Annars tryck bara ENTER. ");
            _ = int.TryParse(Console.ReadLine(), out currencyInput);

            Console.Clear();
            if (currencyInput == 1)
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
                    currencyName = "USD";
                }
                else if (temp == 2)
                {
                    currencyName = "EURO";
                }
            }

            else
            {
                currencyName = "SEK";
            }

            return currencyName;
        }

        /// <summary>
        /// Lets user specify ammount of money to add and returns it as a double-variable.
        /// </summary>
        private double AddMoney(string currency)
        {
            double result;
            Console.WriteLine($"Vilket belopp vill du föra över till ditt konto i valutan {currency}?");
            do
            {
                double.TryParse(Console.ReadLine(), out result);

                //If user enters a number less than 0 it will not be approved.
                if (result < 0)
                {
                    Console.WriteLine("Beloppet kan inte vara mindre än 0, vänligen försök igen!\n");
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
            if (customer.BankAccounts.Count >= 2 && customer != null)
            {
                while (true)
                {
                    Console.Clear();
                    bool check = false;
                    //Printing out all of the current user's accounts
                    foreach (var myAcc in customer.BankAccounts)
                    {
                        Console.WriteLine($"Konto: [{myAcc.Name} - {myAcc.AccountNumber}, tillgängligt belopp: {myAcc.Amount} {myAcc.Currency}]");
                    }

                    Console.WriteLine("Skriv in namnet på det konto du vill föra över pengar ifrån: ");

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
                        Console.WriteLine("Kontot kunde inte hittas. Vänligen försök igen om några sekunder.");
                        Thread.Sleep(3000);
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
                            Console.WriteLine($"Konto: [{myAcc.Name} - {myAcc.AccountNumber}, tillgängligt belopp: {myAcc.Amount} {myAcc.Currency}]");
                        }
                    }

                    Console.WriteLine("Skriv in namnet på det konto du vill föra över pengar till: ");

                    sendToAcc = Console.ReadLine().ToLower();

                    sendTo = customer.BankAccounts.Find(x => x.Name.ToLower() == sendToAcc);

                    if (!string.IsNullOrEmpty(sendToAcc) && sendTo != null && sendTo != sendFrom && sendToAcc != "ex")
                    {
                        check = customer.BankAccounts.Contains(sendTo);

                        if (check)
                        {
                            break;
                        }

                        else
                        {
                            Console.WriteLine("Kontot kunde inte hittas. Vänligen försök igen.");
                            Thread.Sleep(3000);
                        }
                    }

                    else if (string.IsNullOrEmpty(sendToAcc) || !check)
                    {
                        Console.WriteLine("Kontot kunde inte hittas. Vänligen försök igen om några sekunder.");
                        Thread.Sleep(5000);
                    }
                }

                while (true)
                {
                    Console.WriteLine($"Vilket belopp i {sendFrom.Currency} vill du föra över?");
                    bool check = double.TryParse(Console.ReadLine(), out amountToSend);

                    if (amountToSend <= sendFrom.Amount && amountToSend > 0)
                    {
                        break;
                    }
                }

                Send(sendFrom, sendTo, amountToSend, customer.UserName, customer.UserName);
            }

            else if (customer.BankAccounts.Count == 0)
            {
                Console.WriteLine("Du har inte skapat några konton än.");
            }

            else if (customer.BankAccounts.Count == 1)
            {
                Console.Clear();
                Console.WriteLine("Det har bara ett konto och kan därmed inte föra över pengar just nu.");
                Thread.Sleep(3000);
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
                    Console.WriteLine("Du äger följande konton: \n");
                    //Printing out all of the current user's accounts
                    foreach (var myAcc in customer.BankAccounts)
                    {
                        Console.WriteLine($"Konto: [{myAcc.Name} - {myAcc.AccountNumber}, tillgängligt belopp: {myAcc.Amount} {myAcc.Currency}]");
                    }

                    Console.WriteLine("\nSkriv in namnet på det konto du vill föra över pengar ifrån: ");

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
                        Console.WriteLine("Kontot kunde inte hittas. Skrev du in det korrekta namnet för kontot.\n" +
                                          "Vänligen tryck på ENTER för att försöka igen!");
                        Console.ReadKey();
                    }
                }

                Console.Clear();
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

                            Console.WriteLine($"\n{temp.UserName} har följande konton:\n");

                            for (int j = 0; j < temp.BankAccounts.Count; j++)
                            {
                                Console.WriteLine($"Konto: {temp.BankAccounts[j].Name} - [{temp.BankAccounts[j].AccountNumber}] har {temp.BankAccounts[j].Currency} som valuta.");
                            }

                            //Console.WriteLine(customer.BankAccounts[i]);
                        }
                    }

                    bool check = false;

                    Console.WriteLine("Skriv namnet på mottagaren du vill föra över pengar till:");

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

                    else if (customer != null)
                    {
                        if (customer.UserName == loggedInUser.UserName)
                        {
                            Console.WriteLine("Du får inte skriva dig själv som mottagare.");
                        }                        
                    }

                    else if (string.IsNullOrEmpty(sendFromAcc) || !check)
                    {
                        Console.WriteLine("Något blev fel! Vänligen försök igen!");
                        Thread.Sleep(2000);
                        Console.Clear();
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
                                          $"har {customer.BankAccounts[j].Currency} som valuta.");
                    }

                    Console.WriteLine("Skriv namnet på kontot du vill föra över pengar till: ");

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
                        Console.WriteLine("Fel värde. Du får försöka igen om några sekunder.");
                        Thread.Sleep(5000);
                    }
                }

                //Loops until the amount to send is above 0 and less than what the sender owns.
                while (true)
                {
                    Console.WriteLine($"Hur mycket pengar i {sendFrom.Currency} önskar du föra över?");

                    double.TryParse(Console.ReadLine(), out amountToSend);

                    if (amountToSend <= sendFrom.Amount && amountToSend > 0)
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("Fel värde. Vänligen försök igen.");
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
                            Console.WriteLine($"Nytt belopp är: {customer.BankAccounts[j].Amount} {customer.BankAccounts[j].Currency}");

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

                            Console.WriteLine($"{amount} {from.Currency} har nu förts över till {userList[i].UserName}s {to.Name}.\n");

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
            Console.Clear();
            customer = (Customer)userList.Find(x => x.UserName == loggedInUser.UserName);

            if (customer.TransactionsSent.Count > 0 || customer.TransactionsReceived.Count > 0)
            {
                Console.WriteLine("Utgående transaktioner: ");

                foreach (var item in customer.TransactionsSent)
                {
                    Console.WriteLine($"Till [{item.ToUser} - kontonummer {item.To.AccountNumber}] - {item.Amount}");
                }

                if (customer.TransactionsReceived.Count > 0)
                {
                    Console.WriteLine("Inkommande transaktioner: ");
                    foreach (var item in customer.TransactionsReceived)
                    {
                        Console.WriteLine($"Från [{item.FromUser} {item.From.AccountNumber}] Belopp: {item.Amount}");
                    }
                }

                else
                {
                    Console.WriteLine("Det finns för närvarande inga inkommande transaktioner.\n");
                }
            }

            else
            {
                Console.WriteLine("Det finns för närvarande inga transaktioner.\n");
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
            string theEnd = null;
            double inputAmount;
            bool check = false;
            double totalLoanAmount;
            Customer allAcc = (Customer)loggedInUser;

            do
            {
                Console.WriteLine("Fyll i vilket belopp du vill låna i valuta SEK:");
                inputAmount = Convert.ToDouble(Console.ReadLine());

                //loops through all the user accounts - adding up the amounts
                foreach (var item in allAcc.BankAccounts)
                {
                    totalBankAmount += item.Amount;
                }

                totalLoanAmount = (totalBankAmount * 5.0);

                if (inputAmount > totalLoanAmount)
                {
                    Console.WriteLine($"Tyvärr kan inte ett lån på beloppet {inputAmount} SEK godkännas.");
                    Console.WriteLine("Vill du prova att ansöka om ett annat belopp: Ja eller Nej");
                    theEnd = Console.ReadLine();
                    Console.Clear();
                }
                if (inputAmount <= totalLoanAmount)
                {
                    break;
                }
            }
            while (theEnd.ToLower() != "nej");

            if (inputAmount <= totalLoanAmount)
            {
                Console.WriteLine($"\nDitt lån på belopp {inputAmount} SEK godkänns. Räntesatsen är 10 procent.");
                Console.WriteLine($"Det totala lånebeloppet att betala tillbaka är {InterestRate(inputAmount)} SEK.\n");

                double approvedLoan = InterestRate(inputAmount);

                while (true)
                {
                    Console.WriteLine("Din kontolista:");

                    foreach (var item in customer.BankAccounts)
                    {
                        Console.WriteLine($"Konto: [{item.Name} - {item.AccountNumber}, belopp: {item.Amount} {item.Currency}]");
                    }

                    Console.WriteLine("\nSkriv namnet på det konto du vill att ditt nya lån ska överföras till: ");
                    inputAcc = Console.ReadLine().ToLower();

                    //Check to see if account exists and save in instance
                    newLoanToAcc = customer.BankAccounts.Find(x => x.Name.ToLower() == inputAcc);

                    if (!string.IsNullOrEmpty(inputAcc) && newLoanToAcc != null)
                    {
                        check = customer.BankAccounts.Contains(newLoanToAcc);

                        if (check)
                        {
                            newLoanToAcc.Amount += approvedLoan;
                            Console.WriteLine($"\nDitt lån har nu lagts till i ditt konto [{newLoanToAcc.Name}, kontonummer: {newLoanToAcc.AccountNumber}]");
                            break;
                        }
                    }
                    else if (string.IsNullOrEmpty(inputAcc) || !check)
                    {
                        Console.WriteLine($"Kontot {inputAcc} existerar inte. Klicka på ENTER för att prova fylla i kontots namn igen.\n");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
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
