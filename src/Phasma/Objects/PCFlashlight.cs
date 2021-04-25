
namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class PCFlashlight
			{

				private global::PCFlashlight instance;

				public PCFlashlight(global::PCFlashlight withInstance)
				{
					this.instance = withInstance;
				}

				public bool IsSwitchedOn()
				{
					return this.instance.field_Public_Boolean_0;
				}

				public void Toggle()
				{
					this.Toggle(!this.IsSwitchedOn(), false);
				}

				public void Toggle(bool toggleOn, bool surpressSound)
				{
					this.instance.EnableOrDisableLight(toggleOn, surpressSound);
					this.instance.EnableOrDisableLightNetworked(toggleOn, surpressSound);
				}
			}
		}
	}
}