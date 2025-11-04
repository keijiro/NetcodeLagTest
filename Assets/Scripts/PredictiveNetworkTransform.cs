using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public sealed class PredictiveNetworkTransform : NetworkTransform
{
    [SerializeField] Transform _offsetXform = null;

    Vector3 _velocity;
    MarkerPool _markerPool;
    int _receivedFrame;

    protected override void OnNetworkTransformStateUpdated
      (ref NetworkTransformState oldState, ref NetworkTransformState newState)
    {
        base.OnNetworkTransformStateUpdated(ref oldState, ref newState);

        var tickRate = (float)NetworkManager.NetworkConfig.TickRate;

        var dt = (newState.GetNetworkTick() - oldState.GetNetworkTick()) / tickRate;
        _velocity = (newState.GetPosition() - oldState.GetPosition()) / dt;

        var latency = (NetworkManager.Singleton.LocalTime.Tick - newState.GetNetworkTick()) / tickRate;
        _offsetXform.localPosition = _velocity * latency;

        _markerPool.PutMarker(newState.GetPosition());

        _receivedFrame = Time.frameCount;
    }

    void Start()
      => _markerPool = FindFirstObjectByType<MarkerPool>();

    void Update()
    {
        if (!IsServer && _receivedFrame != Time.frameCount)
            _offsetXform.localPosition += _velocity * Time.deltaTime;
    }
}
