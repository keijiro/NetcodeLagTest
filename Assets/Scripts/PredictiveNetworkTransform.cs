using Unity.Netcode.Components;
using UnityEngine;

public sealed class PredictiveNetworkTransform : NetworkTransform
{
    [SerializeField] Transform _offsetXform = null;

    Vector3 _velocity;
    MarkerPool _markerPool;

    protected override void OnNetworkTransformStateUpdated
      (ref NetworkTransformState oldState, ref NetworkTransformState newState)
    {
        base.OnNetworkTransformStateUpdated(ref oldState, ref newState);

        var tickDiff = newState.GetNetworkTick() - oldState.GetNetworkTick();
        var timeDiff = tickDiff / (float)NetworkManager.NetworkConfig.TickRate;

        _velocity = (newState.GetPosition() - oldState.GetPosition()) / timeDiff;

        _offsetXform.localPosition = _velocity * (timeDiff - Time.deltaTime);

        _markerPool.PutMarker(newState.GetPosition());
    }

    void Start()
      => _markerPool = FindFirstObjectByType<MarkerPool>();

    void Update()
    {
        if (IsServer) return;

        _offsetXform.localPosition += _velocity * Time.deltaTime;
    }
}
