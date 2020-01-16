using System;
using System.Collections.Generic;
using UnityEngine;

namespace GambitMonkey.ScriptableObjects
{
    [Serializable]
    public abstract class BaseGameEvent<T> : ScriptableObject
    {
        private readonly List<IBaseGameEventListener<T>> eventListeners = new List<IBaseGameEventListener<T>>();

        //Description of when this event is raised
        [TextArea]
        [Tooltip("When is this event raised")]
        public string eventDescription = "[When does this event trigger]";        

        public void Raise(T item)
        {
            for (int i = eventListeners.Count - 1; i>=0;i--)
            {
                eventListeners[i].OnEventRaised(item);
            }
        }

        public void RegisterListener(IBaseGameEventListener<T> listener)
        {
            if (!eventListeners.Contains(listener))
            {
                eventListeners.Add(listener);
            }
        }

        public void UnregisterListener(IBaseGameEventListener<T> listener)
        {
            if (eventListeners.Contains(listener))
            {
                eventListeners.Remove(listener);
            }
        }
    }
}
