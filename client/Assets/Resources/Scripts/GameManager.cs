using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int hight;
    private int width;
    private GameObject[,] gridArray;
    public static GameManager gm;

    private ServerCommunication _serverCommunication;

    public void Awake()
    {
        gm = this;
        _serverCommunication = transform.GetComponent<ServerCommunication>();
        _serverCommunication.simulationNamespace.onUpdate.AddListener(ResponseFromServer);
    }

    public void StartSimulation()
    {
        hight = GridGenerator.Instance.height;
        width = GridGenerator.Instance.width;
        gridArray = GridGenerator.Instance.gridArray;

    }

    public void ResponseFromServer( BoardUpdateModel data)
    {
        for (int i = 0; i < data.wolves.Length; i++)
        {
            if(data.wolves[i].alive)
                SetObjectPosition(GridGenerator.Instance.wolvesList[i].transform, data.wolves[i].x, data.wolves[i].y);
            else
            {
                GridGenerator.Instance.wolvesList[i].SetActive(false);
            }
        }
        for (int i = 0; i < data.rabbits.Length; i++)
        {
            if(data.rabbits[i].alive)
                SetObjectPosition(GridGenerator.Instance.rabbitsList[i].transform, data.rabbits[i].x, data.rabbits[i].y);
            else
            {
                GridGenerator.Instance.rabbitsList[i].SetActive(false);
            }
        }
    }

    public Vector2 SetObjectPosition(Transform gameObject, int x, int y)
    {
        x = (x + hight) % hight;
        y = (y + width) % width;

        GridGenerator.Instance.SetColorAtPosition(x,y);
        var pos = gridArray[x, y].transform.position;
        pos.y += 1f;
        gameObject.DOMove(pos, 0.5f);

        return new Vector2(x,y);
    }
}
