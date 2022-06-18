using BankPlugin.BankObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankPlugin
{
    public interface IHistoryService
    {
        AccountHistory GetHistory(ulong steamid);
        void AddToHistory(ulong steamid, long amount, DateTime time, long balanceAfterChange);
    }
}
