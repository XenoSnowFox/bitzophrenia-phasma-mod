
using Photon.Pun;
using UnityEngine;
using Object = Il2CppSystem.Object;
using Boolean = Il2CppSystem.Boolean;
using Int32 = Il2CppSystem.Int32;

namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class Door
			{

				private global::Door instance;

				public Door(global::Door withInstance)
				{
					this.instance = withInstance;
				}

				public void OpenDoor()
				{
					PhotonView photonView = this.instance.field_Public_PhotonView_0;
					this.instance.DisableOrEnableDoor(true);
					this.instance.DisableOrEnableCollider(true);
					this.instance.UnlockDoor();
					photonView.RPC("SyncLockState", RpcTarget.All, getRPCObject(1, false));
				}

				public void CloseDoor()
				{
					PhotonView photonView = this.instance.field_Public_PhotonView_0;
					this.instance.DisableOrEnableDoor(false);
					this.instance.LockDoor();
					this.instance.transform.localRotation = Quaternion.identity;
					Quaternion localRotation = this.instance.transform.localRotation;
					Vector3 eulerAngles = localRotation.eulerAngles;
					eulerAngles.y = this.instance.field_Public_Single_0;
					localRotation.eulerAngles = eulerAngles;
					this.instance.transform.localRotation = localRotation;
					photonView.RPC("SyncLockState", RpcTarget.All, getRPCObject(1, true));
					photonView.RPC("NetworkedPlayLockSound", RpcTarget.All, getRPCObject(0, false));
				}

				public void PlayLockedSound()
				{
					this.instance.PlayLockedSound();
				}

				public static Object[] getRPCObject(int i, bool isTrue = true, int rangeMin = 0, int rangeMax = 0, bool rangeFirst = false, bool isPosition = false, Vector3 pos = new Vector3())
				{
					Object[] obj = new Object[i];
					if (i > 0)
					{
						Boolean boolean = default(Boolean);
						if (!rangeFirst)
						{
							if (isTrue)
								boolean.m_value = true;
							else
								boolean.m_value = false;
							obj[0] = boolean.BoxIl2CppObject();

							if (i == 2)
							{
								Int32 integer = default(Int32);
								integer.m_value = Random.Range(rangeMin, rangeMax);
								obj[1] = integer.BoxIl2CppObject();
							}
						}
						else
						{
							Int32 integer = default(Int32);
							integer.m_value = Random.Range(rangeMin, rangeMax);
							obj[0] = integer.BoxIl2CppObject();
						}
					}
					if (isPosition)
					{
						Vector3 vector = default(Vector3);
						vector = pos;
						obj[0] = vector.BoxIl2CppObject();
					}

					return obj;
				}
			}
		}
	}
}