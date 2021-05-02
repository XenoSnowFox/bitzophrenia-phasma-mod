using System.Collections.Generic;

namespace Bitzophrenia {
	namespace Phasma {

		public class Global {

			private int SceneID = -1;

			private string SceneName = null;

			private bool _hasMissionStarted = false;

			private List<Bitzophrenia.IAction> onMissionStartActions = new List<Bitzophrenia.IAction>();

			private Bitzophrenia.Phasma.Controllers.Game GameController;

			private Bitzophrenia.Phasma.Controllers.Level LevelController;

			private Bitzophrenia.Phasma.Controllers.Mission MissionController;

			private Bitzophrenia.Phasma.Controllers.SpeechRecognition SpeechRecognitionController;

			public void Reset() {
				this.GameController = null;
				this.LevelController = null;
				this.MissionController = null;
				this.SpeechRecognitionController = null;

				this._hasMissionStarted = false;
			}

			public void SetCurrentScene(int sceneID, string sceneName) {
				this.SceneID = sceneID;
				this.SceneName = sceneName;
			}

			public int GetCurrentSceneID() {
				return this.SceneID;
			}

			public string GetCurrentSceneName() {
				return this.SceneName;
			}

			public bool IsInLobby() {
				return this.SceneID <= 1;
			}

			public bool IsInLevel() {
				return this.SceneID > 1;
			}

			public bool HasMissionStarted() {
				return this._hasMissionStarted;
			}

			public void AddOnMissionStartAction(Bitzophrenia.IAction action) {
				this.onMissionStartActions.Add(action);
			}

			public void Update() {
				if (this.IsInLevel() && !this.HasMissionStarted()) {
					var levelController = this.GetLevelController();
					if (levelController != null && levelController.GetGhost() != null) {
						this._hasMissionStarted = true;
						MelonLoader.MelonLogger.Msg("_hasMissionStarted: " + this._hasMissionStarted);
						foreach(var action in this.onMissionStartActions) {
							try {
								action.Execute();
							} catch {}
						}
					}
				}
			}

			public Bitzophrenia.Phasma.Controllers.Game GetGameController() {
				if (this.GameController == null) {
					global::GameController controller = global::GameController.field_Public_Static_GameController_0;
					this.GameController = controller == null ? null : new Bitzophrenia.Phasma.Controllers.Game(controller);
				}
				return this.GameController;
			}

			public Bitzophrenia.Phasma.Controllers.Level GetLevelController() {
				if (this.LevelController == null) {
					global::LevelController controller = global::LevelController.field_Public_Static_LevelController_0;
					this.LevelController = controller == null ? null : new Bitzophrenia.Phasma.Controllers.Level(controller);
				}
				return this.LevelController;
			}

			public Bitzophrenia.Phasma.Controllers.Mission GetMissionController() {
				if (this.MissionController == null) {
					global::MissionManager controller = global::MissionManager.field_Public_Static_MissionManager_0;
					this.MissionController = controller == null ? null : new Bitzophrenia.Phasma.Controllers.Mission(controller);
				}
				return this.MissionController;
			}

			public Bitzophrenia.Phasma.Controllers.SpeechRecognition GetSpeechRecognitionController() {
				if (this.SpeechRecognitionController == null) {
					global::SpeechRecognitionController controller = global::SpeechRecognitionController.field_Public_Static_SpeechRecognitionController_0;
					this.SpeechRecognitionController = controller == null ? null : new Bitzophrenia.Phasma.Controllers.SpeechRecognition(controller);
				}
				return this.SpeechRecognitionController;
			}
		}
	}
}