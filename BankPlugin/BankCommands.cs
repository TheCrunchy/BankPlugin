using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch.Commands;
using Torch.Commands.Permissions;
using VRage.Game.ModAPI;
using VRageMath;

namespace BankPlugin
{
    [Category("bank")]
    public class BankCommands : CommandModule
    {
        public long ParseStringToBalance(string inputAmount)
        {
            long amount;
            inputAmount = inputAmount.Replace(",", "");
            inputAmount = inputAmount.Replace(".", "");
            inputAmount = inputAmount.Replace(" ", "");
            try
            {
                amount = long.Parse(inputAmount);
            }
            catch (Exception)
            {
                Context.Respond("Error parsing amount", Color.Red, "Bank Man");
                return 0;
            }
            if (amount < 0 || amount == 0)
            {
                Context.Respond("Must be a positive amount", Color.Red, "Bank Man");
                return 0;
            }

            return amount;
        }

        [Command("withdraw", "withdraw from bank")]
        [Permission(MyPromoteLevel.None)]
        public void BankWithdraw(string amount)
        {
            var parsedAmount = ParseStringToBalance(amount);
            if (parsedAmount == 0)
            {
                return;
            }
            if (BankPlugin.Core.BankService.WithdrawMoney(Context.Player.SteamUserId, parsedAmount))
            {
                EconUtils.AddMoney(Context.Player.IdentityId, parsedAmount);
                Context.Respond($"Withdrew {parsedAmount.ToString():C}", "Bank");
            }
            else
            {
                Context.Respond($"Withdraw failed. Bank Balance: {BankPlugin.Core.BankService.GetBalance(Context.Player.SteamUserId).ToString():C}", "Bank");
            }
        }

        [Command("balance", "withdraw from bank")]
        [Permission(MyPromoteLevel.None)]
        public void BankBalance()
        {
            var balance = BankPlugin.Core.BankService.GetBalance(Context.Player.SteamUserId);
            Context.Respond($"Balance {balance.ToString():C}", "Bank");
        }

        [Command("history", "withdraw from bank")]
        [Permission(MyPromoteLevel.None)]
        public void BankHistory()
        {
            var history = BankPlugin.Core.BankService.GetHistory(Context.Player.SteamUserId);
            
        }

        [Command("deposit", "withdraw from bank")]
        [Permission(MyPromoteLevel.None)]
        public void BankDeposit(string amount)
        {
            var parsedAmount = ParseStringToBalance(amount);
            if (parsedAmount == 0)
            {
                return;
            }
            if (EconUtils.GetBalance(Context.Player.IdentityId) < parsedAmount)
            {
                Context.Respond("You dont have that much money.");
                return;
            }
            if (BankPlugin.Core.BankService.DepositMoney(Context.Player.SteamUserId, parsedAmount))
            {
          
                EconUtils.TakeMoney(Context.Player.IdentityId, parsedAmount);
                Context.Respond($"Deposited {parsedAmount.ToString():C}", "Bank");
            }
            {
                Context.Respond($"Deposit failed.");
            }
        }
    }
}
