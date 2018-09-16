using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace BeatThat.Undo
{
    /// <summary>
    /// Undo/Redo support via a change history of <c>UndoableChange</c> objects 
    /// </summary>
    public class ChangeHistory 
	{
		public UnityEvent onUpdated { get { return m_onUpdated; } }
		private UnityEvent m_onUpdated = new UnityEvent();

		public void Execute(UndoableChange c)
		{
			c.Do();
			Add(c);
		}

		public void Add(UndoableChange c)
		{
			m_undoStack.Add(c);
			m_redoStack.Clear();
			this.onUpdated.Invoke();
		}

		public bool hasUndo { get { return this.undoCount > 0;; } }

		public bool hasRedo { get { return this.redoCount > 0; } }

		public int undoCount { get { return m_undoStack.Count; } }

		public int redoCount { get { return m_redoStack.Count; } }

		/// <summary>
		/// Get a copy of the undo stack for inspection purposes.
		///  Generally callers should NOT call UNDO/REDO or otherwise try to alter changes in stack.
		/// </summary>
		public void GetUndoStack(List<UndoableChange> undoStack)
		{
			undoStack.AddRange(m_undoStack);
		}

		public void Undo() 
		{ 
			TrackChange(m_undoStack, m_redoStack, c => c.Undo ());
		}
		
		public void Redo()
		{
			TrackChange(m_redoStack, m_undoStack, c => c.Do () );
		}
		
		public void Clear()
		{
			if(m_undoStack.Count > 0 || m_redoStack.Count > 0) {
				m_undoStack.Clear();
				m_redoStack.Clear();
				this.onUpdated.Invoke();
			}
		}

		public void PrintStack()
		{
			if(m_undoStack.Count < 1 && m_redoStack.Count < 1) {
				Debug.LogError("both stacks empty");
			}

			if(m_undoStack.Count > 0) {
				Debug.LogError(m_undoStack.Select(i => i.ToString()).Aggregate((i,j) => i + "\n" + j));
			}

			if(m_redoStack.Count > 0) {
				Debug.LogError(m_redoStack.Select(i => i.ToString()).Aggregate((i,j) => i + "\n" + j));
			}
		}
		
		private void TrackChange(IList<UndoableChange> fromList, ICollection<UndoableChange> toList, System.Action<UndoableChange> change)
		{
			int n = fromList.Count - 1;
			if(n < 0) {
				Debug.LogError("[" + Time.frameCount + "] " + GetType() + "::TrackChange no change in stack");
				return;
			}

			UndoableChange c = fromList[n];

//			Debug.LogError("[" + Time.frameCount + "] " + GetType() + "::TrackChange " + c);

			fromList.RemoveAt(n);
			change(c);
			toList.Add(c);
			this.onUpdated.Invoke();
		}

		private List<UndoableChange> m_undoStack = new List<UndoableChange>();
		private List<UndoableChange> m_redoStack = new List<UndoableChange>();

	}
}



