
using MelonLoader;
using System.Collections.Generic;

namespace Bitzophrenia {

	public class TwitchChannelPointRedemptionActionFactory {

		private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

		private Dictionary<string, Bitzophrenia.IAction> commands = new Dictionary<string, Bitzophrenia.IAction>();

		public TwitchChannelPointRedemptionActionFactory(Bitzophrenia.Twitch.TwitchIRCClient withIRCClient) {
			this.ircClient = withIRCClient;
		}

		public void Add(string guid, Bitzophrenia.IAction action) {
			commands.Add(guid, action);
		}

		public Bitzophrenia.IAction Find(string withGuid) {

			// only continue if there is a registered command
			if (!this.commands.ContainsKey(withGuid)) {
				return null;
			}

			this.commands.TryGetValue(withGuid, out Bitzophrenia.IAction cmd);
			return cmd;
		}
	}
}