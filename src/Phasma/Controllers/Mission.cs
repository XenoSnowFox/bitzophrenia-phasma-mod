
namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Controllers
		{
			public class Mission
			{

				/// <summary>
				/// Instantiates a new instance.
				/// </summary>
				/// <param name="withMission">Phasma Game Controller instance.</param>
				public Mission(MissionManager withMission)
				{
					this.mission = withMission;
				}


				/// <summary>Phasma Level Controller instance.</summary>
				public MissionManager mission;

				public bool HasMissionsLoaded()
				{
					var missionList = this.mission.field_Public_List_1_Mission_0;
					return missionList != null && missionList.Count > 1;
				}

				public string[] GetMissions()
				{
					var missionList = this.mission.field_Public_List_1_Mission_0;

					string[] arr = new string[missionList.Count];
					int i = 0;
					foreach (var mission in missionList)
					{
						string str = mission.field_Public_String_0;
						arr[i] = str;
						i++;
					}
					return arr;
				}
			}
		}
	}
}