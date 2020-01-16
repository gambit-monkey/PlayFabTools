using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameEvent Event; //Which event does this listen for
        public UnityEvent Response; //Reponse to happen when the event is raised

        [TextArea]
        [Tooltip("What does this object do when the attached event is raised")]
        public string responseDescription = "[What does this object do in response to this event]";

        private void OnEnable()
        {
            //If the event is not null, register this component/gameObject
            if (Event != null)
            {
                Event.RegisterListener(this);
            }
        }

        private void OnDisable()
        {
            //If the event is not null, unregister this component/gameObject
            if (Event != null)
            {
                Event.UnregisterListener(this);
            }
        }

        /// <summary>
        /// Raise the response set to this event
        /// </summary>
        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}