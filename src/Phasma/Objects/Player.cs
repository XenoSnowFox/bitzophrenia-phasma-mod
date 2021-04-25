namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{
			public class Player
			{

				private global::Player instance;

				private Bitzophrenia.Phasma.Objects.PlayerSanity sanityObject;

				public Player(global::Player withInstance)
				{
					this.instance = withInstance;
				}

				public string getNickName()
				{
					try
					{
						return this.instance.field_Public_PhotonView_0.owner.NickName;
					}
					catch { }
					return null;
				}

				public void Kill()
				{
					this.instance.StartKillingPlayerNetworked();
					this.instance.StartKillingPlayer();
				}

				public void ForceDropProbs()
				{
					this.instance.field_Public_PCPropGrab_0.DropAllInventoryProps();
				}

				public Bitzophrenia.Phasma.Objects.PCFlashlight GetPCFlashlight()
				{
					return this.instance.field_Public_PCFlashlight_0 == null ? null : new Bitzophrenia.Phasma.Objects.PCFlashlight(this.instance.field_Public_PCFlashlight_0);
				}

				public Bitzophrenia.Phasma.Objects.PlayerSanity GetSanityObject()
				{
					if (this.sanityObject == null)
					{
						var tmp = this.instance.field_Public_PlayerSanity_0;
						this.sanityObject = tmp == null ? null : new Bitzophrenia.Phasma.Objects.PlayerSanity(tmp);
					}
					return this.sanityObject;
				}
			}
		}
	}
}