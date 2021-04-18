
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace Bitzophrenia {
	namespace Twitch {

		/// <summary> Callback method for when a twitch account is successfully authenticated </summary>
		public delegate void OnAuthentication(string authenticationCode);

		/// <summary>Utility class for handling Twitch Authentication</summary>
		public class TwitchOAuth2Client {

			/// <summary>Client ID of the registered Twitch Application</summary>
			private string clientId;

			/// <summary>Collection of callbacks to be invoked on a successful authentication</summary>
			private List<OnAuthentication> onAuthenticationDelegates = new List<OnAuthentication>();

			/// <summary>Current instance of the authenticated twitch user's access token.</summary>
			private string accessToken = null;

			/// <summary>Instantiates a new instance.</summary>
			public TwitchOAuth2Client(string withClientId) {
				this.clientId = withClientId;

				// start the webserver thread
				new Thread(this.AuthenticationServerThreadStartDelegate).Start();
			}

			/// <summary>Thread Delegate for setting up an authentication callback.</summary>
			private void AuthenticationServerThreadStartDelegate() {

				HttpListener server = new HttpListener();
				server.Prefixes.Add("http://127.0.0.1/");
				server.Prefixes.Add("http://localhost/");
				server.Start();
				MelonLogger.Msg("Starting Authentication Callback WebServer");

				while (true) {
					HttpListenerContext context = server.GetContext();
					HttpListenerResponse response = context.Response;

					string page = context.Request.Url.LocalPath;

					string query = context.Request.Url.Query;
					string[] queryParts = query.Split('&');
					foreach (string part in queryParts) {
						if (part.Length <= 1) {
							continue;
						}

						string[] p = part.Split(new char[1] { '=' }, 2);
						if (p.Length != 2) {
							continue;
						}

						if (p[0].Equals("access_token") || p[0].Equals("?access_token")) {
							this.OnAuthentication(p[1]);
						}
					}

					string str = "<html>";
					str += "<head>";
					str += "<script type='text/javascript'>";
					if (this.accessToken == null) {
						str += "var h = window.location.hash.substr(1);";
						str += "if (h) {";
						str += "window.location = \"http://localhost?\" + h;";
						str += "}";
					} else {
						str += "window.close();";
					}
					str += "</script>";
					str += "</head>";
					str += "<body>";
					str += "<h1>Logging Into Twitch</h1>";
					str += "</body>";
					str += "</html>";

					byte[] buffer = Encoding.UTF8.GetBytes(str);
					response.ContentLength64 = buffer.Length;
					Stream st = response.OutputStream;
					st.Write(buffer, 0, buffer.Length);

					context.Response.Close();
				}
			}

			/// <summary>Adds a delegate to be invoked once a twitch account has been authenticated.</summary>
			public void AddOnAuthenticationDelegate(OnAuthentication withDelegate) {
				this.onAuthenticationDelegates.Add(withDelegate);
			}

			/// <summary>Internal delegate that is called to initiate a successful authentication.</summary>
			private void OnAuthentication(String withAuthenticationCode) {
				this.accessToken = withAuthenticationCode;
				foreach (var d in this.onAuthenticationDelegates) {
					d(withAuthenticationCode);
				}
			}

			/// <summary>Returns the URL to open, that starts the authentication process.</summary>
			public string GetAuthenticationUrl() {
				return "https://id.twitch.tv/oauth2/authorize?client_id=" + this.clientId
						+ "&redirect_uri=http://localhost"
						+ "&response_type=token"
						+ "&scope=bits:read%20channel:read:redemptions%20chat:edit%20chat:read";
			}

			/// <summary>Starts the authentication process.</summary>
			public void StartAuthenticationFlow() {
				MelonLogger.Msg("Starting Twitch Authentication Flow");
				System.Diagnostics.Process.Start(this.GetAuthenticationUrl());
			}

			/// <summary>Returns the access token, if it has been created.</summary>
			public string GetAccessToken() {
				return this.accessToken;
			}
		}
	}
}