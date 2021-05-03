using MelonLoader;
using System.Collections.Generic;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class TurnOffFuseBox : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public TurnOffFuseBox(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Turn off the fuse box.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing TurnOffFuseBox");

				if (!this.Phasmophobia().HasMissionStarted())
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				var fuseBox = this.Phasmophobia()
						?.GetLevelController()
						?.GetFuseBox();

				if (fuseBox == null)
				{
					this.ircClient.SendPrivateMessage("Unable to turn off the Fuse Box.");
					return;
				}

				fuseBox.TurnOff();
				this.ircClient.SendPrivateMessage("/me can't afford this power bill. Time to switch off the fuse box.");
			}
		}
	}
}