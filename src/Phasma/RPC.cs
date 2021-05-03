using MelonLoader;
using System.Collections.Generic;
using UnityEngine;
using Object = Il2CppSystem.Object;
using Boolean = Il2CppSystem.Boolean;
using Int32 = Il2CppSystem.Int32;

namespace Bitzophrenia {
	namespace Phasma {

		public class RPC {

			public static RPCMethodAppender UsingPhotonView(Photon.Pun.PhotonView photonView) {
				var executor = new RPC();
				executor.photonView = photonView;
				return new RPCMethodAppender(executor);
			}

			private RPC() { }

			public Photon.Pun.PhotonView photonView { get; set; }

			public string methodName { get; set; }

			public Photon.Pun.RpcTarget rpcTarget { get; set; }

			public Object[] parameters { get; set; }

			public void Execute() {
				this.photonView.RPC(this.methodName, this.rpcTarget, this.parameters);
			}
		}

		public class RPCMethodAppender {

			private RPC rpcExecutor;

			public RPCMethodAppender(RPC withExecutor) {
				this.rpcExecutor = withExecutor;
			}

			public RPCTargetAppender ExecuteMethod(string withMethodName) {
				this.rpcExecutor.methodName = withMethodName;
				return new RPCTargetAppender(this.rpcExecutor);
			}
		}

		public class RPCTargetAppender {

			private RPC rpcExecutor;

			private List<Object> objectList = new List<Object>();

			public RPCTargetAppender(RPC withExecutor) {
				this.rpcExecutor = withExecutor;
			}

			public RPCTargetAppender WithParameter(bool withBoolean) {
				Boolean boolean = default(Boolean);
				boolean.m_value = withBoolean;
				objectList.Add(boolean.BoxIl2CppObject());
				return this;
			}

			public RPCTargetAppender WithParameter(int withInteger) {
				Int32 integer = default(Int32);
				integer.m_value = Random.Range(withInteger, withInteger);
				objectList.Add(integer.BoxIl2CppObject());
				return this;
			}

			public RPCTargetAppender WithParameter(Vector3 withVector3) {
				Vector3 vector = default(Vector3);
				vector = withVector3;
				objectList.Add(vector.BoxIl2CppObject());
				return this;
			}

			public void OnAllTargets() {
				this.OnTarget(Photon.Pun.RpcTarget.All);
			}

			public void OnOtherTargets() {
				this.OnTarget(Photon.Pun.RpcTarget.Others);
			}

			public void OnTarget(Photon.Pun.RpcTarget withRPCTarget) {
				this.rpcExecutor.rpcTarget = withRPCTarget;
				this.rpcExecutor.parameters = objectList.ToArray();
				this.rpcExecutor.Execute();
			}
		}
	}
}