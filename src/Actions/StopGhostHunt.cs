using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class StopGhostHunt : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public StopGhostHunt(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Force the ghost to start a hunt.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing StopGhostHunt");

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

				ghost.EndHunt();
			}
		}
	}
}