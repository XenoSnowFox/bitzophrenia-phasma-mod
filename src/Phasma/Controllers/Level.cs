using System.Collections.Generic;

namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Controllers
		{
			public class Level
			{

				/// <summary>Phasma Level Controller instance.</summary>
				private LevelController controller;

				private Bitzophrenia.Phasma.Objects.Ghost ghost;


				/// <summary>
				/// Instantiates a new instance.
				/// </summary>
				/// <param name="withController">Phasma Level Controller instance.</param>
				public Level(LevelController withController)
				{
					this.controller = withController;
				}

				public Bitzophrenia.Phasma.Objects.Ghost GetGhost()
				{
					if (this.ghost == null && this.controller.field_Public_GhostAI_0 != null)
					{
						this.ghost = new Bitzophrenia.Phasma.Objects.Ghost(this.controller.field_Public_GhostAI_0);
					}
					return this.ghost;
				}

				public List<Bitzophrenia.Phasma.Objects.Torch> GetTorches()
				{
					if (this.controller == null)
					{
						return null;
					}

					List<Bitzophrenia.Phasma.Objects.Torch> list = new List<Bitzophrenia.Phasma.Objects.Torch>();
					foreach (Torch t in this.controller.field_Public_List_1_Torch_0)
					{
						list.Add(new Bitzophrenia.Phasma.Objects.Torch(t));
					}
					return list;
				}

				public Bitzophrenia.Phasma.Controllers.Game GetGameController()
				{
					try
					{
						return this.controller.field_Public_GameController_0 == null ? null : new Bitzophrenia.Phasma.Controllers.Game(this.controller.field_Public_GameController_0);
					}
					catch { }
					return null;
				}

				public Bitzophrenia.Phasma.Controllers.Sound GetSoundController()
				{
					try
					{
						return this.controller.soundController == null ? null : new Bitzophrenia.Phasma.Controllers.Sound(this.controller.soundController);
					}
					catch { }
					return null;
				}
			}
		}
	}
}