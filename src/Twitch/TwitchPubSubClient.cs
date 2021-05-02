
using MelonLoader;

using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bitzophrenia {
	namespace Twitch {

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

			/// <summary></summary>
			public TwitchPubSubClient() {
				new Task(this.RunWebSocketListener).Start();
			}

			/// <summary>Starts a new connection via WbSocket to Twitch's PubSub server.</summary>
			public void Connect()
			{
				MelonLogger.Msg("[PUBSUB Cliect] Connect()");
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
				MelonLogger.Msg("[PUBSUB Cliect] Ping()");
				if (this.webSocket == null)
				{
					return;
				}

				byte[] buffer = TwitchPubSubClient.UTF8_ENCODING.GetBytes("{\"type\":\"PING\"}");
				this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None)
						.Wait();
				MelonLogger.Msg("[PUBSUB ->] " + TwitchPubSubClient.UTF8_ENCODING.GetString(buffer));
			}

			/// <summary>Listen command</summary>
			public void Listen()
			{
				MelonLogger.Msg("[PUBSUB Cliect] Listen()");
				if (this.webSocket == null)
				{
					return;
				}

				byte[] buffer = TwitchPubSubClient.UTF8_ENCODING.GetBytes("{\"type\":\"LISTEN\", \"noonce\":\"16465456623\", \"data\": {  \"topics\":[\"channel-bits-events-v2.125483513\", \"channel-points-channel-v1.125483513\"], \"auth_token\":\"" + this.authenticationToken + "\" }}");
				this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None)
						.Wait();
				MelonLogger.Msg("[PUBSUB ->] " + TwitchPubSubClient.UTF8_ENCODING.GetString(buffer));
			}

			/// <summary>Websicket listerner</summary>
			private async void RunWebSocketListener()
			{
				MelonLogger.Msg("Running PubSub WebSocket Listener");

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
							MelonLogger.Msg("WebSocket has been closed.");
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
						MelonLogger.Msg("[PUBSUB <-] " + input);

						// string[] lines = TwitchPubSubClient.UTF8_ENCODING.GetString(buffer).Split('\n');
						// foreach (string line in lines)
						// {
							// if (line.Trim().Length == 0)
							// {
							// 	continue;
							// }

							// string[] subparts = line.Split(new char[] { ':' }, 3);

							// ping command
							// if (subparts.Length == 2)
							// {
							// 	if (subparts[0].Trim() == "PING")
							// 	{
							// 		this.Pong();
							// 	}
							// 	continue;
							// }

							// incoming private message
							// if (subparts.Length == 3)
							// {
							// 	string[] metaParts = subparts[1].Split(' ');
							// 	if (metaParts.Length >= 3 && metaParts[1] == "PRIVMSG")
							// 	{
							// 		this.OnPrivateMessage(metaParts[2].Substring(1), subparts[2]);
							// 	}
							// 	continue;
							// }

							// unknown command
						// 	MelonLogger.Msg("UNKNOWN COMMAND: " + line);
						// 	continue;
						// }
					}
					catch (Exception ex)
					{
						if (caughtExceptions.Add(ex.ToString())) {
							MelonLogger.Msg("[PUBSUB Client] EXCEPTION CAUGHT");
							MelonLogger.Msg("[PUBSUB Client] " + ex.GetType().ToString());
							MelonLogger.Msg("[PUBSUB Client] " + ex.ToString());
						}
					}

					// Thread.Sleep(100);
				}

				MelonLogger.Msg("Stopping PUBSUB WebSocket Listener");
			}
		}
	}
}