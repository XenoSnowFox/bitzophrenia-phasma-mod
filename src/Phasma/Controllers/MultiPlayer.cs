
using System.Collections.Generic;

namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Controllers
		{
			public class MultiPlayer
			{

				/// <summary>Phasma Level Controller instance.</summary>
				private MultiplayerController instance;

				private Bitzophrenia.Phasma.Objects.Player player;

				/// <summary>
				/// Instantiates a new instance.
				/// </summary>
				/// <param name="withInstance">Phasma Game Controller instance.</param>
				public MultiPlayer(MultiplayerController withInstance)
				{
					this.instance = withInstance;
				}


				// field_Private_Player_0

				public Bitzophrenia.Phasma.Objects.Player GetPlayerObject()
				{
					if (this.player == null)
					{
						Player tmp = this.instance.field_Private_Player_0;
						this.player = tmp == null ? null : new Bitzophrenia.Phasma.Objects.Player(tmp);
					}
					return this.player;
				}
			}
		}
	}
}