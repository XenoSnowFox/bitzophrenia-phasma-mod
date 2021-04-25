
namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Controllers
		{
			public class Sound
			{

				/// <summary>Phasma Sound Controller instance.</summary>
				private SoundController controller;

				/// <summary>
				/// Instantiates a new instance.
				/// </summary>
				/// <param name="withController">Phasma Game Controller instance.</param>
				public Sound(SoundController withController)
				{
					this.controller = withController;
				}

				public void PlayDoorKnockingSound()
				{
					try
					{
						this.controller.view.RPC("PlayDoorKnockingSound", Photon.Pun.RpcTarget.All, Bitzophrenia.Phasma.RPC.GetObject(0, false));
					}
					catch { }
				}
			}
		}
	}
}