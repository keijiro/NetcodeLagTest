using UnityEngine;
using Unity.Netcode;

public sealed class AppController : MonoBehaviour
{
#if UNITY_EDITOR
    void Start() => GetComponent<NetworkManager>().StartServer();
#else
    void Start() => GetComponent<NetworkManager>().StartClient();
#endif
}
