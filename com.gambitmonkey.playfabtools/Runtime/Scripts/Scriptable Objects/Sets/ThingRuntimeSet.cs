// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;

namespace GambitMonkey.ScriptableObjects.Sets
{
    [CreateAssetMenu(menuName = "GambitMonkey/Create/RuntimeSets/Thing")]
    public class ThingRuntimeSet : RuntimeSet<Thing>
    {}
}