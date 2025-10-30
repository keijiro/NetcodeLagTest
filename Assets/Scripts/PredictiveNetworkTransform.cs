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

        var tickLatency = GetTickLatency();
        var tickRate = NetworkManager.NetworkConfig.TickRate;
        var latencyInSeconds = tickLatency / (float)tickRate;

        _offsetXform.localPosition = _lastVelocity * (latencyInSeconds - Time.deltaTime);
    }

    void Update()
    {
        if (IsServer) return;

        _offsetXform.localPosition += _lastVelocity * Time.deltaTime;
    }
}
