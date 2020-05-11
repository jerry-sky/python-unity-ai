using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleTargetCamera : MonoBehaviour
{
    public GridGenerator GridGenerator;
    public List<Transform> targets;
    public Vector3 offset = new Vector3(0,0,-30f);

    private void Start()
    {
        for (int i = 0; i < GridGenerator.hight; i++)
        {
            for (int j = 0; j < GridGenerator.width; j++)
            {
                targets.Add(GridGenerator.GetGrid(i,j).transform);
            }
        }
    }

    private void LateUpdate()
    {
        if(targets.Count == 0)
            return;
        Vector3 centerPoint = GetCentrerPoint();
        Vector3 newPosition = centerPoint +offset;
        Camera.main.transform.position = newPosition;
    }

    public Vector3 GetCentrerPoint()
    {
        var bounds = new Bounds(targets[0].position,Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
}
