using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBank2
{
    public class Balance
    {
        private decimal _balance = 0m;

        public Balance(string accountHolder)
        {
            AccountHolder = accountHolder;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be more than 0");

            _balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be more than 0");

            if (_balance - amount < 0)
                return false;

            _balance = _balance - amount;
            return true;
        }

        public decimal GetBalance => _balance;

        public string AccountHolder { get; }
    }
}
