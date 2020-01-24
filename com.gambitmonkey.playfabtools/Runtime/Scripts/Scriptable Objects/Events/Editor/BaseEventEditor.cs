#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GambitMonkey.ScriptableObjects
{
    [CustomEditor(typeof(FloatEvent))]
    [CanEditMultipleObjects]
    internal class ScriptableFloatGameEventEditor : Editor
    {

        [SerializeField]
        public float hSliderValue = 0.0F;
        public override void OnInspectorGUI()
        {
            //Draw the defualt inspector options
            DrawDefaultInspector();

            FloatEvent script = (FloatEvent)target;

            hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, 0.0F, 10.0F);

            //Draw button
            if (GUILayout.Button("Raise Event"))
            {
                //If the application is playing - raise/trigger the event
                if (EditorApplication.isPlaying)
                {
                    script.Raise(hSliderValue);
                }
            }
        }
    }
}
#endif

