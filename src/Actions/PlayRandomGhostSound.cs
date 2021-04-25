using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class PlayRandomGhostSound : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public PlayRandomGhostSound(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Get the ghost to randomly make a sound.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing PlayRandomGhostSound");

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
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}
				try
				{
					ghost.PlayRandomSound();
				}
				catch { }
			}
		}
	}
}