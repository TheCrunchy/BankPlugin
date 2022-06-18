using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankPlugin.BankObjects;

namespace BankPlugin.BankServices
{
    class JsonBankService : IBankService
    {
        private string _storagePath { get; set; }

        public JsonBankService(String storagePath)
        {
            _storagePath = storagePath;
        }

        public Account CreateAccount(ulong steamid, long balance = 0)
        {
            if (!File.Exists($"{_storagePath}//BankPlugin//Data//Json//{steamid.ToString()}.json"))
            {
                Core.utils.WriteToJsonFile<Account>($"{_storagePath}//BankPlugin//Data//Json//{steamid.ToString()}.json", new Account() { Owner = steamid, Balance = balance });
            }
            return new Account() { Owner = steamid, Balance = balance };
        }

        public bool DepositMoney(ulong steamid, long amount)
        {
            try
            {
                var account = GetAccount(steamid);
                account.Balance += amount;
                Core.utils.WriteToJsonFile<Account>($"{_storagePath}//BankPlugin//Data//Json//{steamid.ToString()}.json", account);
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
            foreach (String s in Directory.GetFiles($"{_storagePath}//BankPlugin//Data//Json"))
            {
                try
                {
                    var account = Core.utils.ReadFromJsonFile<Account>(s);
                    accounts.Add(account);
                }
                catch (Exception ex)
                {
                    errors.Append($"Error Parsing {s} {ex.ToString()}");
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
                Core.utils.WriteToJsonFile<Account>($"{_storagePath}//BankPlugin//Data//Json//{steamid.ToString()}.json", account);
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
            if (File.Exists($"{_storagePath}//BankPlugin//Data//Json//{steamid.ToString()}.json"))
            {
                return Core.utils.ReadFromJsonFile<Account>($"{_storagePath}//BankPlugin//Data//Json//{steamid.ToString()}.json");
            }
            else
            {
                return CreateAccount(steamid, 0);
            }
        }
    }
}
