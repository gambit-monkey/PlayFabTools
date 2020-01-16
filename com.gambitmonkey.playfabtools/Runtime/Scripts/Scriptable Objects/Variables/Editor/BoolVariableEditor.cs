using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(BoolVariable))]
    [CanEditMultipleObjects]
    public class BoolVariableEditor : Editor
    {
#if UNITY_EDITOR
        public override void OnInspectorGUI()
        {
            //Draw the defualt inspector options
            DrawDefaultInspector();

            BoolVariable script = (BoolVariable)target;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Debugging Options", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.LabelField("Current value: " + script.Value, EditorStyles.boldLabel);

            //Display button that toggles the bool value
            if (GUILayout.Button("Toggle Value"))
            {
                if (EditorApplication.isPlaying)
                {
                    script.Toggle();
                }
            }

            //Display button that resets the value to the starting value
            if (GUILayout.Button("Reset Value"))
            {
                if (EditorApplication.isPlaying)
                {
                    script.ResetValue();
                }
            }
            EditorGUILayout.EndVertical();
        }
#endif
    }
}
