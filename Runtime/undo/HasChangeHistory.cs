
namespace BeatThat.Undo
{
	public interface HasChangeHistory 
	{
		ChangeHistory changeHistory { get; }
	}
}