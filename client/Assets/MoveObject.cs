using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    private float timer = 0.0f;

    private int maxX;
    private int maxY;
    // Start is called before the first frame update
    void Start()
    {
        maxX = GridGenerator.Instance.width;
        maxY = GridGenerator.Instance.hight;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            GridGenerator.Instance.SetObjectPosition(transform,Random.Range(0,maxX),Random.Range(0,maxY));
            timer = 0.0f;
        }
}
}
