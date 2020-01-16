using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(GameEvent))]
    [CanEditMultipleObjects]
    public class ScriptableGameEventEditor : Editor
    {
#if UNITY_EDITOR
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
#endif
    }
}
