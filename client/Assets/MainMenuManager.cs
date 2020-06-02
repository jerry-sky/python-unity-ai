using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private ServerCommunication _serverCommunication;


    [SerializeField]
    private GameObject waitForConnection;

    [SerializeField]
    private GameObject mainMenuSettings;

    private void Start()
    {
        waitForConnection.SetActive(true);
        mainMenuSettings.SetActive(false);
        _serverCommunication.simulationNamespace.onConnect.AddListener(ShowButton);
    }

    void ShowButton()
    {
        waitForConnection.SetActive(false);
        mainMenuSettings.SetActive(true);
    }
}
