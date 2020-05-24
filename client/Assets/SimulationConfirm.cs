using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimulationConfirm : MonoBehaviour
{
    public TMP_InputField wolvesTXT,rabbitsTXT,heightTXT,widthTXT;
    public GameObject canvas;
    public CameraMovement cameraMove;

    [SerializeField] private ServerCommunication _serverCommunication;

    public void ButtonClicked()
    {
        SimulationDataModel data = new SimulationDataModel()
        {
            height = Int32.Parse(heightTXT.text),
            width = Int32.Parse(widthTXT.text),
            wolves_count = Int32.Parse(wolvesTXT.text),
            rabbits_count = Int32.Parse(rabbitsTXT.text)
        };
        _serverCommunication.simulationNamespace.StartSimulation(data);
        canvas.SetActive(false);
        cameraMove.enabled = true;
        GridGenerator.Instance.SettingsConfirmed(data.width,data.height,data.wolves_count,data.rabbits_count);
    }
}
