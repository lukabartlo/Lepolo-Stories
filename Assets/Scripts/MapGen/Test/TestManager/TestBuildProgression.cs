using System.Collections.Generic;
using UnityEngine;

public class TestBuildProgression {
    private Dictionary<ObjectType, ObjectData> _objectDatas = new();

    public TestBuildProgression(List<ObjectWrapper> _allMapableObjects) {
        foreach (var obj in _allMapableObjects) {
            _objectDatas.Add(obj.objectType, obj.objectData);
        }
    }

    public ObjectData GetObjectByType(ObjectType _objectType) {
        return _objectDatas.GetValueOrDefault(_objectType);
    }
}
