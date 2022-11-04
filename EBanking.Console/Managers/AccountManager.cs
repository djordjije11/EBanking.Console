﻿using EBanking.Console.Model;
using EBanking.Console.Models;
using EBanking.Console.Validations.Exceptions;
using EBanking.Console.Validations.Impl;
using EBanking.Console.Validations.Interfaces;

namespace EBanking.Console.Managers
{
    internal class AccountManager : EntityManager<Account>
    {
        public AccountManager(IValidator<Account> validator) : base(validator)
        {
        }
        protected override string GetNameForGetId()
        {
            return "рачуна";
        }
        protected override string GetClassNameForScreen()
        {
            return "рачун";
        }
        protected override string GetPluralClassNameForScreen()
        {
            return "рачуни";
        }
        protected override string[] GetColumnNames()
        {
            return new string[] { "ИД", "Стање", "Статус", "Број", "Корисник", "Валута" };
        }
        protected override Account GetNewEntityInstance(int id = -1)
        {
            return new Account() { Id = id };
        }
        public override async Task<Account> ConstructEntityFromInput(int? id)
        {
            User wantedUser = await (new UserManager(new UserValidator()).FindEntityFromInput());
            Currency wantedCurrency = await (new CurrencyManager(new CurrencyValidator()).FindEntityFromInput());
            System.Console.WriteLine("Унесите стање рачуна:");
            if (!Decimal.TryParse(System.Console.ReadLine() ?? "", out decimal balance)) throw new ValidationException("Стање на рачуну мора бити број.");
            System.Console.WriteLine("Одаберите статус рачуна:\n1. Активан 2. Неактиван");
            string statusOption = System.Console.ReadLine() ?? "";
            while (!statusOption.Trim().Equals("1") && !statusOption.Trim().Equals("2"))
            {
                System.Console.WriteLine("Непозната опција. Покушајте опет.");
                System.Console.Clear();
                System.Console.WriteLine("Одаберите статус рачуна:\n1. Активан 2. Неактиван");
                statusOption = System.Console.ReadLine() ?? "";
            }
            Status status = (Status)Int32.Parse(statusOption);
            System.Console.WriteLine("Унесите број рачуна:");
            string accountNumber = System.Console.ReadLine() ?? "";
            var newAccount = new Account()
            {
                Balance = balance,
                Status = status,
                Number = accountNumber,
                User = wantedUser,
                Currency = wantedCurrency
            };
            if (id.HasValue) newAccount.SetIdentificator(id.Value);
            ValidateEntity(newAccount);
            return newAccount;
        }
    }
}
