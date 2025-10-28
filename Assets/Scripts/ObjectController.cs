using UnityEngine;
using Unity.Netcode.Components;

public sealed class ObjectController : MonoBehaviour
{
    [SerializeField] Vector2 _pathSize = new Vector2(4, 2);
    [SerializeField] float _speed = 1;

    float Perimeter => 2 * (_pathSize.x + _pathSize.y);

    float _progress;

    void Update()
    {
        if (!GetComponent<NetworkTransform>().CanCommitToTransform) return;
        _progress = Mathf.Repeat(_progress + _speed * Time.deltaTime, Perimeter);
        transform.position = EvaluateOffset();
    }

    Vector3 EvaluateOffset()
    {
        var width = _pathSize.x;
        var depth = _pathSize.y;
        var segment = _progress;

        if (segment < width)
            return new Vector3(-0.5f * width + segment, 0f, 0.5f * depth);

        segment -= width;
        if (segment < depth)
            return new Vector3(0.5f * width, 0f, 0.5f * depth - segment);

        segment -= depth;
        if (segment < width)
            return new Vector3(0.5f * width - segment, 0f, -0.5f * depth);

        segment -= width;
        return new Vector3(-0.5f * width, 0f, -0.5f * depth + segment);
    }
}
