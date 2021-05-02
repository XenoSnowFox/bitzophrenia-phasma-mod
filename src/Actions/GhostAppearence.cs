using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class GhostAppearence : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			private Bitzophrenia.Actions.GhostDisappearence ghostDisappearenceAction;

			public GhostAppearence(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Make the ghost appear for a short period of time.", phasmophobia)
			{
				this.ircClient = withIRCClient;
				this.ghostDisappearenceAction = new Bitzophrenia.Actions.GhostDisappearence(phasmophobia, withIRCClient);
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing GhostAppearence");

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

				var ghost = level.GetGhost();
				if (ghost == null)
				{
					this.ircClient.SendPrivateMessage("The ghost hasn't spawned within the game yet.");
					return;
				}

				ghost.Appear();
			}
		}
	}
}