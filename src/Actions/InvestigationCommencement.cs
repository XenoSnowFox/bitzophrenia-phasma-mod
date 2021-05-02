using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class InvestigationCommencement : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public InvestigationCommencement(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Announces that the investigation has begun.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing InvestigationCommencement");

				if (!this.Phasmophobia().HasMissionStarted())
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				var level = this.Phasmophobia().GetLevelController();
				if (level == null)
				{
					return;
				}

				string msg = "/me and the gang have begun the investigation.";

				var ghost = level.GetGhost();
				if (ghost != null)
				{
					msg += " The ghost's name is " + ghost.GetName() + ".";
				}

				msg += " List available commands -> !commands";

				this.ircClient.SendPrivateMessage(msg);
			}
		}
	}
}