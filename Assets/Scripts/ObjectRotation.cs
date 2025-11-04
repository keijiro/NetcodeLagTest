using UnityEngine;
using Klak.Math;

public sealed class ObjectRotation : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 10f;

    (Vector3 pos, Vector3 dir) _prev;

    void LateUpdate()
    {
        var dir = (transform.position - _prev.pos).normalized;

        if (Vector3.Dot(dir, _prev.dir) > 0)
        {
            var rot = Quaternion.LookRotation(dir);
            transform.rotation = ExpTween.Step(transform.rotation, rot, _rotationSpeed);
        }

        _prev = (transform.position, dir);
    }
}
