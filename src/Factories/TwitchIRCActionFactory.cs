
using MelonLoader;
using System.Collections.Generic;

namespace Bitzophrenia {

	public class TwitchIRCActionFactory : Bitzophrenia.IAction {

		private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

		private Dictionary<string, Bitzophrenia.IAction> commands = new Dictionary<string, Bitzophrenia.IAction>();

		public TwitchIRCActionFactory(Bitzophrenia.Twitch.TwitchIRCClient withIRCClient) {
			this.ircClient = withIRCClient;
		}

		public void Add(string command, Bitzophrenia.IAction action) {
			commands.Add(command.ToLower(), action);
		}

		public string Summary() {
			return "Returns a list of commands that can be issued from chat.";
		}

		public void Execute() {
			MelonLogger.Msg("Execuing TwitchIRCActionFactory");

			var amounts = this.commands.Keys;
			foreach(string key in amounts) {
				this.commands.TryGetValue(key, out Bitzophrenia.IAction command);

				if (command == null || command == this) {
					continue;
				}

				this.ircClient.SendPrivateMessage(key + " -> " + command.Summary());
			}

		}

		public Bitzophrenia.IAction Find(string withMessage) {
            string command = withMessage.Trim().Split(' ')[0].ToLower();

			// only continue if the message begins with !
			if (!command.StartsWith("!")) {
				return null;
			}

			// only continue if there is a registered command
			if (!this.commands.ContainsKey(command)) {
				return null;
			}

			this.commands.TryGetValue(command, out Bitzophrenia.IAction cmd);
			return cmd;
		}
	}
}