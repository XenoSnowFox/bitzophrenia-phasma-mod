using MelonLoader;
using System.Collections.Generic;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class ToggleTorches : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			public ToggleTorches(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Turn all the torches on/off.", phasmophobia)
			{
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing ToggleTorches");

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

				List<Bitzophrenia.Phasma.Objects.Torch> torches = level.GetTorches();
				if (torches != null)
				{
					string msg = "Attempting to toggle " + torches.Count + " torches.";
					this.ircClient.SendPrivateMessage(msg);

					foreach (Bitzophrenia.Phasma.Objects.Torch torch in torches)
					{
						torch.Toggle();
						MelonLogger.Msg(torch.ToString());
					}
				}

				// loop over and toggle each player's over shoulder camera
				var game = level.GetGameController();
				if (game == null)
				{
					return;
				}
				var players = game.ListPlayers();
				if (players == null)
				{
					return;
				}
				foreach (var player in players)
				{
					try
					{
						var flashlight = player.GetPCFlashlight();
						if (flashlight == null)
						{
							continue;
						}

						flashlight.Toggle(false, false);
					}
					catch { }
				}
			}
		}
	}
}