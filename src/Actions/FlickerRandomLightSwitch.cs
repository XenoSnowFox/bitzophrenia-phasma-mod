using MelonLoader;
using System.Collections.Generic;
using System.Threading;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class FlickerRandomLightSwitch : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			private Queue<Bitzophrenia.IAction> actionQueue;

			private int currentlyFlickering = 0;

			public FlickerRandomLightSwitch(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient, Queue<Bitzophrenia.IAction> withActionQueue)
					: base("Flicker a randomly selected light switch.", phasmophobia)
			{
				this.ircClient = withIRCClient;
				this.actionQueue = withActionQueue;
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing FlickerRandomLightSwitch");

				if (!this.Phasmophobia().HasMissionStarted())
				{
					this.ircClient.SendPrivateMessage("The investigation has not started yet.");
					return;
				}

				// check if the fuse box is switched on

				// add a limit to the number of lights that can flicker
				if (currentlyFlickering > 1) {
					this.ircClient.SendPrivateMessage("Too many lights are already being flickered.");
					return;
				}

				this.currentlyFlickering += 1;

				// find a light swtich
				var lightSwitchList = new List<Bitzophrenia.Phasma.Objects.LightSwitch>();
				foreach (var room in this.Phasmophobia()
						.GetLevelController()
						.ListAllRooms()) {
					lightSwitchList.AddRange(room.ListLightScwitches());
				}

				var rnd = UnityEngine.Random.Range(0, lightSwitchList.Count);
				this.ExecuteTask(lightSwitchList[rnd]);
			}

			private void ExecuteTask(Bitzophrenia.Phasma.Objects.LightSwitch withLightSwitch) {
				new Thread(() => {
					// get the original state of the light (ie on/off)
					bool initialState = withLightSwitch.IsSwitchedOn();

					int currentDuration = UnityEngine.Random.Range(3333, 6666);
					do {
						int timeout = UnityEngine.Random.Range(30, 400);
						this.actionQueue.Enqueue(new Bitzophrenia.Actions.ToggleSpecificLightSwitch(this.Phasmophobia(), withLightSwitch, this.ircClient));
						Thread.Sleep(timeout);
						currentDuration -= timeout;
					} while (currentDuration > 0 && this.Phasmophobia().HasMissionStarted());

					// reset light to either on/off
					withLightSwitch.Use(initialState);

					this.currentlyFlickering -= 1;
				}).Start();
			}
		}
	}
}