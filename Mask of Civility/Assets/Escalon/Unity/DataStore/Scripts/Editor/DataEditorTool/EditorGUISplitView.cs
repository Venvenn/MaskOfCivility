using UnityEngine;
using UnityEditor;

namespace Escalon
{
	public class EditorGUISplitView
	{
		public enum Direction
		{
			Horizontal,
			Vertical
		}

		private readonly Direction _splitDirection;

		private float _splitNormalizedPosition;
		private bool _resize;
		private Rect _availableRect;

		public Vector2 ScrollPosition;

		/// <param name="splitPoint">Section of the screen where the split occurs, from 0 to 1</param>
		public EditorGUISplitView(Direction splitDirection, float splitPoint = 0.5f)
		{
			_splitNormalizedPosition = splitPoint;
			_splitDirection = splitDirection;
		}

		public void BeginSplitView()
		{
			Rect tempRect;

			if (_splitDirection == Direction.Horizontal)
			{
				tempRect = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
			}
			else
			{
				tempRect = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
			}

			if (tempRect.width > 0.0f)
			{
				_availableRect = tempRect;
			}

			if (_splitDirection == Direction.Horizontal)
			{
				ScrollPosition = GUILayout.BeginScrollView(ScrollPosition,
					GUILayout.Width(_availableRect.width * _splitNormalizedPosition));
			}
			else
			{
				ScrollPosition = GUILayout.BeginScrollView(ScrollPosition,
					GUILayout.Height(_availableRect.height * _splitNormalizedPosition));
			}
		}

		public void Split()
		{
			GUILayout.EndScrollView();
			ResizeSplitFirstView();
		}

		public void EndSplitView()
		{
			if (_splitDirection == Direction.Horizontal)
			{
				EditorGUILayout.EndHorizontal();
			}
			else
			{
				EditorGUILayout.EndVertical();
			}
		}

		private void ResizeSplitFirstView()
		{
			Rect resizeHandleRect;

			if (_splitDirection == Direction.Horizontal)
			{
				resizeHandleRect = new Rect(_availableRect.width * _splitNormalizedPosition, _availableRect.y, 2f,
					_availableRect.height);
			}
			else
			{
				resizeHandleRect = new Rect(_availableRect.x, _availableRect.height * _splitNormalizedPosition,
					_availableRect.width, 2f);
			}

			GUI.DrawTexture(resizeHandleRect, EditorGUIUtility.whiteTexture);

			if (_splitDirection == Direction.Horizontal)
			{
				EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeHorizontal);
			}
			else
			{
				EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeVertical);
			}

			if (Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition))
			{
				_resize = true;
			}

			if (_resize)
			{
				if (_splitDirection == Direction.Horizontal)
					_splitNormalizedPosition = Event.current.mousePosition.x / _availableRect.width;
				else
					_splitNormalizedPosition = Event.current.mousePosition.y / _availableRect.height;
			}

			if (Event.current.type == EventType.MouseUp)
				_resize = false;
		}
	}
}