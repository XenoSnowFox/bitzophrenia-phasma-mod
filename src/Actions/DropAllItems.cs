using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class DropAllItems : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public DropAllItems(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Makes the streamer drop all their items.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing DropAllItems");

				if (this.Phasmophobia().HasMissionStarted())
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
						.ForceDropProbs();

					string msg = "We don't need all these items.";
					this.ircClient.SendPrivateMessage(msg);
				}
				catch
				{

				}
			}
		}
	}
}