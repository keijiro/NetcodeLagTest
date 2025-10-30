using System.Collections.Generic;
using UnityEngine;

public sealed class MarkerPool : MonoBehaviour
{
    [SerializeField] GameObject _prefab = null;
    [SerializeField] int _maxInstances = 16;

    readonly Queue<GameObject> _instances = new Queue<GameObject>();

    public void PutMarker(Vector3 position)
    {
        if (_instances.Count < _maxInstances)
        {
            var go = Instantiate(_prefab, position, Quaternion.identity, transform);
            _instances.Enqueue(go);
        }
        else
        {
            var recycled = _instances.Dequeue();
            recycled.transform.position = position;
            _instances.Enqueue(recycled);
        }
    }
}
