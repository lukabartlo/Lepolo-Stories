using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Blackboard : MonoBehaviour
{
    Dictionary<string, object> blackboardValues = new Dictionary<string, object>();

    private object GetValue(string _key)
    {
        return blackboardValues.TryGetValue(_key, out var value) ? value : null;
    }

    private void AddValue(string _key, object _value)
    {
        if (!blackboardValues.ContainsKey(_key))
            blackboardValues.Add(_key, _value);
    }
    private void ModifyValue(string _key, object _newValue) 
    {
        if (blackboardValues.ContainsKey(_key))
            blackboardValues[_key] = _newValue;
    }
}
