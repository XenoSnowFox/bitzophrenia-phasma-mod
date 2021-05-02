

using MelonLoader;

namespace Bitzophrenia
{

	namespace Twitch
	{

		public class TwitchController
		{

			private string clientId;

			private string authenticationCode;

			private Bitzophrenia.Twitch.TwitchOAuth2Client oAuth2Client;

			private Bitzophrenia.Twitch.TwitchIRCClient ircClient = new Bitzophrenia.Twitch.TwitchIRCClient();

			private Bitzophrenia.Twitch.TwitchPubSubClient pubSubClient = new Bitzophrenia.Twitch.TwitchPubSubClient();

			public TwitchController(string withClientId)
			{
				this.clientId = withClientId;

				this.oAuth2Client = new Bitzophrenia.Twitch.TwitchOAuth2Client(withClientId);
				this.oAuth2Client.AddOnAuthenticationDelegate(this.OnAuthentication);
				this.oAuth2Client.StartAuthenticationFlow();

			}

			public Bitzophrenia.Twitch.TwitchIRCClient GetIRCClient() {
				return this.ircClient;
			}

			public Bitzophrenia.Twitch.TwitchPubSubClient GetPubSubClient() {
				return this.pubSubClient;
			}

			private void OnAuthentication(string withAuthenticationCode) {
				MelonLogger.Msg("[TWITCH CONTROLLER] Authenticated.");
				this.authenticationCode = withAuthenticationCode;

				// start the IRC client
				this.ircClient.Connect();
				this.ircClient.Authenticate(withAuthenticationCode);
				this.ircClient.SetNickName(Bitzophrenia.Properties.Twitch.Username);
				this.ircClient.JoinChannel(Bitzophrenia.Properties.Twitch.Username);

				// start the pubsub client
				this.pubSubClient.Connect();
				this.pubSubClient.Authenticate(withAuthenticationCode);
				this.pubSubClient.Listen();
			}

		}
	}
}

