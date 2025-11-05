using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public sealed class PredictiveNetworkTransform : NetworkTransform
{
    [SerializeField] bool _extrapolate = false;
    [SerializeField] Transform _offsetXform = null;

    Vector3 _velocity;
    MarkerPool _markerPool;
    int _receivedFrame;

    protected override void OnNetworkTransformStateUpdated
      (ref NetworkTransformState oldState, ref NetworkTransformState newState)
    {
        base.OnNetworkTransformStateUpdated(ref oldState, ref newState);

        var tickRate = (float)NetworkManager.NetworkConfig.TickRate;

        var oldTick = oldState.GetNetworkTick();
        var newTick = newState.GetNetworkTick();
        var localTick = NetworkManager.Singleton.LocalTime.Tick;

        var dt = (newTick - oldTick) / tickRate;
        _velocity = (newState.GetPosition() - oldState.GetPosition()) / dt;

        if (_extrapolate)
        {
            var latency = (localTick - newTick) / tickRate;
            _offsetXform.localPosition = _velocity * latency;
        }

        _markerPool.PutMarker(newState.GetPosition());

        //_receivedFrame = Time.frameCount;
    }

    void Start()
      => _markerPool = FindFirstObjectByType<MarkerPool>();

    void Update()
    {
        if (!IsServer && _extrapolate && _receivedFrame != Time.frameCount)
            _offsetXform.localPosition += _velocity * Time.deltaTime;
    }
}
