namespace Bitzophrenia {
	namespace Actions {
		public abstract class AbstractAction : Bitzophrenia.IAction {

			private string _summary;

			private Bitzophrenia.Phasma.Global _phasmophobia;

			public AbstractAction(string summary, Bitzophrenia.Phasma.Global phasmophobia) {
				this._summary = summary;
				this._phasmophobia = phasmophobia;
			}

			public abstract void Execute();

			public string Summary()
			{
				return this._summary;
			}

			protected Bitzophrenia.Phasma.Global Phasmophobia() {
				return this._phasmophobia;
			}
		}
	}
}