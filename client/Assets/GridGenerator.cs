using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width = 3;
    public int hight = 3;
    public GameObject Node;

    [HideInInspector]
    public Vector3 startPos;
    [HideInInspector]
    public Vector3 currentPos;

    public MultipleTargetCamera MultipleTargetCamera;
    public GameObject[,] gridArray;
    public static GridGenerator Instance;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        gridArray = new GameObject[hight,width];
        startPos = transform.position;
        currentPos = startPos;

        for (int i = 0; i < hight; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var x = Instantiate(Node, currentPos, Quaternion.identity,transform);
                gridArray[i, j] = x;
                currentPos += Vector3.right *1.5f;
            }

            currentPos.x = startPos.x;
            currentPos += Vector3.forward *1.5f;

        }
    }

    public GameObject GetGrid(int x,int y)
    {
        return gridArray[x, y];
    }

    public void SetObjectPosition(Transform gameObject, int x, int y)
    {
        var pos = gridArray[x, y].transform.position;
        pos.y += 1f;
        gameObject.position = pos;



        // Update is called once per frame
    }
}
