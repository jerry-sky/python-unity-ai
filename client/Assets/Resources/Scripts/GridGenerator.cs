using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridGenerator : MonoBehaviour
{
    [HideInInspector]
    public int width, height;

    public List<GameObject> wolvesList;
    public List<GameObject> rabbitsList;

    public GameObject Node, Wolf,Rabbit;

    [HideInInspector]
    public Vector3 startPos;
    [HideInInspector]
    public Vector3 currentPos;

    public GameObject[,] gridArray;
    private Renderer[,] GridArrayMaterials;

    public static GridGenerator Instance;

    public void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    public void SettingsConfirmed(int width, int height, int wolvesCount, int rabbitsCount)
    {
        this.width = width;
        this.height = height;
        var cameraPos = transform.position;
        cameraPos.z -= 10;
        cameraPos.y += 5;
        Camera.main.transform.position = cameraPos;
        gridArray = new GameObject[height,width];
        GridArrayMaterials = new Renderer[height,width];
        startPos = transform.position;
        currentPos = startPos;

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                var x = Instantiate(Node, currentPos, Quaternion.identity,transform);
                var pos =  x.transform.position;
                float noiseDensity =5.1f;
                pos.y += Mathf.PerlinNoise((float)i/height *noiseDensity, (float)j/width * noiseDensity)*(float)width/10f;
                x.transform.position = pos;
                gridArray[i, j] = x;
                GridArrayMaterials[i,j] = x.GetComponent<Renderer>();
                currentPos += Vector3.right;
            }
            currentPos.x = startPos.x;
            currentPos += Vector3.forward;
        }

        for (int i = 0; i < wolvesCount; i++)
        {
            var wolf = GameObject.Instantiate(Wolf);
            wolvesList.Add(wolf);
        }
        for (int i = 0; i < rabbitsCount; i++)
        {
            var wolf = GameObject.Instantiate(Rabbit);
            rabbitsList.Add(Rabbit);
        }

        GameManager.gm.StartSimulation();
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
