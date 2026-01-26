using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

// [CanEditMultipleObjects]
// [CustomEditor(typeof(ObjectAnimation), true)]
// public class ObjectAnimationEditor : Editor   
// {
//     public override void OnInspectorGUI()
//     {
//         base.OnInspectorGUI();
//         
//         ObjectAnimation targetObject = (ObjectAnimation)target;
//         
//         if(GUILayout.Button("Play Once"))
//         {
//             Task task = targetObject.PlayOnce();
//             RunTask(task, targetObject);
//         }
//         if(GUILayout.Button("Rewind Once"))
//         {
//             Task task = targetObject.RewindOnce();
//             RunTask(task, targetObject);
//         }
//         
//         if (GUILayout.Button("Play and Rewind Once"))
//         {
//             Task task = targetObject.PingPongOnce();
//             RunTask(task, targetObject);
//         }
//     }
//
//     private async void RunTask(Task task, ObjectAnimation targetObject)
//     {
//         while (!task.IsCompleted)
//         {
//             EditorUtility.SetDirty(targetObject);
//             await Task.Yield();
//         }
//     }
// }
