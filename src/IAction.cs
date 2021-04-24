
namespace Bitzophrenia
{
	public interface IAction
	{

		/// <summary>Returns a short, single-line, summay describing what the command does.</summary>
		string Summary();

		/// <summary>Executes the command.</summary>
		void Execute();
	}
}