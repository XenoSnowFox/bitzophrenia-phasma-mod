
using MelonLoader;
using System.Collections.Generic;

namespace Bitzophrenia {

	public class TwitchBitRedemptionActionFactory : Bitzophrenia.IAction {

		private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

		private Dictionary<int, Bitzophrenia.IAction> commands = new Dictionary<int, Bitzophrenia.IAction>();

		public TwitchBitRedemptionActionFactory(Bitzophrenia.Twitch.TwitchIRCClient withIRCClient) {
			this.ircClient = withIRCClient;
		}

		public void Add(int amount, Bitzophrenia.IAction action) {
			commands.Add(amount, action);
		}

		public string Summary() {
			return "Returns a list of commands that can be redeemed with Bits.";
		}

		public void Execute() {
			MelonLogger.Msg("Execuing TwitchBitRedemptionActionFactory");

			var amounts = this.commands.Keys;
			foreach(int key in amounts) {
				this.commands.TryGetValue(key, out Bitzophrenia.IAction command);

				if (command == null || command == this) {
					continue;
				}

				this.ircClient.SendPrivateMessage(key + " -> " + command.Summary());
			}

		}

		public Bitzophrenia.IAction Find(int withAmount) {

			// only continue if there is a registered command
			if (!this.commands.ContainsKey(withAmount)) {
				return null;
			}

			this.commands.TryGetValue(withAmount, out Bitzophrenia.IAction cmd);
			return cmd;
		}
	}
}