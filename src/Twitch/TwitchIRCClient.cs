
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bitzophrenia {
	namespace Twitch {

		/// <summary> Callback method for private messages </summary>
		public delegate void OnPrivateMessage(string withNickName, string withMessage);

		/// <summary>IRC Client for Twitch.</summary>
		public class TwitchIRCClient {
			/// <summary></summary>
			private const string IRC_WEBSOCKET_URI = "wss://irc-ws.chat.twitch.tv:443";

			/// <summary></summary>
			private const int SEND_CHUNK_SIZE = 256;

			/// <summary></summary>
			private const int RECEIVE_CHUNK_SIZE = 256;

			/// <summary></summary>
			private static UTF8Encoding UTF8_ENCODING = new UTF8Encoding();

			/// <summary></summary>
			private string nickName;

			/// <summary></summary>
			private string channelName;

			/// <summary></summary>
			private ClientWebSocket webSocket = null;

			/// <summary>Collection of callbacks to be invoked on a successful authentication</summary>
			private List<OnPrivateMessage> onPrivateMessageDelegates = new List<OnPrivateMessage>();

			/// <summary>Instantiates a new instance.</summary>
			public TwitchIRCClient() {
				new Task(this.RunWebSocketListener).Start();
			}

			/// <summary>Starts a new connection via WbSocket to Twitch's IRC server.</summary>
			public void Connect() {
				MelonLogger.Msg("[IRL Cliect] Connect()");
				if (this.webSocket != null) {
					return;
				}

				this.webSocket = new ClientWebSocket();
				this.webSocket.ConnectAsync(new Uri(TwitchIRCClient.IRC_WEBSOCKET_URI), CancellationToken.None).Wait();
			}

			/// <summary>Send the authentication command</summary>
			public void Authenticate(string withAuthenticationToken) {
				MelonLogger.Msg("[IRL Cliect] Authenticate()");
				if (this.webSocket == null) {
					return;
				}

				byte[] buffer = TwitchIRCClient.UTF8_ENCODING.GetBytes("PASS oauth:" + withAuthenticationToken);
				this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None)
						.Wait();
				MelonLogger.Msg("[IRC ->] " + TwitchIRCClient.UTF8_ENCODING.GetString(buffer));
			}

			/// <summary>Set my nickname</summary>
			public void SetNickName(string withNickName) {
				MelonLogger.Msg("[IRL Cliect] SetNickName()");
				if (this.webSocket == null) {
					return;
				}

				byte[] buffer = TwitchIRCClient.UTF8_ENCODING.GetBytes("NICK " + withNickName);
				this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None)
						.Wait();
				MelonLogger.Msg("[IRC ->] " + TwitchIRCClient.UTF8_ENCODING.GetString(buffer));

				this.nickName = withNickName;
			}

			/// <summary>Join a channel</summary>
			public void JoinChannel(string withChannelName) {
				MelonLogger.Msg("[IRL Cliect] JoinChannel()");
				if (this.webSocket == null) {
					return;
				}

				byte[] buffer = TwitchIRCClient.UTF8_ENCODING.GetBytes("JOIN #" + withChannelName.ToLower());
				this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None)
						.Wait();
				MelonLogger.Msg("[IRC ->] " + TwitchIRCClient.UTF8_ENCODING.GetString(buffer));

				this.channelName = withChannelName;
			}

			/// <summary>Sends a message via the IRC</summary>
			public void SendPrivateMessage(string withMessage) {
				MelonLogger.Msg("[IRL Cliect] SendPrivateMessage()");
				if (this.webSocket == null) {
					return;
				}

				byte[] buffer = TwitchIRCClient.UTF8_ENCODING.GetBytes(":" + this.nickName + "!" + this.nickName + "@" + this.nickName + ".tmi.twitch.tv PRIVMSG #" + this.channelName.ToLower() + " :" + withMessage);
				this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None)
						.Wait();
				MelonLogger.Msg("[IRC ->] " + TwitchIRCClient.UTF8_ENCODING.GetString(buffer));
			}

			/// <summary>Pong command for when twitch issues a PING</summary>
			public void Pong() {
				MelonLogger.Msg("[IRL Cliect] Pong()");
				if (this.webSocket == null) {
					return;
				}

				byte[] buffer = TwitchIRCClient.UTF8_ENCODING.GetBytes("PONG :tmi.twitch.tv");
				this.webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None)
						.Wait();
				MelonLogger.Msg("[IRC ->] " + TwitchIRCClient.UTF8_ENCODING.GetString(buffer));
			}

			/// <summary>Adds a delegate to be invoked once a twitch account has been authenticated.</summary>
			public void AddOnPrivateMessageDelegate(OnPrivateMessage withDelegate) {
				this.onPrivateMessageDelegates.Add(withDelegate);
			}

			/// <summary>Invokes any linked delegates with a newly received message</summary>
			private void OnPrivateMessage(string withNickName, string withMessage) {
				foreach(var d in this.onPrivateMessageDelegates) {
					try {
						d(withNickName, withMessage);
					} catch {}
				}
			}

			/// <summary>Websicket listerner</summary>
			private async void RunWebSocketListener() {
				MelonLogger.Msg("Running IRC WebSocket Listener");

				while (true) {
					try {
						if (this.webSocket == null) {
							continue;
						}

						if (webSocket.State == WebSocketState.Closed) {
							MelonLogger.Msg("WebSocket has been closed.");
							break;
						}

						if (webSocket.State != WebSocketState.Open) {
							continue;
						}

						byte[] buffer = new byte[TwitchIRCClient.RECEIVE_CHUNK_SIZE];

						var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

						if (result.MessageType == WebSocketMessageType.Close) {
							await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
							continue;
						}

						MelonLogger.Msg("[IRC <-] " + TwitchIRCClient.UTF8_ENCODING.GetString(buffer));

						// split the incomming messages by line
						string[] lines = TwitchIRCClient.UTF8_ENCODING.GetString(buffer).Split('\n');
						foreach(string line in lines) {
							if (line.Trim().Length == 0) {
								continue;
							}

							string[] subparts = line.Split(new char[] {':'}, 3);

							// ping command
							if (subparts.Length == 2) {
								if (subparts[0].Trim() == "PING") {
									this.Pong();
								}
								continue;
							}

							// incoming private message
							if (subparts.Length == 3) {
								string[] metaParts = subparts[1].Split(' ');
								if (metaParts.Length >= 3 && metaParts[1] == "PRIVMSG") {
									this.OnPrivateMessage(metaParts[2].Substring(1), subparts[2]);
								}
								continue;
							}

							// unknown command
							MelonLogger.Msg("UNKNOWN COMMAND: " + line);
							continue;
						}
					} catch {
						MelonLogger.Msg("[IRC Client] EXCEPTION CAUGHT");
					}
				}

				MelonLogger.Msg("Stopping IRC WebSocket Listener");
			}
		}
	}
}