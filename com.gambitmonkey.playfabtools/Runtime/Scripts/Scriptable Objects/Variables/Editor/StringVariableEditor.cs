#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(StringVariable))]
    [CanEditMultipleObjects]
    internal class SOStringEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //Draw the defualt inspector options
            DrawDefaultInspector();

            StringVariable script = (StringVariable)target;

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.LabelField("Debugging Options", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.LabelField("Current value: " + script.Value, EditorStyles.boldLabel);

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
    }
}
#endif
