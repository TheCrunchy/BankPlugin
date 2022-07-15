using BankPlugin.BankObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankPlugin.BankServices
{
    public class CSVHistoryService : IHistoryService
    {
        private string _storagePath { get; set; }

        public CSVHistoryService(string storagePath) => _storagePath = storagePath;

        public AccountHistory GetHistory(ulong steamid)
        {
            if (!File.Exists($"{_storagePath}//BankPlugin//Data//History//{steamid}.csv"))
            {
                StringBuilder newHistory = new StringBuilder();
                newHistory.AppendLine("Time,ChangeAmount,BalanceAfterChange");
                File.WriteAllText($"{_storagePath}//BankPlugin//Data//History//{steamid}.csv", newHistory.ToString());
                return new AccountHistory();
            }

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

        public void AddToHistory(ulong steamid, long amount, DateTime time, long balanceAfterChange)
        {
            var history = GetHistory(steamid);

            var action = new AccountAction()
            {
                Time = time,
                ChangeAmount = amount,
                BalanceAfterChange = balanceAfterChange
            };

            history.Actions.Add(action);
            StringBuilder builder = new StringBuilder();
            foreach (var historyAction in history.Actions)
            {
                builder.Append($"{historyAction.Time},{historyAction.ChangeAmount},{historyAction.BalanceAfterChange}");
            }

            File.WriteAllText($"{_storagePath}//BankPlugin//Data//History//{steamid}.csv", builder.ToString());
        }
    }
}
