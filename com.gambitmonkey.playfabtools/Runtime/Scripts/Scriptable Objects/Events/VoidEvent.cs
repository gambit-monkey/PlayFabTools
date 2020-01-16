using System;
using UnityEngine;
using UnityEngine.Events;

namespace GambitMonkey.ScriptableObjects
{
    [CreateAssetMenu(fileName = "VoidGameEvent", menuName = "GambitMonkey/Create/Events/VoidGameEvent")]
    [Serializable]
    public class VoidEvent : BaseGameEvent<Void>
    {
        public void Raise() => Raise(new Void());
    }
}
