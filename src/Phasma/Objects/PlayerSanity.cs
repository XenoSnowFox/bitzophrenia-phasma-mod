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

				public double getSanityLevel()
				{
					if (this.instance.field_Public_Single_0 <= -1)
					{
						return -1;
					}
					return Math.Round(100 - this.instance.field_Public_Single_0, 0);
				}

				public override string ToString()
				{
					var d = this.getSanityLevel();
					if (d == -1)
					{
						return "??";
					}

					return d.ToString();
				}
			}
		}
	}
}
