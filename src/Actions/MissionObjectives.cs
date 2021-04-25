using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class MissionObjectives : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public MissionObjectives(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("List the current mission objectives.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing MissionObjectives");

				if (this.Phasmophobia().HasMissionStarted())
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				var mission = this.Phasmophobia().GetMissionlController();
				if (mission == null || !mission.HasMissionsLoaded())
				{
					this.ircClient.SendPrivateMessage("The mission objectives are not ready yet.");
					return;
				}

				int i = 0;
				foreach (string objective in mission.GetMissions())
				{
					i++;
					this.ircClient.SendPrivateMessage("#" + i + " -> " + objective);
				}
			}
		}
	}
}