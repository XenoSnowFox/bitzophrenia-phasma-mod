#define MOD_ENABLED

using MelonLoader;
using System.Collections.Generic;
using Console = System.Console;
using UnityEngine.InputSystem;

namespace Bitzophrenia
{
	public class Main : MelonMod
	{

		public Bitzophrenia.Phasma.Global Phasmophobia = new Bitzophrenia.Phasma.Global();

		public Bitzophrenia.Twitch.TwitchController twitchController;

		private Bitzophrenia.TwitchIRCActionFactory ircActionFactory = null;

		private Bitzophrenia.TwitchBitRedemptionActionFactory bitRedemptionFactory = null;

		private Bitzophrenia.TwitchChannelPointRedemptionActionFactory channelPointRedemptionFactory = null;

		private Queue<Bitzophrenia.IAction> actionQueue = new Queue<IAction>();

		private static void Log(string withMessage) {
			MelonLogger.Msg("[MAIN] " + withMessage);
		}

#if (MOD_ENABLED)
		public override void OnApplicationStart()
		{
			BasicInjection.Main();
			Main.Log("Starting Application");

			Main.Log("Set console title to: Phasmophobia");
			Console.Title = string.Format("Phasmophobia");

			// initialize twitch client
			this.twitchController = new Bitzophrenia.Twitch.TwitchController(Bitzophrenia.Properties.Twitch.ClientId);
			var ircClient = this.twitchController.GetIRCClient();
			if (ircClient != null) {
				ircClient.AddOnPrivateMessageDelegate(this.HandleTwitchIRCMessage);
			}
			var pubSubClient = this.twitchController.GetPubSubClient();
			if (pubSubClient != null) {
				pubSubClient.AddOnChannelPointRedemptionDelegate(this.HandleTwitchChannelPointRedemption);
			}

			// set up the action factories
			this.ircActionFactory = new Bitzophrenia.TwitchIRCActionFactory(ircClient);
			this.channelPointRedemptionFactory = new Bitzophrenia.TwitchChannelPointRedemptionActionFactory(ircClient);
			this.bitRedemptionFactory = new Bitzophrenia.TwitchBitRedemptionActionFactory(ircClient);

			// set up callback for when an investigation starts
			// this will publish a message in chat
			this.Phasmophobia.AddOnMissionStartAction(new Bitzophrenia.Actions.InvestigationCommencement(this.Phasmophobia, ircClient));

			// load IRC commands
			this.ircActionFactory.Add("!commands", this.ircActionFactory);
			this.ircActionFactory.Add("!bits", this.bitRedemptionFactory);

			this.ircActionFactory.Add("!appear", new Bitzophrenia.Actions.GhostAppearence(this.Phasmophobia, ircClient));
			this.ircActionFactory.Add("!drop", new Bitzophrenia.Actions.DropCurrentItem(this.Phasmophobia, ircClient));
			this.ircActionFactory.Add("!flicker", new Bitzophrenia.Actions.FlickerRandomLightSwitch(this.Phasmophobia, ircClient, this.actionQueue));
			this.ircActionFactory.Add("!ghost",  new Bitzophrenia.Actions.GhostName(this.Phasmophobia, ircClient));
			this.ircActionFactory.Add("!hey", new Bitzophrenia.Actions.PlayRandomGhostSound(this.Phasmophobia, ircClient));
			this.ircActionFactory.Add("!objectives", new Bitzophrenia.Actions.MissionObjectives(this.Phasmophobia, ircClient));
			this.ircActionFactory.Add("!sanity", new Bitzophrenia.Actions.PlayerSanity(this.Phasmophobia, ircClient));
			this.ircActionFactory.Add("!torch", new Bitzophrenia.Actions.ToggleTorches(this.Phasmophobia, ircClient));

			// load BIT commands
			this.bitRedemptionFactory.Add(500, new Bitzophrenia.Actions.StartGhostHunt(this.Phasmophobia, ircClient, this.actionQueue));
			this.bitRedemptionFactory.Add(666, new Bitzophrenia.Actions.KillCurrentPlayer(this.Phasmophobia, ircClient));
			this.bitRedemptionFactory.Add(999, new Bitzophrenia.Actions.KillSpaceMonkey(this.Phasmophobia, ircClient));

			// load CHANNEL POINT redemptions
			// this.channelPointRedemptionFactory.Add("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", new Bitzophrenia.Actions.FlickerRandomLightSwitch(this.Phasmophobia, ircClient, this.actionQueue));
		}

		public override void OnApplicationQuit()
		{
			Main.Log("Quitting Application");
			try {
				this.twitchController
						.GetIRCClient()
						.Disconnect();
			} catch { }
			try {
				this.twitchController
						.GetPubSubClient()
						.Disconnect();
			} catch { }
		}

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			Main.Log("OnSceneWasLoaded: [" + buildIndex + "] " + sceneName);
		}

		public override void OnSceneWasInitialized(int buildIndex, string sceneName)
		{
			Main.Log("OnSceneWasInitialized: [" + buildIndex + "] " + sceneName);
			this.Phasmophobia.Reset();
			this.Phasmophobia.SetCurrentScene(buildIndex, sceneName);
		}

		/**
		 * Called at the end of each Update call
		 */
		public override void OnUpdate()
		{
			// tempory fallbacks in the event that Bit redemptions do not work
			try {
				if (this.Phasmophobia.IsInLevel() && this.Phasmophobia.HasMissionStarted()) {
					Keyboard keyboard = Keyboard.current;

					// NUMPAD 0
					if (keyboard.numpad0Key.wasPressedThisFrame) {
						this.HandleTwitchBitRedemption("", 666);
					}

					// NUMPAD 1
					if (keyboard.numpad1Key.wasPressedThisFrame) {
						this.HandleTwitchBitRedemption("", 500);
					}

					// NUMPAD 2
					if (keyboard.numpad2Key.wasPressedThisFrame) {
						this.HandleTwitchBitRedemption("", 100);
					}

					// NUMPAD 3
					if (keyboard.numpad3Key.wasPressedThisFrame) {
						// this.Phasmophobia.GetGameController()
						// 		?.GetPlayer()
						// 		?.GetSanityObject()
						// 		?.Log();
					}

					// NUMPAD 7
					if (keyboard.numpad7Key.wasPressedThisFrame) {
						this.HandleTwitchBitRedemption("", 999);
					}

					// NUMPAD 8
					if (keyboard.numpad3Key.wasPressedThisFrame) {
						this.HandleTwitchIRCMessage("", "!flicker");
					}

					// // NUMPAD 9
					// if (keyboard.numpad9Key.wasPressedThisFrame) {
					// 	var sanity = this.Phasmophobia.GetGameController()
					// 			.GetPlayer()
					// 			.GetSanityObject()
					// 			.GetInstance();
					// 	//sanity.ChangeSanity(100);
					// 	//sanity.UpdatePlayerSanity();
					// 	sanity.NetworkedUpdatePlayerSanity(0);
					// }
				}
			} catch {
				MelonLogger.Error("Error in the `onUpdate` method");
			}

			// Run our main update loop
			try {
				this.Phasmophobia.Update();
				this.ExecuteQueueActions();
			} catch {
				MelonLogger.Error("Error in the `onUpdate` method");
			}
		}
#endif

		private void ExecuteQueueActions() {

			if (this.actionQueue.Count <= 0) {
				return;
			}
			Main.Log(this.actionQueue.Count + " actions were queued");

			HashSet<Bitzophrenia.IAction> hashSet = new HashSet<IAction>();
			while (this.actionQueue.Count > 0) {
				var action = this.actionQueue.Dequeue();
				if (action == null) {
					continue;
				}
				if (hashSet.Add(action)) {
					try {
						action.Execute();
					} catch {}
				}
			}
			Main.Log(hashSet.Count + " actions have been executed");
		}

		private void HandleTwitchIRCMessage(string withUsername, string withMessage) {
			var action = this.ircActionFactory.Find(withMessage);
			if (action == null) {
				return;
			}

			actionQueue.Enqueue(action);
		}

		private void HandleTwitchBitRedemption(string withUsername, int withAmount) {
			var action = this.bitRedemptionFactory.Find(withAmount);
			if (action == null) {
				return;
			}

			actionQueue.Enqueue(action);
		}

		private void HandleTwitchChannelPointRedemption(string withUsername, string withGuid) {
			var action = this.channelPointRedemptionFactory.Find(withGuid);
			if (action == null) {
				return;
			}

			actionQueue.Enqueue(action);
		}

	}
}
