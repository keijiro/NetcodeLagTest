using UnityEngine;
using Klak.Math;

public sealed class ObjectRotation : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 10f;

    Vector3 _prevPosition;

    void Update()
    {
        var delta = transform.position - _prevPosition;
        var rot = Quaternion.LookRotation(delta.normalized);
        transform.rotation = ExpTween.Step(transform.rotation, rot, _rotationSpeed);
        _prevPosition = transform.position;
    }
}
