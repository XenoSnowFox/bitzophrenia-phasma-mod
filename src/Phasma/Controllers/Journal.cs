
namespace Bitzophrenia
{
	namespace Phasma
	{
		namespace Controllers
		{
			public class Journal
			{

				/// <summary>
				/// Instantiates a new instance.
				/// </summary>
				/// <param name="withController">Phasma Game Controller instance.</param>
				public Journal(JournalController withController)
				{
					this.controller = withController;
				}

				/// <summary>Phasma Level Controller instance.</summary>
				private JournalController controller;

				// #TODO: public Evidence getEvidence1
				// #TODO: public Evidence getEvidence2
				// #TODO: public Evidence getEvidence3
			}
		}
	}
}