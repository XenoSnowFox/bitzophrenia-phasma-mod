#define MOD_ENABLED

using MelonLoader;
using System.Collections.Generic;
using Console = System.Console;

namespace Bitzophrenia
{
	public class Main : MelonMod
	{

		public Bitzophrenia.Phasma.Global Phasmophobia = new Bitzophrenia.Phasma.Global();

		public Bitzophrenia.Twitch.TwitchController twitchController;

		private Bitzophrenia.TwitchIRCActionFactory ircActionFactory = null;

		private Bitzophrenia.TwitchBitRedemptionActionFactory bitRedemptionFactory = null;

		private Queue<Bitzophrenia.IAction> actionQueue = new Queue<IAction>();

#if (MOD_ENABLED)
		public override void OnApplicationStart()
		{
			BasicInjection.Main();
			MelonLogger.Msg("Starting Application");

			MelonLogger.Msg("Set console title to: Phasmophobia");
			Console.Title = string.Format("Phasmophobia");

			// initialize twitch client
			this.twitchController = new Bitzophrenia.Twitch.TwitchController(Bitzophrenia.Properties.Twitch.ClientId);
			var ircClient = this.twitchController.GetIRCClient();
			if (ircClient != null) {
				ircClient.AddOnPrivateMessageDelegate(this.HandleTwitchIRCMessage);
			}

			// set up the action factories
			this.ircActionFactory = new Bitzophrenia.TwitchIRCActionFactory(ircClient);
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
		}

		public override void OnApplicationQuit()
		{
			MelonLogger.Msg("Quitting Application");
			try {
				this.twitchController
						.GetIRCClient()
						.Disconnect();
				this.twitchController
						.GetPubSubClient()
						.Disconnect();
			} catch { }
		}

		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			MelonLogger.Msg("OnSceneWasLoaded: [" + buildIndex + "] " + sceneName);
		}

		public override void OnSceneWasInitialized(int buildIndex, string sceneName)
		{
			MelonLogger.Msg("OnSceneWasInitialized: [" + buildIndex + "] " + sceneName);
			this.Phasmophobia.Reset();
			this.Phasmophobia.SetCurrentScene(buildIndex, sceneName);
		}

		/**
		 * Called at the end of each Update call
		 */
		public override void OnUpdate()
		{
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
			MelonLogger.Msg(this.actionQueue.Count + " actions were queued");

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
			MelonLogger.Msg(hashSet.Count + " actions have been executed");
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

	}
}
