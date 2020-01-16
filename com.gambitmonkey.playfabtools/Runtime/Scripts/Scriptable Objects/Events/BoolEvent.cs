using System;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "BoolGameEvent", menuName = "GambitMonkey/Create/Events/BoolGameEvent")]
    [Serializable]
    public class BoolEvent : BaseGameEvent<bool>
    {
     
    }
}
