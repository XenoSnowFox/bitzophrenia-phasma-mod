using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class KillSpaceMonkey : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public KillSpaceMonkey(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Instant kill Space Monkey.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing KillSpaceMonkey");

				if (!this.Phasmophobia().HasMissionStarted())
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				try
				{
					var players = this.Phasmophobia()
								?.GetGameController()
								?.ListPlayers();
					if (players != null) {
						foreach(var player in players) {
							if (player.getNickName()
									.ToLower()
									.Contains("space")) {
								player.Kill();

								this.ircClient.SendPrivateMessage("Chat has decreed that Space Moneky's time has come.");
								return;
							}
						}
					}
				}
				catch { }
			}
		}
	}
}