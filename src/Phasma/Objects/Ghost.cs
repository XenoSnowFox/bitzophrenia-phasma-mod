namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class Ghost
			{

				private GhostAI ghostAI;

				public Ghost(GhostAI withGhostAI)
				{
					this.ghostAI = withGhostAI;
				}

				public string GetName()
				{
					try {
						if (this.ghostAI == null)
						{
							return null;
						}

						return this.ghostAI
								.field_Public_GhostInfo_0 // Get ghost info
								.field_Public_ValueTypePublicSealedObInBoStInBoInInInUnique_0 // get ghost meta-data
								.field_Public_String_0; // get ghost name
					} catch {}
					return null;
				}

				public int GetAge()
				{
					try {
						if (this.ghostAI == null)
						{
							return -1;
						}

						return this.ghostAI
								.field_Public_GhostInfo_0 // Get ghost info
								.field_Public_ValueTypePublicSealedObInBoStInBoInInInUnique_0 // get ghost meta-data
								.field_Public_Int32_0; // get ghost age
					} catch {}
					return -1;
				}

				public void PlayRandomSound()
				{
					try
					{
						var ghostAudio = this.ghostAI
								.field_Public_GhostAudio_0;
						if (ghostAudio == null)
						{
							return;
						}
						ghostAudio.PlaySoundNetworked(1, false);
						ghostAudio.PlaySound(1, false);
					}
					catch { }
				}

				public void Appear() {
					try {
						Photon.Pun.PhotonView photonView = this.ghostAI.field_Public_PhotonView_0;
            			photonView.RPC("MakeGhostAppear", Photon.Pun.RpcTarget.All, Bitzophrenia.Phasma.RPC.GetObject(2, true, 1, 1));
						this.ghostAI.Appear(true);
						this.ghostAI.MakeGhostAppear(true, 1);
					}
					catch { }
				}

				public void Disappear() {
					try {
						Photon.Pun.PhotonView photonView = this.ghostAI.field_Public_PhotonView_0;
            			photonView.RPC("MakeGhostAppear", Photon.Pun.RpcTarget.All, Bitzophrenia.Phasma.RPC.GetObject(2, false, 0, 1));
						this.ghostAI.UnAppear(true);
						this.ghostAI.MakeGhostAppear(false, 1);
					}
					catch { }
				}

				public void Hunt() {
					try {
						this.ghostAI.field_Public_Boolean_0 = true;
						this.ghostAI.field_Public_Boolean_1 = true;
						this.ghostAI.field_Public_Boolean_2 = true;
						this.ghostAI.field_Public_Boolean_3 = true;
						this.ghostAI.field_Public_Boolean_4 = true;
						this.ghostAI.field_Public_Boolean_5 = true;
						this.ghostAI.field_Public_Player_0 = null;
						// this.ghostAI.ChasingPlayer(true); Method_Public_Void_Boolean_0 Method_Public_Void_Boolean_1
						this.ghostAI.field_Public_Animator_0.SetBool("isIdle", false);
						this.ghostAI.field_Public_Animator_0.SetInteger("WalkType", 1);
						this.ghostAI.field_Public_GhostAudio_0.TurnOnOrOffAppearSource(true);
						this.ghostAI.field_Public_GhostAudio_0.PlayOrStopAppearSource(true);
						this.ghostAI.field_Public_NavMeshAgent_0.speed = this.ghostAI.field_Public_Single_0;
						this.ghostAI.field_Public_NavMeshAgent_0.SetDestination(getDestination());
						this.ghostAI.ChangeState(GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.hunting, null, null);
						this.ghostAI.field_Public_GhostInteraction_0.CreateAppearedEMF(this.ghostAI.transform.position);
						this.ghostAI.Appear(true);

						Photon.Pun.PhotonView photonView = this.ghostAI.field_Public_PhotonView_0;
						photonView.RPC("Hunting", Photon.Pun.RpcTarget.All, Bitzophrenia.Phasma.RPC.GetObject(1, true));
						photonView.RPC("SyncChasingPlayer", Photon.Pun.RpcTarget.All, Bitzophrenia.Phasma.RPC.GetObject(1, true));
					}
					catch { }
				}

				public void EndHunt() {
					try {
						this.ghostAI.field_Public_Player_0 = null;
						this.ghostAI.field_Public_Animator_0.SetInteger("IdleNumber", UnityEngine.Random.Range(0, 2));
						this.ghostAI.field_Public_Animator_0.SetBool("isIdle", true);
						this.ghostAI.field_Public_GhostAudio_0.TurnOnOrOffAppearSource(false);
						this.ghostAI.field_Public_GhostAudio_0.PlayOrStopAppearSource(false);
						//this.ghostAI.ChasingPlayer(false);
						this.ghostAI.StopGhostFromHunting();
						this.ghostAI.UnAppear(false);
						this.ghostAI.ChangeState(GhostAI.EnumNPublicSealedvaidwahufalidothfuapUnique.idle, null, null);

						Photon.Pun.PhotonView photonView = this.ghostAI.field_Public_PhotonView_0;
						photonView.RPC("Hunting", Photon.Pun.RpcTarget.All, Bitzophrenia.Phasma.RPC.GetObject(1, false));
						photonView.RPC("SyncChasingPlayer", Photon.Pun.RpcTarget.All, Bitzophrenia.Phasma.RPC.GetObject(1, false));
					}
					catch { }
				}

				private UnityEngine.Vector3 getDestination()
				{
					UnityEngine.Vector3 destination = UnityEngine.Vector3.zero;
					float num = UnityEngine.Random.Range(2f, 15f);
					UnityEngine.AI.NavMeshHit navMeshHit;
					if (UnityEngine.AI.NavMesh.SamplePosition(UnityEngine.Random.insideUnitSphere * num + this.ghostAI.transform.position, out navMeshHit, num, 1))
					{
						destination = navMeshHit.position;
					}
					else
					{
						destination = UnityEngine.Vector3.zero;
					}

					return destination;
				}
			}
		}
	}
}