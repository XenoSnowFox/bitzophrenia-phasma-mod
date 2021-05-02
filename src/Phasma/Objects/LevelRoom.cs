
using System.Collections.Generic;

namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class LevelRoom
			{

				private global::LevelRoom instance;

				public LevelRoom(global::LevelRoom withInstance)
				{
					this.instance = withInstance;
				}

				public global::LevelRoom GetInstance() {
					return this.instance;
				}

				/// <summary>Returns whether the room lights are switched on or not.</summary>
				public bool AreLightsOn() {
					return this.instance.AreLightsOn();
				}

				public List<Bitzophrenia.Phasma.Objects.LightSwitch> ListLightScwitches() {
					var list = new List<Bitzophrenia.Phasma.Objects.LightSwitch>();
					foreach(var lightSwitch in this.instance.field_Public_List_1_LightSwitch_0) {
						list.Add(new Bitzophrenia.Phasma.Objects.LightSwitch(lightSwitch));
					}
					return list;
				}
			}
		}
	}
}