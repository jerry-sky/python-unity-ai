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

    public List<GameObject> wolfes;
    public List<GameObject> rabbits;

    private void Start()
    {
        gm = this;
        hight = GridGenerator.Instance.hight;
        width = GridGenerator.Instance.width;
        gridArray = GridGenerator.Instance.gridArray;

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
