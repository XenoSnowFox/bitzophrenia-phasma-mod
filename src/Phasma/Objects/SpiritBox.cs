using UnityEngine;

namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Objects
		{

			public class SpiritBox
			{

				private global::EVPRecorder instance;

				public SpiritBox(global::EVPRecorder withInstance)
				{
					this.instance = withInstance;
				}

				public global::EVPRecorder GetInstance() {
					return this.instance;
				}

				public void PlayRandomAudioClip() {
					var rnd = Random.RandomRangeInt(0,3); // 1=about, 2=location, 3=difficulty
					var clips = rnd == 0
							? this.instance.aboutAnswerClips
							: rnd == 1
									? this.instance.locationAnswerClips
									: this.instance.difficultyAnswerClips;

					var rndClipIndex = Random.RandomRangeInt(0, clips.Count);
					string command = "";
					if (rnd == 0) {
						this.instance.PlayAboutSound(rndClipIndex);
					} else if (rnd == 1) {
						command = "PlayAboutSound";
						this.instance.PlayLocationSound(rndClipIndex);
						command = "PlayLocationSound";
					} else {
						this.instance.PlayLocationSound(rndClipIndex);
						command = "PlayLocationSound";
					}

					this.instance.field_Public_AudioSource_0.PlayOneShot(clips[rndClipIndex]); // on ground
					this.instance.field_Public_AudioSource_1.PlayOneShot(clips[rndClipIndex]); // being held

					RPC.UsingPhotonView(this.instance.view)
							.ExecuteMethod(command)
							.WithParameter(true)
							.WithParameter(rndClipIndex)
							.OnAllTargets();
				}

			}
		}
	}
}