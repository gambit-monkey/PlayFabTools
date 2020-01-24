#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(GameEvent))]
    [CanEditMultipleObjects]
    internal class ScriptableGameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //Draw the defualt inspector options
            DrawDefaultInspector();

            GameEvent script = (GameEvent)target;

            //Draw button
            if (GUILayout.Button("Raise Event"))
            {
                //If the application is playing - raise/trigger the event
                if (EditorApplication.isPlaying)
                {
                    script.Raise();
                }
            }
        }
    }
}
#endif

