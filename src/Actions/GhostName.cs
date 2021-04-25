using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class GhostName : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public GhostName(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Retrieve the Ghost's name.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing GhostName");

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

				var ghost = level.GetGhost();
				if (ghost == null)
				{
					this.ircClient.SendPrivateMessage("The ghost hasn't spawned within the game yet.");
					return;
				}

				string msg = "The ghost's name is " + ghost.GetName() + ".";
				this.ircClient.SendPrivateMessage(msg);
			}
		}
	}
}