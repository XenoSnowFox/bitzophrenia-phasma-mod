namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class Torch
			{

				private global::Torch instance;

				public Torch(global::Torch withInstance)
				{
					this.instance = withInstance;
				}

				public void Toggle()
				{
					if (this.instance != null)
					{
						this.instance.Use();
					}
				}
			}
		}
	}
}