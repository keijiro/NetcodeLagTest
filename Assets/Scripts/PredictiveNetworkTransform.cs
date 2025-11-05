using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public sealed class PredictiveNetworkTransform : NetworkTransform
{
    [SerializeField] bool _extrapolate = false;
    [SerializeField] Transform _offsetXform = null;

    Vector3 _velocity, _offset;
    MarkerPool _markerPool;

    protected override void OnNetworkTransformStateUpdated
      (ref NetworkTransformState oldState, ref NetworkTransformState newState)
    {
        base.OnNetworkTransformStateUpdated(ref oldState, ref newState);

        var tickRate = (float)NetworkManager.NetworkConfig.TickRate;

        var oldTick = oldState.GetNetworkTick();
        var newTick = newState.GetNetworkTick();
        var localTick = NetworkManager.Singleton.LocalTime.Tick;

        var dt = Mathf.Max(1, newTick - oldTick) / tickRate;
        _velocity = (newState.GetPosition() - oldState.GetPosition()) / dt;
        _offset = _velocity * ((localTick - newTick) / tickRate - Time.deltaTime);

        _markerPool.PutMarker(newState.GetPosition());
    }

    void Start()
      => _markerPool = FindFirstObjectByType<MarkerPool>();

    void Update()
    {
        _offset += _velocity * Time.deltaTime;

        if (!IsServer && _extrapolate)
            _offsetXform.position = transform.position + _offset;
    }
}
