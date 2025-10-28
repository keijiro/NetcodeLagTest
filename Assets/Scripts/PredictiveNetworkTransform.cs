using Unity.Netcode.Components;
using UnityEngine;

public sealed class PredictiveNetworkTransform : NetworkTransform
{
    [SerializeField] Transform _offsetXform = null;

    Vector3 _lastPosition;
    Vector3 _lastVelocity;
    float _lastReceiveTime;

    protected override void OnNetworkTransformStateUpdated(
        ref NetworkTransformState oldState,
        ref NetworkTransformState newState)
    {
        base.OnNetworkTransformStateUpdated(ref oldState, ref newState);

        var now = Time.time;
        var position = GetSpaceRelativePosition(true);
        var rotation = GetSpaceRelativeRotation(true);

        _lastVelocity = (position - _lastPosition) / (now - _lastReceiveTime);
        _lastPosition = position;
        _lastReceiveTime = now;

        _offsetXform.localPosition = Vector3.zero;
    }

    void Update()
    {
        if (IsServer || _lastReceiveTime <= 0) return;

        _offsetXform.localPosition += _lastVelocity * Time.deltaTime;
    }
}
