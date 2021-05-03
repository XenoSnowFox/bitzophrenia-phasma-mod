using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class GhostInteraction : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public GhostInteraction(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Make the Ghost interact with something.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing GhostInteraction");

				if (!this.Phasmophobia().HasMissionStarted())
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				this.Phasmophobia()
						?.GetLevelController()
						?.GetGhost()
						?.Interact();
			}
		}
	}
}