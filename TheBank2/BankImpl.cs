using System;
using System.Collections.Generic;

namespace TheBank2
{
    public class BankImpl : IBank
    {
        public Dictionary<String, decimal> holdaccounts = new Dictionary<string, decimal>();
        public bool AccountExists(string accountHolder)
        {
            bool doesaccountexist = false;
            if (holdaccounts.ContainsKey(accountHolder))
            {
                doesaccountexist = true;
            }
            else if (!holdaccounts.ContainsKey(accountHolder))
            {
                doesaccountexist = false;
            }

            return doesaccountexist;
        }

        public void CreateAccount(string accountHolder)
        {
            // do nothing
            holdaccounts.Add(accountHolder, 0m);
        }

        public void Deposit(string accountHolder, decimal amount)
        {
      
            if (amount < 0)
            {
                throw new ArgumentException("Amount must be more than 0");
            }
            else
            {
                decimal currentamount = holdaccounts[accountHolder];
                holdaccounts[accountHolder] = amount + currentamount;
            }
        }

        public decimal GetBalance(string accountHolder)
        {
            return holdaccounts[accountHolder];
        }

        public bool Transfer(string fromAccount, string toAccount, decimal amount)
        {
            bool cantransfer = false;
            decimal fromaccountamount = holdaccounts[fromAccount];
            decimal toaccountamount = holdaccounts[toAccount];
       

            if (amount > fromaccountamount)
            {
                Console.WriteLine("You are transfering more money than your account has");
            }
            else if (fromaccountamount < 0)
            {
                throw new ArgumentException("You cannot transfer no or negative money");
            }
            else if(!holdaccounts.ContainsKey(toAccount))
            {
                throw new TransferNotPossibleException();
               
            }
            else
            {
                cantransfer = true;
                holdaccounts[toAccount] = toaccountamount + amount;
                holdaccounts[fromAccount] = fromaccountamount - amount;
            }

            return cantransfer;
        }

        public bool Withdraw(string accountHolder, decimal amount)
        {
            bool canwithdraw = false;
            decimal newamount = holdaccounts[accountHolder];

            if (amount > holdaccounts[accountHolder])
            {
                Console.WriteLine("You are withdrawing more than you can withdraw");
            }
            else if (amount < 0)
            {
                throw new ArgumentException("You are withdrawing a negative amount");
            }
            else
            {
                canwithdraw = true;
                newamount = newamount - amount;
                holdaccounts[accountHolder] = newamount;
            }

            return canwithdraw;
        }
    }
}
