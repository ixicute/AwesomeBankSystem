# AwesomeBankSystem
## This program is a simulated bank with high security. It's usable for both customers and admin staff. The bank is called Awsome Bank and as a logged in customer you can open new bank accounts, send money to other users or between your own accounts, lend money, trade money in different currencies and print list of your own accounts and transactions. As a logged in admin staff you can add new customers and print list of all the customers.

## Classes
### User 
User class determines if user is admin och normal customer depending on name and password.

	  -Admin, 
	  Admin class inherits from User class. Admin class can add new users. 
	  -Customer, 
	  Customer class inherits from User class. This class contains different lists to handle bank accounts and transactions 
      between accounts. 
	
### BankAccount  
BankAccount abstract class handles new bank account and generates a random account number to a new account. 

	  -BaseAccount, 
	  BaseAccount class inherits from BankAccount class.
	  -SavingsAccount, 
	  SavingsAccount class inherits from BankAccount class and handles interest rate. 

### Transactions 
Transactions class handles new transactions. 

	  -TransactionsReceieved, 
	  TransactionsReceieved class inherits from Transactions class. When an account receives money a list is created of that 
      data type. 
	  -TransactionsSent, 
	  TransactionsSent class inherits from Transactions class. When an account sends money a list is created of that data type.
	
### CurrencyExchange 
CurrencyExchange class handles the different currencies in the bank system and admin staff have access to update the exchange rate of 
currencies USD and EURO.


### BankSystem
BankSystem class have a range of different methods that connects with the other classes in the program.

#### Methods:

Run - Starts the program. Checks if user have correct username and password. The user can't enter the incorrect data more than three times.

LoggedIn - Determines if logged in user is customer or admin and initiates method of AdminMenu or UserMenu. 

PrintBankAccount - Prints logged in customers accounts. 

OpenBankAccount -  Used to open a new bank account (normal or for savings) with a generated account number.

AddMoney - Lets user specify ammount of money to add and returns it as a double variable.

MoneyToSelf - Used to send money between the customers own accounts.

MoneyToUser - Used to send money from the customers account to another user account.

Send - Method that sends money from and to any user based on received parameters.

ShowTransactions - Shows transactions done between accounts, both incoming and outgoing transactions.

NewBankLoan - Checking and adding up all amounts then approves loan OR customer can try entering another loan amount. 
Customer then chooses which account the money transfers to.

InterestRate- Formula for interest rate, returns the loan amount including interest.

AddCustomer - Method for admin users to add new customers to the system. 

PrintAllCustomers - Method working together with method AddCustomer enables admin to print list of all customers.
