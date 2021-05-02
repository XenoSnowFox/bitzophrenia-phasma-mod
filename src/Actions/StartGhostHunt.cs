using MelonLoader;
using System.Collections.Generic;
using System.Threading;

namespace Bitzophrenia
{
	namespace Actions
	{

		public class StartGhostHunt : Bitzophrenia.Actions.AbstractAction
		{

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient;

			private Bitzophrenia.Actions.StopGhostHunt stopHuntAction;

			private Queue<Bitzophrenia.IAction> actionQueue;

			private bool isHunting = false;

			public StartGhostHunt(Bitzophrenia.Phasma.Global phasmophobia, Bitzophrenia.Twitch.TwitchIRCClient withIRCClient, Queue<Bitzophrenia.IAction> withActionQueue)
					: base("Force the ghost to start a hunt.", phasmophobia)
			{
				this.ircClient = withIRCClient;
				this.actionQueue = withActionQueue;
				this.stopHuntAction = new Bitzophrenia.Actions.StopGhostHunt(phasmophobia, withIRCClient);
			}

			public override void Execute()
			{
				MelonLogger.Msg("Execuing StartGhostHunt");

				if (this.isHunting) {
					this.ircClient.SendPrivateMessage("A hunt has already begun!");
					return;
				}

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

				var ghost = level.GetGhost();
				if (ghost == null)
				{
					this.ircClient.SendPrivateMessage("The ghost hasn't spawned within the game yet.");
					return;
				}

				this.isHunting = true;
				ghost.Hunt();
				new Thread(this.CompleteHuntTask).Start();
			}

			private void CompleteHuntTask() {
				Thread.Sleep(UnityEngine.Random.Range(10000, 25000));
				this.actionQueue.Enqueue(this.stopHuntAction);
				this.isHunting = false;
			}
		}
	}
}