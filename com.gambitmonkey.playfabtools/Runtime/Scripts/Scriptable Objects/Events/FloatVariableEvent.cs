using System;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "FloatVariableGameEvent", menuName = "GambitMonkey/Create/Events/FloatVariableGameEvent")]
    [Serializable]
    public class FloatVariableEvent : BaseGameEvent<FloatVariable>
    {
     
    }
}
