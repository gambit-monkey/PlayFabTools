#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(GameEventListener))]
    [CanEditMultipleObjects]
    internal class GameEventListenerEditor : Editor
    {
        SerializedProperty gameEvent;
        SerializedProperty response;
        SerializedProperty responseDescription;

        private void OnEnable()
        {
            gameEvent = serializedObject.FindProperty("Event");
            response = serializedObject.FindProperty("Response");
            responseDescription = serializedObject.FindProperty("responseDescription");
        }

        public override void OnInspectorGUI()
        {
            GameEventListener targetScript = (GameEventListener)target;

            serializedObject.Update();

            EditorGUILayout.PropertyField(gameEvent);

            if (targetScript.Event != null) //If the gameEvent value is not empty - get the gameEvent description
            {
                GUIStyle boldStyle = new GUIStyle();
                boldStyle.richText = true;

                EditorGUILayout.LabelField("Event Description", targetScript.Event.eventDescription, EditorStyles.helpBox);
            }

            EditorGUILayout.PropertyField(responseDescription);

            EditorGUILayout.PropertyField(response);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif

