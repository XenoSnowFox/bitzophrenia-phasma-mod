
using MelonLoader;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bitzophrenia {
	namespace Twitch {

		/// <summary> Callback method for channel point redemptions.</summary>
		public delegate void OnChannelPointRedemption(string withNickName, string withGuid);

		/// <summary></summary>
		public class TwitchPubSubClient {
			/// <summary></summary>
			private const string IRC_WEBSOCKET_URI = "wss://pubsub-edge.twitch.tv";

			/// <summary></summary>
			private const int SEND_CHUNK_SIZE = 2048;

			/// <summary></summary>
			private const int RECEIVE_CHUNK_SIZE = 2048;

			/// <summary></summary>
			private static UTF8Encoding UTF8_ENCODING = new UTF8Encoding();

			/// <summary></summary>
			private ClientWebSocket webSocket = null;

			private string authenticationToken = null;

			/// <summary>Collection of callbacks to be invoked on a successful authentication</summary>
			private List<OnChannelPointRedemption> onChannelPointRedemptionDelegates = new List<OnChannelPointRedemption>();

			/// <summary></summary>
			public TwitchPubSubClient() {
				new Task(this.RunWebSocketListener).Start();
			}

			/// <summary>Starts a new connection via WbSocket to Twitch's PubSub server.</summary>
			public void Connect()
			{
				TwitchPubSubClient.Log("Connect()");
				if (this.webSocket != null)
				{
					return;
				}

				this.webSocket = new ClientWebSocket();
				this.webSocket.ConnectAsync(new Uri(TwitchPubSubClient.IRC_WEBSOCKET_URI), CancellationToken.None).Wait();
			}

			public void Disconnect()
			{
				if (this.webSocket != null)
				{
					return;
				}

				this.webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Quitting", CancellationToken.None).Wait();
				this.webSocket = null;
			}

			/// <summary>Defines the authentication command</summary>
			public void Authenticate(string withAuthenticationToken)
			{
				this.authenticationToken = withAuthenticationToken;
			}

			/// <summary>Pong command for when twitch issues a PING</summary>
			public void Ping()
			{
				TwitchPubSubClient.Log("Ping()");
				if (this.webSocket == null)
				{
					return;
				}

				byte[] buffer = TwitchPubSubClient.UTF8_ENCODING.GetBytes("{\"type\":\"PING\"}");
				this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None)
						.Wait();
				TwitchPubSubClient.Log("[->] " + TwitchPubSubClient.UTF8_ENCODING.GetString(buffer));
			}

			/// <summary>Listen command</summary>
			public void Listen()
			{
				TwitchPubSubClient.Log("Listen()");
				if (this.webSocket == null)
				{
					return;
				}

				byte[] buffer = TwitchPubSubClient.UTF8_ENCODING.GetBytes("{\"type\":\"LISTEN\", \"noonce\":\"16465456623\", \"data\": {  \"topics\":[\"channel-bits-events-v2.125483513\", \"channel-points-channel-v1.125483513\"], \"auth_token\":\"" + this.authenticationToken + "\" }}");
				this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None)
						.Wait();
				TwitchPubSubClient.Log("[->] " + TwitchPubSubClient.UTF8_ENCODING.GetString(buffer));
			}

			/// <summary>Adds a delegate to be invoked when chennel points have been redeemed.</summary>
			public void AddOnChannelPointRedemptionDelegate(OnChannelPointRedemption withDelegate)
			{
				this.onChannelPointRedemptionDelegates.Add(withDelegate);
			}

			/// <summary>Invokes any linked delegates when channel points are redeemed</summary>
			private void OnChannelPointRedemption(Bitzophrenia.Twitch.TwitchChannelPointsMessage withMessage)
			{
				foreach (var d in this.onChannelPointRedemptionDelegates)
				{
					try
					{
						d(withMessage.data.redemption.user.login, withMessage.data.redemption.reward.id);
					}
					catch { }
				}
			}

			/// <summary>Websicket listerner</summary>
			private async void RunWebSocketListener()
			{
				TwitchPubSubClient.Log("Running PubSub WebSocket Listener");

				HashSet<string> caughtExceptions = new HashSet<string>();

				while (true)
				{
					try
					{
						if (this.webSocket == null)
						{
							continue;
						}

						if (this.webSocket.State == WebSocketState.Closed)
						{
							TwitchPubSubClient.Log("WebSocket has been closed.");
							break;
						}

						if (this.webSocket.State != WebSocketState.Open)
						{
							continue;
						}

						byte[] buffer = new byte[TwitchPubSubClient.RECEIVE_CHUNK_SIZE];

						var result = await this.webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
						if (result.MessageType == WebSocketMessageType.Close)
						{
							await this.webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
							continue;
						}

						// split the incomming messages by line
						string input = TwitchPubSubClient.UTF8_ENCODING.GetString(buffer);
						TwitchPubSubClient.Log("[<-] " + input);

						Bitzophrenia.Twitch.TwitchMessage msg = JsonConvert.DeserializeObject<Bitzophrenia.Twitch.TwitchMessage>(input);

						// attempt channel point redemption
						var channelPointRedemption = msg.ToChannelPointsMessage();
						if (channelPointRedemption != null) {
							this.OnChannelPointRedemption(channelPointRedemption);
						}
					}
					catch (Exception ex)
					{
						if (caughtExceptions.Add(ex.ToString())) {
							TwitchPubSubClient.Log("EXCEPTION CAUGHT");
							TwitchPubSubClient.Log(ex.GetType().ToString());
							TwitchPubSubClient.Log(ex.ToString());
						}
					}
				}

				TwitchPubSubClient.Log("Stopping PUBSUB WebSocket Listener");
			}

			private static void Log(string withMessage) {
				MelonLogger.Msg("[PUBSUB Client] " + withMessage);
			}
		}
	}
}