using System;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FloatGameEvent", menuName = "GambitMonkey/Create/Events/FloatGameEvent")]
    [Serializable]
    public class FloatEvent : BaseGameEvent<float>
    {
     
    }
}
