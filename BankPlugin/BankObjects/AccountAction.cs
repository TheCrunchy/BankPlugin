using System;

namespace BankPlugin.BankObjects
{
    public class AccountAction
    {
        public long BalanceAfterChange { get; set; }
        public long ChangeAmount { get; set; }
        public DateTime Time { get; set; }
    }
}