
using System.Collections.Generic;

namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Controllers
		{
			public class Game
			{

				/// <summary>Phasma Level Controller instance.</summary>
				private GameController controller;

				private Bitzophrenia.Phasma.Controllers.Level levelController = null;

				private Bitzophrenia.Phasma.Controllers.MultiPlayer multiPlayerController = null;

				/// <summary>
				/// Instantiates a new instance.
				/// </summary>
				/// <param name="withController">Phasma Game Controller instance.</param>
				public Game(GameController withController)
				{
					this.controller = withController;
				}


				public bool HasLevelController()
				{
					return this.GetLevelController() != null;
				}

				public Bitzophrenia.Phasma.Controllers.Level GetLevelController()
				{
					if (this.levelController == null)
					{
						LevelController tmp = this.controller.levelController;
						this.levelController = tmp == null ? null : new Bitzophrenia.Phasma.Controllers.Level(tmp);
					}

					return this.levelController;
				}

				public Bitzophrenia.Phasma.Objects.Player GetPlayer() {
					var player = this.controller.field_Public_ObjectPublicPlStPlUnique_0;
					if (player == null || player.field_Public_Player_0 == null) {
						return null;
					}
					return new Bitzophrenia.Phasma.Objects.Player(player.field_Public_Player_0);
				}

				public List<Bitzophrenia.Phasma.Objects.Player> ListPlayers()
				{
					var playerList = new List<Bitzophrenia.Phasma.Objects.Player>();
					try
					{
						var list = this.controller.field_Public_List_1_ObjectPublicPlStPlUnique_0;
						foreach (var item in list)
						{
							var tmp = item.field_Public_Player_0;
							if (tmp != null)
							{
								playerList.Add(new Bitzophrenia.Phasma.Objects.Player(tmp));
							}
						}
					}
					catch { }
					return playerList;
				}

				public bool HasMultiPlayerController()
				{
					return this.GetMultiPlayerController() != null;
				}

				public Bitzophrenia.Phasma.Controllers.MultiPlayer GetMultiPlayerController()
				{
					if (this.multiPlayerController == null)
					{
						MultiplayerController tmp = this.controller.multiplayerController;
						this.multiPlayerController = tmp == null ? null : new Bitzophrenia.Phasma.Controllers.MultiPlayer(tmp);
					}

					return this.multiPlayerController;
				}

			}
		}
	}
}