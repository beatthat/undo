
namespace BeatThat
{
	/// <summary>
	/// Encapsulates a single change to enable the undo/redo system.
	/// </summary>
	public interface UndoableChange  
	{
		void Update(); // TODO: shouldn't really be here. Maybe a special subtype 'UpdatableChange'?
		
		void Do();
		
		void Undo();
	}
}

