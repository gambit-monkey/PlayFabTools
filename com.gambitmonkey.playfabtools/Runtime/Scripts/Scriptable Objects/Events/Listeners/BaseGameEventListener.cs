using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IBaseGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
    {
        [SerializeField]
        private E gameEvent;

        public E GameEvent
        {
            get
            {
                return gameEvent;
            }
            set
            {
                gameEvent = value;
            }
        }

        [SerializeField]
        public UER unityEventResponse;

        [TextArea]
        [Tooltip("What does this object do when the attached event is raised")]
        public string responseDescription = "[What does this object do in response to this event]";

        private void OnEnable()
        {
            if (gameEvent == null)
            {
                return;
            }

            GameEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (gameEvent == null)
            {
                return;
            }

            GameEvent.UnregisterListener(this);
        }

        public void OnEventRaised(T item)
        {
            if (unityEventResponse != null)
            {
                unityEventResponse.Invoke(item);
            }
        }
    }
}