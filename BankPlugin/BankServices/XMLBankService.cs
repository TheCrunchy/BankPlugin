using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankPlugin.BankObjects;

namespace BankPlugin.BankServices
{
    class XMLBankService : IBankService
    {
        private string _storagePath { get; set; }

        public XMLBankService(string storagePath) => _storagePath = storagePath;

        public Account CreateAccount(ulong steamid, long balance = 0)
        {
            if (!File.Exists($"{_storagePath}//BankPlugin//Data//Xml//{steamid}.Xml"))
            {
                Core.utils.WriteToXmlFile($"{_storagePath}//BankPlugin//Data//Xml//{steamid}.Xml", new Account() { Owner = steamid, Balance = balance });
            }
            return new Account() { Owner = steamid, Balance = balance };
        }

        public bool DepositMoney(ulong steamid, long amount)
        {
            try
            {
                var account = GetAccount(steamid);
                account.Balance += amount;
                Core.utils.WriteToXmlFile($"{_storagePath}//BankPlugin//Data//Xml//{steamid}.Xml", account);
                return true;
            }
            catch (Exception ex)
            {
                Core.Log.Error(ex.ToString());
                return false;
            }
        }

        public List<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();
            StringBuilder errors = new StringBuilder();
            foreach (string s in Directory.GetFiles($"{_storagePath}//BankPlugin//Data//Xml"))
            {
                try
                {
                    var account = Core.utils.ReadFromXmlFile<Account>(s);
                    accounts.Add(account);
                }
                catch (Exception ex)
                {
                    errors.Append($"Error Parsing {s} {ex}");
                    continue;
                }
            }
            Core.Log.Error(errors.ToString());
            return accounts;
        }

        public long GetBalance(ulong steamid)
        {
            try
            {
                var account = GetAccount(steamid);
                return account.Balance;
            }
            catch (Exception ex)
            {
                Core.Log.Error(ex.ToString());
                return 0;
            }
        }

        public AccountHistory GetHistory(ulong steamid)
        {
            var account = GetAccount(steamid);
            var temp = File.ReadLines($"{_storagePath}//BankPlugin//Data//History//{steamid}.csv").Skip(1).ToArray();
            var history = new AccountHistory();
            foreach (var line in temp)
            {
                var split = line.Split(',');
                AccountAction action = new AccountAction();
                action.Time = DateTime.Parse(split[0]);
                action.ChangeAmount = long.Parse(split[1]);
                action.BalanceAfterChange = long.Parse(split[2]);
                history.Actions.Add(action);
            }
            return history;
        }

        public bool WithdrawMoney(ulong steamid, long amount)
        {
            try
            {
                var account = GetAccount(steamid);
                if (account.Balance < amount)
                {
                    return false;
                }
                account.Balance -= amount;
                Core.utils.WriteToXmlFile($"{_storagePath}//BankPlugin//Data//Xml//{steamid}.Xml", account);

                return true;
            }
            catch (Exception ex)
            {
                Core.Log.Error(ex.ToString());
                return false;
            }
        }

        public Account GetAccount(ulong steamid)
        {
            if (File.Exists($"{_storagePath}//BankPlugin//Data//Xml//{steamid}.Xml"))
            {

                return Core.utils.ReadFromXmlFile<Account>($"{_storagePath}//BankPlugin//Data//Xml//{steamid}.Xml");
            }
            else
            {
                return CreateAccount(steamid, 0);
            }
        }
    }
}
