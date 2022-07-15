using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankPlugin.BankObjects
{
    public class AccountHistory
    {
        public List<AccountAction> Actions = new List<AccountAction>();
        public string GetOutputString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var historyAction in Actions)
            {
                builder.Append($"{historyAction.Time},{historyAction.ChangeAmount},{historyAction.BalanceAfterChange}");
            }
            return builder.ToString();
        }
    }
}
