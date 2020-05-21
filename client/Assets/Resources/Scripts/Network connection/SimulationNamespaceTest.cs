using UnityEngine;
using TMPro;

public class SimulationNamespaceTest : MonoBehaviour
{
    [SerializeField] ServerCommunication server = default;
    private WebSocketSimulationNamespace simulationNamespace;

    [Space]
    [SerializeField] TextMeshProUGUI output = default;

    [Space]
    [SerializeField] private TMP_InputField widthInput = default;
    [SerializeField] private TMP_InputField heightInput = default;
    [SerializeField] private TMP_InputField rabbitInput = default;
    [SerializeField] private TMP_InputField wolfInput = default;

    public void Start()
    {
        simulationNamespace = server.simulationNamespace;
        simulationNamespace.onConnect.AddListener(Connected);
        simulationNamespace.onUpdate.AddListener(BoardUpdate);
    }

    private void Connected()
    {
        output.text = "Connected to server";
    }

    private void BoardUpdate(BoardUpdateModel data)
    {
        output.text = data.print();
    }


    public void StartSimulation()
    {
        SimulationDataModel model = new SimulationDataModel
        {
            width = int.Parse(widthInput.text),
            height = int.Parse(heightInput.text),
            n_wolves = int.Parse(wolfInput.text),
            n_rabbits = int.Parse(rabbitInput.text)
        };
        simulationNamespace.StartSimulation(model);
    }
}
