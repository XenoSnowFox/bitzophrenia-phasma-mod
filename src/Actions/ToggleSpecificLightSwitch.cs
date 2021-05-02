using MelonLoader;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class ToggleSpecificLightSwitch : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			private Bitzophrenia.Phasma.Objects.LightSwitch lightSwitch;

			public ToggleSpecificLightSwitch(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Phasma.Objects.LightSwitch withLightSwitch, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient)
					: base("Turn a specific light switch on/off.", phasmophobia)
			{
				this.lightSwitch = withLightSwitch;
				this.ircClient = withIRCClient;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing ToggleSpecificLightSwitch");

				if (!this.Phasmophobia().HasMissionStarted() || this.lightSwitch == null)
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				this.lightSwitch.Toggle();
			}
		}
	}
}