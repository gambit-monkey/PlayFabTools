using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects.Variables
{
    public class UnityEventRaiser : MonoBehaviour
    {
        public UnityEvent OnEnableEvent;

        public void OnEnable()
        {
            OnEnableEvent.Invoke();
        }
    }
}