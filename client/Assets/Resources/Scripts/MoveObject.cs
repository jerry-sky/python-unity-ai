using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveObject : MonoBehaviour
{
    private float timer = 0.0f;

    private int maxX;
    private int maxY;

    private Vector2 currentPos;
    void Start()
    {
        maxX = GridGenerator.Instance.width;
        maxY = GridGenerator.Instance.hight;

        currentPos.x = Random.Range(0, maxX);
        currentPos.y = Random.Range(0, maxY);

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1f)
        {
            GridGenerator.Instance.RestartColorAtPosition((int)currentPos.x,(int)currentPos.y);
        }
    }

    private void LateUpdate()
    {
        if (timer >= 0.1f)
        {
            currentPos = GameManager.gm.SetObjectPosition(transform,Random.Range((int)currentPos.x-1,(int)currentPos.x+2),
                Random.Range((int) currentPos.y-1,(int)currentPos.y+2));
            timer = 0.0f;
        }


    }
}
