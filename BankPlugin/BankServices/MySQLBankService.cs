using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankPlugin.BankObjects;

namespace BankPlugin.BankServices
{
    class MySQLBankService : IBankService
    {
        public void CreateAccount(ulong steamid)
        {
            throw new NotImplementedException();
        }

        public Account CreateAccount(ulong steamid, long balance)
        {
            throw new NotImplementedException();
        }

        public bool DepositMoney(ulong steamid, long amount)
        {
            throw new NotImplementedException();
        }

        public Account GetAccount(ulong steamid)
        {
            throw new NotImplementedException();
        }

        public List<Account> GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public long GetBalance(ulong steamId)
        {
            throw new NotImplementedException();
        }

        public List<AccountHistory> GetHistory(ulong steamid)
        {
            throw new NotImplementedException();
        }

        public bool WithdrawMoney(ulong steamid, long amount)
        {
            throw new NotImplementedException();
        }
    }
}
