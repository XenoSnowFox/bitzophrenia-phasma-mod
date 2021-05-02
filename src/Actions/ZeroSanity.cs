using MelonLoader;
using System.Collections.Generic;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class ZeroSanity : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public ZeroSanity(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Reduce the streamers sanity level to zero.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing ZeroSanity");

				var sanity = this.Phasmophobia()
						?.GetGameController()
						?.GetPlayer()
						?.GetSanityObject();
				if (sanity == null)
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				sanity.SetSanityLevel(5);
				this.ircClient.SendPrivateMessage("/me has taken a massive toll to their sanity. Questioning what reality even is anymore.");
			}
		}
	}
}