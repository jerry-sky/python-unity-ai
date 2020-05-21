using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridGenerator : MonoBehaviour
{
    public int width = 3;
    public int hight = 3;
    public GameObject Node;

    [HideInInspector]
    public Vector3 startPos;
    [HideInInspector]
    public Vector3 currentPos;

    public GameObject[,] gridArray;
    private Renderer[,] GridArrayMaterials;

    public static GridGenerator Instance;

    // Start is called before the first frame update
    void Awake()
    {
        var cameraPos = transform.position;
        cameraPos.z -= 10;
        cameraPos.y += 5;
        Camera.main.transform.position = cameraPos;
        Instance = this;
        gridArray = new GameObject[hight,width];
        GridArrayMaterials = new Renderer[hight,width];
        startPos = transform.position;
        currentPos = startPos;

        for (int i = 0; i < hight; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var x = Instantiate(Node, currentPos, Quaternion.identity,transform);
                var pos =  x.transform.position;
                float noiseDensity =5.1f;
                pos.y += Mathf.PerlinNoise((float)i/hight *noiseDensity, (float)j/width * noiseDensity)*(float)width/10f;
                x.transform.position = pos;
                gridArray[i, j] = x;
                GridArrayMaterials[i,j] = x.GetComponent<Renderer>();
                currentPos += Vector3.right;
            }
            currentPos.x = startPos.x;
            currentPos += Vector3.forward;
        }
    }

    public GameObject GetGrid(int x,int y)
    {
        return gridArray[x, y];
    }


    public void SetColorAtPosition( int x, int y)
    {
        GridArrayMaterials[x, y].material.DOColor(Color.green, 0.5f);
    }
    public void RestartColorAtPosition( int x, int y)
    {
        GridArrayMaterials[x, y].material.DOColor(Color.gray, 10f);
    }


}
