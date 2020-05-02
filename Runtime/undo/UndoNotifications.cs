using BeatThat.Notifications;

namespace BeatThat.Undo
{
	public static class UndoNotifications 
	{

		[NotificationType]
		public const string UNDO = "undo";

		[NotificationType]
		public const string REDO = "redo";

	}
}

