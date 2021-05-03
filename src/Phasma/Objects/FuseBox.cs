namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class FuseBox
			{

				private global::FuseBox instance;

				public FuseBox(global::FuseBox withInstance)
				{
					this.instance = withInstance;
				}

				public void Toggle()
				{
					this.instance.Use();
				}

				public void TurnOff() {
					this.instance.TurnOff(true);
				}
			}
		}
	}
}