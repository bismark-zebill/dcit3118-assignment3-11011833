using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    // a. Record to represent financial data
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // b. Interface for processing transactions
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // c. Implementing classes for each processor type
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Bank Transfer] Processed {transaction.Category} of GHS {transaction.Amount} on {transaction.Date.ToShortDateString()}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Mobile Money] Processed {transaction.Category} of GHS {transaction.Amount} on {transaction.Date.ToShortDateString()}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Crypto Wallet] Processed {transaction.Category} of GHS {transaction.Amount} on {transaction.Date.ToShortDateString()}");
        }
    }

    // d. Base Account class
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
        }
    }

    // e. Sealed SavingsAccount class
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance)
        {
        }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds for transaction.");
            }
            else
            {
                base.ApplyTransaction(transaction);
                Console.WriteLine($"Transaction applied. New balance: GHS {Balance}");
            }
        }
    }

    // f. FinanceApp to simulate system
    public class FinanceApp
    {
        private List<Transaction> _transactions = new List<Transaction>();

        public void Run()
        {
            // i. Instantiate SavingsAccount
            SavingsAccount account = new SavingsAccount("ACC123", 1000m);

            // ii. Create 3 transactions
            var transaction1 = new Transaction(1, DateTime.Now, 150m, "Groceries");
            var transaction2 = new Transaction(2, DateTime.Now, 300m, "Utilities");
            var transaction3 = new Transaction(3, DateTime.Now, 200m, "Entertainment");

            // iii. Process with processors
            ITransactionProcessor mobileMoney = new MobileMoneyProcessor();
            ITransactionProcessor bankTransfer = new BankTransferProcessor();
            ITransactionProcessor cryptoWallet = new CryptoWalletProcessor();

            mobileMoney.Process(transaction1);
            bankTransfer.Process(transaction2);
            cryptoWallet.Process(transaction3);

            // iv. Apply transactions
            account.ApplyTransaction(transaction1);
            account.ApplyTransaction(transaction2);
            account.ApplyTransaction(transaction3);

            // v. Add to transactions list
            _transactions.Add(transaction1);
            _transactions.Add(transaction2);
            _transactions.Add(transaction3);
        }
    }

    // Main method
    public class Program
    {
        public static void Main(string[] args)
        {
            FinanceApp app = new FinanceApp();
            app.Run();
        }
    }
}
