using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class DropCurrentItem : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public DropCurrentItem(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Makes the streamer drop their current item.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing DropCurrentItem");

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
						.ForceDropProbs();

					string msg = "/me doesn't need this item. YEET!";
					this.ircClient.SendPrivateMessage(msg);
				}
				catch
				{

				}
			}
		}
	}
}