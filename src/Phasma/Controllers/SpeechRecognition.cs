using System.Collections.Generic;

namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Controllers
		{
			public class SpeechRecognition
			{

				/// <summary>Phasma Speech Recognition Controller instance.</summary>
				private SpeechRecognitionController controller;

				/// <summary>
				/// Instantiates a new instance.
				/// </summary>
				/// <param name="withController">Phasma Speech Recognition Controller instance.</param>
				public SpeechRecognition(SpeechRecognitionController withController)
				{
					this.controller = withController;
				}

				public List<Bitzophrenia.Phasma.Objects.SpiritBox> ListSpiritBoxes() {
					var list = new List<Bitzophrenia.Phasma.Objects.SpiritBox>();
					foreach(var spiritBox in this.controller.field_Private_List_1_EVPRecorder_0) {
						list.Add(new Bitzophrenia.Phasma.Objects.SpiritBox(spiritBox));
					}
					return list;
				}
			}
		}
	}
}