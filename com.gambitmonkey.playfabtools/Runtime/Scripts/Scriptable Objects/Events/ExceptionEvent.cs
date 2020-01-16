using System;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "ExceptionGameEvent", menuName = "GambitMonkey/Create/Events/ExceptionGameEvent")]
    [Serializable]
    public class ExceptionEvent : BaseGameEvent<Exception>
    {

    }
}
