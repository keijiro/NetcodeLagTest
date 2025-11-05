using UnityEngine;
using Unity.Netcode.Components;

public sealed class ObjectController : MonoBehaviour
{
    [SerializeField] Vector2 _pathSize = new Vector2(4, 2);
    [SerializeField] float _speed = 1;

    float Perimeter => 2 * (_pathSize.x + _pathSize.y);

    float _progress;

    (float, float, float) GetXZYaw()
    {
        var (w, d, seg) = (_pathSize.x, _pathSize.y, _progress);
        var (hw, hd) = (w / 2, d / 2);

        if (seg < w) return (-hw + seg, hd, 90);
        seg -= w;

        if (seg < d) return (hw, hd - seg, 180);
        seg -= d;

        if (seg < w) return (hw - seg, -hd, 270);
        seg -= w;

        return (-hw, -hd + seg, 0);
    }

    void Start()
      => enabled = GetComponent<NetworkTransform>().IsServer;

    void FixedUpdate()
    {
        _progress = (_progress + _speed * Time.fixedDeltaTime) % Perimeter;
        var (x, z, ry) = GetXZYaw();
        transform.position = new Vector3(x, 0, z);
        transform.rotation = Quaternion.AngleAxis(ry, Vector3.up);
    }
}
