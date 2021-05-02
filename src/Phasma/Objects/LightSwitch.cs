namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class LightSwitch
			{

				private global::LightSwitch instance;

				public LightSwitch(global::LightSwitch withInstance)
				{
					this.instance = withInstance;
				}

				public bool IsSwitchedOn() {
					return this.instance.field_Public_Boolean_0;
				}

				public void Use(bool switchOn)
				{
					this.instance.Use(switchOn);
				}

				public void Toggle()
				{
					this.instance.UseLight();
				}

				public void StartBlinking()
				{
					this.instance.StartBlinking();
				}

				public void StopBlinking()
				{
					this.instance.StopBlinking();
				}
			}
		}
	}
}