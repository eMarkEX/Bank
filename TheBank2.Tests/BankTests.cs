using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBank2.Tests
{
    [TestFixture]
    public class BankTests
    {
        private IBank _theBank;

        [SetUp]
        public void Setup()
        {
            _theBank = new BankImpl();
        }

        [Test]
        public void CanCreateABalance()
        {
            _theBank.CreateAccount("David");
            var result = _theBank.AccountExists("David");
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void CanGetBalanceForNewlyCreatedAccount()
        {
            _theBank.CreateAccount("David");
            var result = _theBank.GetBalance("David");
            Assert.That(result, Is.EqualTo(0m).Within(0.0000001m));
        }


        [Test]
        public void AccountThatDoesNotExist()
        {
            var result = _theBank.AccountExists("Test");
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void ThrowsExceptionForAccountThatDoesNotExist()
        {
            Assert.Throws<Exception>(() => _theBank.GetBalance("Test"));
        }

        [Test]
        public void CanAddMoneyToAnAccount()
        {
            _theBank.CreateAccount("David");
            _theBank.Deposit("David", 100m);

            // act
            var result = _theBank.GetBalance("David");

            // assert
            Assert.That(result, Is.EqualTo(100m).Within(0.0000001m));
        }

        [Test]
        public void CanAddMoneySeveralTimesToAnAccount()
        {
            _theBank.CreateAccount("David");
            _theBank.Deposit("David", 100m);
            _theBank.Deposit("David", 400m);
            _theBank.Deposit("David", 350m);

            // act
            var result = _theBank.GetBalance("David");

            // assert
            Assert.That(result, Is.EqualTo(850m).Within(0.0000001m));
        }

        [Test]
        public void CannotDepositNegativeAmount()
        {
            _theBank.CreateAccount("David");
            _theBank.Deposit("David", 400m);

            Assert.Throws<ArgumentException>(() => _theBank.Deposit("David", -100m));
        }

        [Test]
        public void CanWithdrawMoney()
        {
            _theBank.CreateAccount("David");
            _theBank.Deposit("David", 400m);

            // act
            var result = _theBank.Withdraw("David", 100m);

            // assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(_theBank.GetBalance("David"), Is.EqualTo(300m));
        }

        [Test]
        public void CannotWithdrawMoneyWhenNotEnoughCash()
        {
            _theBank.CreateAccount("David");
            _theBank.Deposit("David", 400m);

            // act
            var result = _theBank.Withdraw("David", 500m);

            // assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(_theBank.GetBalance("David"), Is.EqualTo(400m));
        }

        [Test]
        public void CannotWithdrawNegativeAmount()
        {
            _theBank.CreateAccount("David");
            _theBank.Deposit("David", 400m);

            // act
            Assert.Throws<ArgumentException>(() => _theBank.Withdraw("David", -100m));
        }

        [Test]
        public void CanTransferMoney()
        {
            _theBank.CreateAccount("Hong");
            _theBank.CreateAccount("Emmanuel");
            _theBank.Deposit("Hong", 500m);
            _theBank.Deposit("Emmanuel", 500m);

            // act
            var result = _theBank.Transfer("Hong", "Emmanuel", 100m);

            // assert
            Assert.That(result, Is.EqualTo(true));
            Assert.That(_theBank.GetBalance("Hong"), Is.EqualTo(400m));
            Assert.That(_theBank.GetBalance("Emmanuel"), Is.EqualTo(600m));
        }


        [Test]
        public void CannnotTransferMoneyWhenMoreThanBalance()
        {
            _theBank.CreateAccount("Hong");
            _theBank.CreateAccount("Emmanuel");
            _theBank.Deposit("Hong", 500m);
            _theBank.Deposit("Emmanuel", 500m);

            // act
            var result = _theBank.Transfer("Hong", "Emmanuel", 1000m);

            // assert
            Assert.That(result, Is.EqualTo(false));
            Assert.That(_theBank.GetBalance("Hong"), Is.EqualTo(500m));
            Assert.That(_theBank.GetBalance("Emmanuel"), Is.EqualTo(500m));
        }


        [Test]
        public void CannnotTransferMoneyToNoneExistentBalance()
        {
            _theBank.CreateAccount("Hong");
            _theBank.Deposit("Hong", 500m);

            // act
            Assert.Throws<TransferNotPossibleException>(() => _theBank.Transfer("Hong", "Fake", 100m));

            // assert
            Assert.That(_theBank.GetBalance("Hong"), Is.EqualTo(500m));
        }

        [Test]
        public void TheSystemWith10MillionRecords()
        {
            for (int i = 1; i <= 10_000_000; i++)
            {
                _theBank.CreateAccount($"Balance {i}");
                _theBank.Deposit($"Balance {i}", i * 10m);
            }

            for (int i = 1; i <= 1000; i++)
            {
                var accountHolder = $"Balance {i}";
                Assert.True(_theBank.AccountExists(accountHolder));

                if (i % 7 == 0)
                {
                    var depositAmount = i + 20 - i % 11;

                    _theBank.Deposit(accountHolder, depositAmount);
                    var curBalance = _theBank.GetBalance(accountHolder);
                    Assert.That(curBalance, Is.EqualTo((i * 10m) + depositAmount));
                }

                if (i % 11 == 0)
                {
                    var balance = _theBank.GetBalance(accountHolder);
                    _theBank.Withdraw(accountHolder, balance / 2);
                    Assert.That(_theBank.GetBalance(accountHolder), Is.EqualTo(balance / 2).Within(0.0001m));
                }
            }
        }
    }
}
