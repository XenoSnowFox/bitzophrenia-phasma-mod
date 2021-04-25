namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class Ghost
			{

				private global::GhostAI ghostAI;

				public Ghost(global::GhostAI withGhostAI)
				{
					this.ghostAI = withGhostAI;
				}

				public string GetName()
				{
					try {
						if (this.ghostAI == null)
						{
							return null;
						}

						return this.ghostAI
								.field_Public_GhostInfo_0 // Get ghost info
								.field_Public_ValueTypePublicSealedObInBoStInBoInInInUnique_0 // get ghost meta-data
								.field_Public_String_0; // get ghost name
					} catch {}
					return null;
				}

				public int GetAge()
				{
					try {
						if (this.ghostAI == null)
						{
							return -1;
						}

						return this.ghostAI
								.field_Public_GhostInfo_0 // Get ghost info
								.field_Public_ValueTypePublicSealedObInBoStInBoInInInUnique_0 // get ghost meta-data
								.field_Public_Int32_0; // get ghost age
					} catch {}
					return -1;
				}

				public void PlayRandomSound()
				{
					try
					{
						var ghostAudio = this.ghostAI
								.field_Public_GhostAudio_0;
						if (ghostAudio == null)
						{
							return;
						}
						ghostAudio.PlaySoundNetworked(1, false);
						ghostAudio.PlaySound(1, false);
					}
					catch { }
				}
			}
		}
	}
}