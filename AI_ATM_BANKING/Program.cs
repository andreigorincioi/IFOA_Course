using System.Data;

namespace AI_ATM_BANKING
{
    class Program
    {
        // Declare a global account balance variable
        static string correctPin, Name, Surname;
        static decimal balance = 1000;
        static string cardNumber;
        static bool continuare = true;
        static string position = @"C:\Users\andre\source\repos\ATM_Banking\AI_ATM_BANKING\bin\Debug\net6.0\CARDS\";
        static void Main(string[] args)
        {
            bool ifNumbers;
            do
            {
                ifNumbers = InsertCardNumber();
                if (!ifNumbers)
                    Console.WriteLine("Card number accepts only numbers.");
                ifNumbers = DoesTheCardExist();
                if (!ifNumbers)
                    Console.WriteLine("The Card does not exist.\nPlease make sure the card is correct.");
            } while (!ifNumbers);
            

            if (InsertPin())
            {
                while (continuare)
                {
                    ShowMenu();
                }
            }
        }

        private static bool DoesTheCardExist()
        {
            FileInfo fi;
            FileStream fs;
            StreamReader sr;
            try
            {
                fi = new FileInfo(position);
                fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
                sr = new StreamReader(fs);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); return false; }

            try
            {
                correctPin = sr.ReadLine();
                Surname = sr.ReadLine();
                Name = sr.ReadLine();
                balance = Convert.ToDecimal(sr.ReadLine());
            }
            catch(Exception ex) { Console.WriteLine(ex.Message);return false; }
            finally
            {
                fs.Close();
                sr.Close();
            }
            return true;
        }
        private static bool InsertCardNumber()
        {
            Console.WriteLine("Please insert card number: ");
            cardNumber = Console.ReadLine();
            position += cardNumber + ".txt";
            for (int i = 0; i < cardNumber.Length; i++)
            {
                if (cardNumber[i] < 48 && cardNumber[i] > 57)
                {
                    return false;
                }
            }
            return true;
        }
        private static bool InsertPin()
        {
            string ioPin;
            Console.WriteLine("Pin: ");
            ioPin = Console.ReadLine();
            if (string.Compare(Program.correctPin, ioPin) != 0)
            {
                Console.WriteLine("The inserted pin is not correct.");
                return false;
            }
            return true;
        }
        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to the ATM Banking System");
            Console.WriteLine("1. Check balance");
            Console.WriteLine("2. Withdraw money");
            Console.WriteLine("3. Deposit money");
            Console.WriteLine("4. Exit");
            Console.WriteLine("Enter your choice: ");

            // Read the user's choice
            char choice = Console.ReadKey(true).KeyChar;
            Console.Clear();
            switch (choice)
            {
                case '1':
                    CheckBalance();
                    break;
                case '2':
                    WithdrawMoney();
                    break;
                case '3':
                    DepositMoney();
                    break;
                case '4':
                    Exit();
                    Console.WriteLine("Thank you for using our ATM. Goodbye!");
                    continuare = false;
                    break;
            }
            Console.Clear();
        }

        private static void Exit()
        {
            FileInfo fi;
            FileStream fs;
            StreamWriter sw;
            try
            {
                fi = new FileInfo(position);
                fs = fi.Open(FileMode.Open, FileAccess.Write, FileShare.Write);
                sw = new StreamWriter(fs);
                sw.WriteLine(correctPin);
                sw.WriteLine(Surname);
                sw.WriteLine(Name);
                sw.WriteLine(balance);
                sw.Close();
                fs.Close();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        private static void CheckBalance()
        {
            Console.WriteLine($"Your balance is {balance} eur.");
            Console.WriteLine($"Press button to exit.");
            Console.ReadLine();
        }

        private static void WithdrawMoney()
        {
            string[] ammountSel = new string[] { "1. 10", "2. 20", "3. 50", "4. 100", "5. 200", "6. 500", "7. Custom","8. Exit" };
            char sel = '7';
            int withdraw = 0;
            foreach (var item in ammountSel)
            {
                Console.WriteLine($"{item}");
            }
            Console.WriteLine("Select the ammount to withdraw...");
            try
            {
                sel = Console.ReadKey(true).KeyChar;
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            switch (sel)
            {
                case '1':
                    withdraw = 10;
                    break;
                case '2':
                    withdraw = 20;
                    break;
                case '3':
                    withdraw = 50;
                    break;
                case '4':
                    withdraw = 100;
                    break;
                case '5':
                    withdraw = 200;
                    break;
                case '6':
                    withdraw = 500;
                    break;
                case '7':
                    try
                    {
                        Console.Clear();
                        Console.Write("Withdraw: ");
                        withdraw = Convert.ToInt32(Console.ReadLine());
                        if (withdraw < 20)
                        {
                            Console.WriteLine("Transaction negated: the withdrawed ammount must be bigger than 20.");
                            return;
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message);return; }
                    break;
                case '8':
                    break;
            }
            if (withdraw > balance)
            {
                Console.WriteLine("INSUFFICIENT FUNDS");
                return;
            }
            balance -= withdraw;
        }
        private static void DepositMoney()
        {
            int insertedMoney = 0;
            Console.WriteLine("\tDepositMoney");
            Console.WriteLine("Please insert money: ");
            try
            {
                insertedMoney = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            balance += insertedMoney;
        }
    }
}