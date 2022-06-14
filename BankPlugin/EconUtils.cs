using Sandbox.Game.GameSystems.BankingAndCurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankPlugin
{
    public class EconUtils
    {
        public static long GetBalance(long walletID)
        {
            if (MyBankingSystem.Static.TryGetAccountInfo(walletID, out MyAccountInfo info))
            {
                return info.Balance;
            }
            return 0L;
        }
        public static void AddMoney(long walletID, Int64 amount)
        {
            MyBankingSystem.ChangeBalance(walletID, amount);

            return;
        }
        public static void TakeMoney(long walletID, Int64 amount)
        {
            if (GetBalance(walletID) >= amount)
            {
                amount = amount * -1;
                MyBankingSystem.ChangeBalance(walletID, amount);
            }
            return;
        }
    }
}
