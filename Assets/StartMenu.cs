using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;

public sealed class StartMenu : MonoBehaviour
{
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button>("host-button")  .clicked += OnHostButtonClicked;
        root.Q<Button>("server-button").clicked += OnServerButtonClicked;
        root.Q<Button>("client-button").clicked += OnClientButtonClicked;
    }

    void OnHostButtonClicked()
    {
        GetComponent<NetworkManager>().StartHost();
        GetComponent<UIDocument>().enabled = false;
    }

    void OnServerButtonClicked()
    {
        GetComponent<NetworkManager>().StartServer();
        GetComponent<UIDocument>().enabled = false;
    }

    void OnClientButtonClicked()
    {
        GetComponent<NetworkManager>().StartClient();
        GetComponent<UIDocument>().enabled = false;
    }
}
