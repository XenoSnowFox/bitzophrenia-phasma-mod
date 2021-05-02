using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class PlayerSanity : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public PlayerSanity(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("List the current sanity levels of each player.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing PlayerSanity");

				if (!this.Phasmophobia().HasMissionStarted())
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				var game = this.Phasmophobia().GetGameController();
				if (game == null)
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				var players = game.ListPlayers();
				if (players == null) {
					return;
				}

				foreach(var player in players) {
					var sanity = player.GetSanityObject();
					if (sanity == null) {
						continue;
					}

					this.ircClient.SendPrivateMessage(player.getNickName() + " -> " + sanity.ToString());
				}
			}
		}
	}
}