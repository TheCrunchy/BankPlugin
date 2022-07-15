using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Torch.Commands;
using Torch.Commands.Permissions;
using Torch.Mod;
using Torch.Mod.Messages;
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
            if (parsedAmount == 0 || parsedAmount < 0)
            {
                Context.Respond("Withdraw amount must be positive.");
                return;
            }

            if (Core.BankService.WithdrawMoney(Context.Player.SteamUserId, parsedAmount))
            {
                EconUtils.AddMoney(Context.Player.IdentityId, parsedAmount);
                Context.Respond($"Withdrew {parsedAmount.ToString():C} from your Bank", "Bank");

                Core.HistoryService.AddToHistory(Context.Player.SteamUserId, parsedAmount * -1, DateTime.Now, Core.BankService.GetBalance(Context.Player.SteamUserId));
                Core.Log.Info($"Bank Withdraw: {Context.Player.SteamUserId}, {parsedAmount} success");
            }
            else
            {
                Context.Respond($"Withdraw failed. Bank Balance: {Core.BankService.GetBalance(Context.Player.SteamUserId).ToString():C}", "Bank");
                Core.Log.Info($"Bank Withdraw: {Context.Player.SteamUserId}, {parsedAmount} failed");
            }
        }

        [Command("balance", "view bank balance")]
        [Permission(MyPromoteLevel.None)]
        public void BankBalance()
        {
            var balance = Core.BankService.GetBalance(Context.Player.SteamUserId);
            Context.Respond($"Balance {balance.ToString():C}", "Bank");
        }

        [Command("history", "withdraw from bank")]
        [Permission(MyPromoteLevel.None)]
        public void BankHistory()
        {
            var history = Core.HistoryService.GetHistory(Context.Player.SteamUserId);
            DialogMessage m = new DialogMessage("Bank History", Context.Player.SteamUserId.ToString(), history.GetOutputString());
            ModCommunication.SendMessageTo(m, Context.Player.SteamUserId);
        }

        [Command("deposit", "deposit to bank")]
        [Permission(MyPromoteLevel.None)]
        public void BankDeposit(string amount)
        {
            var parsedAmount = ParseStringToBalance(amount);
            if (parsedAmount == 0 || parsedAmount < 0)
            {
                Context.Respond("Deposit amount must be positive.");
                return;
            }
            if (EconUtils.GetBalance(Context.Player.IdentityId) < parsedAmount)
            {
                Context.Respond("You dont have that much money.");
                return;
            }
            if (Core.BankService.DepositMoney(Context.Player.SteamUserId, parsedAmount))
            {
                EconUtils.TakeMoney(Context.Player.IdentityId, parsedAmount);
                Context.Respond($"Deposited {parsedAmount.ToString():C} to your Bank", "Bank");
                Core.HistoryService.AddToHistory(Context.Player.SteamUserId, parsedAmount, DateTime.Now, Core.BankService.GetBalance(Context.Player.SteamUserId));
                Core.Log.Info($"Bank Deposit: {Context.Player.SteamUserId}, {parsedAmount} success");
            }
            else
            {
                Context.Respond($"Deposit failed.");
                Core.Log.Info($"Bank Deposit: {Context.Player.SteamUserId}, {parsedAmount} failed");
            }
        }
    }
}
