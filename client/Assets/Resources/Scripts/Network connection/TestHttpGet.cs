using TMPro;
using UnityEngine;

public class TestHttpGet : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField = default;
    [SerializeField] private ServerCommunication communication = default;

    public void GetSimpleText()
    {
        textField.text = communication.Get("/");
    }

    public void GetJson()
    {
        textField.text = JsonUtility.FromJson<TestModel>(communication.Get("/data")).more;
    }
}

[System.Serializable]
public class TestModel
{
    public string more;
}
