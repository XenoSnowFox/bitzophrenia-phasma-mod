using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class KillCurrentPlayer : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public KillCurrentPlayer(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Instant kill the streamer.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing KillCurrentPlayer");

				if (!this.Phasmophobia().HasMissionStarted())
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				var level = this.Phasmophobia().GetLevelController();
				if (level == null)
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				try
				{
					level.GetGameController()
							.GetMultiPlayerController()
							.GetPlayerObject()
							.Kill();

					this.ircClient.SendPrivateMessage("/me is no longer meant for this world.");
				}
				catch { }
			}
		}
	}
}