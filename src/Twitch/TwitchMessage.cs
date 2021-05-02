using Newtonsoft.Json;

namespace Bitzophrenia {
	namespace Twitch {
		public class TwitchMessage {
			public string type { get; set; }
			public string error { get; set; }
			public string nonce { get; set; }
			public TwitchMessageData data { get; set; }

			public TwitchChannelPointsMessage ToChannelPointsMessage() {
				if (this.data == null || this.data.topic != "channel-points-channel-v1.125483513") {
					return null;
				}
				return JsonConvert.DeserializeObject<TwitchChannelPointsMessage>(this.data.message);
			}
		}

		public class TwitchMessageData {
			public string topic { get; set; }
			public string message { get; set; }
		}

		public class TwitchChannelPointsMessage {
			public string type { get; set; }
			public TwitchChannelPointsMessageData data { get; set; }
		}

		public class TwitchChannelPointsMessageData {
			public string timestamp { get; set; }
			public TwitchChannelPointsMessageRedemptionData redemption { get; set; }
		}

		public class TwitchChannelPointsMessageRedemptionData {
			public string id { get; set; }
			public TwitchChannelPointsMessageUserData user { get; set; }
			public string channel_id { get; set; }
			public string redeemed_at { get; set; }
			public TwitchChannelPointsMessageRewardData reward { get; set; }
		}

		public class TwitchChannelPointsMessageRewardData {
			public string id { get; set; }
			public string channel_id { get; set; }
			public string title { get; set; }
			public string prompt { get; set; }
			public int cost { get; set; }
			public bool is_user_input_required { get; set; }
			public bool is_sub_only { get; set; }
		}

		public class TwitchChannelPointsMessageUserData {
			public string id { get; set; }
			public string login { get; set; }
			public string display_name { get; set; }
		}
	}
}