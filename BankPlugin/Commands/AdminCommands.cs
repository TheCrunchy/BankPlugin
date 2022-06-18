using Sandbox.Game.World;
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

namespace BankPlugin.Commands
{
    [Category("bankadmin")]
    public class AdminCommands : CommandModule
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
        [Permission(MyPromoteLevel.Admin)]
        public void BankWithdraw(string nameOrSteamId, string amount)
        {
            var parsedAmount = ParseStringToBalance(amount);
            if (parsedAmount == 0 || parsedAmount < 0)
            {
                Context.Respond("Withdraw amount must be positive.");
                return;
            }
            var playerIdentity = Core.GetIdentityByNameOrId(nameOrSteamId);
            if (playerIdentity == null)
            {
                Context.Respond("Couldnt find that player");
                return;
            }
            var steamId = MySession.Static.Players.TryGetSteamId(playerIdentity.IdentityId);
            if (BankPlugin.Core.BankService.WithdrawMoney(steamId, parsedAmount))
            {
                Context.Respond($"Withdrew {parsedAmount.ToString():C}", "Bank Admin");

                Core.HistoryService.AddToHistory(steamId, parsedAmount * -1, DateTime.Now, Core.BankService.GetBalance(steamId));
                Core.Log.Info($"Bank Withdraw: {steamId}, {parsedAmount} success");
            }
            else
            {
                Context.Respond($"Withdraw failed. Bank Balance: {BankPlugin.Core.BankService.GetBalance(steamId).ToString():C}", "Bank");
                Core.Log.Info($"Bank Withdraw: {steamId}, {parsedAmount} failed");
            }
        }

        [Command("balance", "view bank")]
        [Permission(MyPromoteLevel.Admin)]
        public void BankBalance(string nameOrSteamId)
        {
            var playerIdentity = Core.GetIdentityByNameOrId(nameOrSteamId);
            if (playerIdentity == null)
            {
                Context.Respond("Couldnt find that player");
                return;
            }
            var steamId = MySession.Static.Players.TryGetSteamId(playerIdentity.IdentityId);
            var balance = BankPlugin.Core.BankService.GetBalance(steamId);
            Context.Respond($"Balance {balance.ToString():C}", "Bank");
        }

        [Command("history", "withdraw from bank")]
        [Permission(MyPromoteLevel.Admin)]
        public void BankHistory(string nameOrSteamId)
        {
            var playerIdentity = Core.GetIdentityByNameOrId(nameOrSteamId);
            if (playerIdentity == null)
            {
                Context.Respond("Couldnt find that player");
                return;
            }
            var steamId = MySession.Static.Players.TryGetSteamId(playerIdentity.IdentityId);
            var history = BankPlugin.Core.HistoryService.GetHistory(steamId);
            DialogMessage m = new DialogMessage("Bank History", steamId.ToString(), history.GetOutputString());
            if (Context.Player != null)
            {
                ModCommunication.SendMessageTo(m, Context.Player.SteamUserId);
            }
            else
            {
                Context.Respond(history.GetOutputString());
            }
        }

        [Command("deposit", "deposit to bank admin")]
        [Permission(MyPromoteLevel.Admin)]
        public void BankDeposit(string nameOrSteamId, string amount)
        {
            var parsedAmount = ParseStringToBalance(amount);
            if (parsedAmount == 0 || parsedAmount < 0)
            {
                Context.Respond("Deposit amount must be positive.");
                return;
            }
            var playerIdentity = Core.GetIdentityByNameOrId(nameOrSteamId);
            if (playerIdentity == null)
            {
                Context.Respond("Couldnt find that player");
                return;
            }
            var steamId = MySession.Static.Players.TryGetSteamId(playerIdentity.IdentityId);
            if (BankPlugin.Core.BankService.DepositMoney(steamId, parsedAmount))
            {
                Context.Respond($"Deposited {parsedAmount.ToString():C}", "Bank Admin");
                Core.HistoryService.AddToHistory(steamId, parsedAmount, DateTime.Now, Core.BankService.GetBalance(steamId));
                Core.Log.Info($"Bank Admin Deposit: {steamId.ToString()}, {parsedAmount} success");
            }
            {
                Context.Respond($"Deposit failed.");
                Core.Log.Info($"Bank Admin Deposit: {steamId.ToString()}, {parsedAmount} failed");
            }
        }
    }
}
