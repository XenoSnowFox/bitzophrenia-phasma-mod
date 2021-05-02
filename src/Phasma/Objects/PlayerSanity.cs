using System;
using UnityEngine;

namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class PlayerSanity
			{

				private global::PlayerSanity instance;

				public PlayerSanity(global::PlayerSanity withInstance)
				{
					this.instance = withInstance;
				}

				public double GetSanityLevel()
				{
					if (this.instance.field_Public_Single_0 <= -1)
					{
						return -1;
					}
					return Math.Round(100 - this.instance.field_Public_Single_0, 0);
				}

				public void SetSanityLevel(int withAmount) {
					this.Log();
					var amount = withAmount;
					if (withAmount < 0) {
						amount = 0;
					}
					if (withAmount > 100) {
						amount = 100;
					}
					this.instance.field_Public_Single_0 = (float)(100 - amount);
					// this.instance.NetworkedUpdatePlayerSanity((float)(100 - amount));
					this.instance.UpdatePlayerSanity();
					// this.instance.Update();
					this.Log();
				}

				public override string ToString()
				{
					var d = this.GetSanityLevel();
					if (d == -1)
					{
						return "??";
					}

					return d.ToString() + "%";
				}

				public global::PlayerSanity GetInstance() {
					return this.instance;
				}

				public void Log() {
					var str = "SANITY [" + this.ToString() + "}";
					str += "\n\t field_Public_Single_0: " + this.instance.field_Public_Single_0;
					str += "\n\t field_Private_Single_0: " + this.instance.field_Private_Single_0;
					str += "\n\t field_Private_Boolean_0: " + this.instance.field_Private_Boolean_0;
					str += "\n\t field_Private_Single_1: " + this.instance.field_Private_Single_1;
					str += "\n\t field_Private_Single_3: " + this.instance.field_Private_Single_3;
					str += "\n\t field_Private_Single_4: " + this.instance.field_Private_Single_4;
					str += "\n\t field_Private_Boolean_1: " + this.instance.field_Private_Boolean_1;
					str += "\n\t field_Private_Single_2: " + this.instance.field_Private_Single_2;
					str += "\n\t field_Public_Boolean_0: " + this.instance.field_Public_Boolean_0;

					MelonLoader.MelonLogger.Msg(str);
				}
			}
		}
	}
}
