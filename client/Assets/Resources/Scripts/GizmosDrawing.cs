using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosDrawing : MonoBehaviour
{
    public GridGenerator grid;
    private void OnDrawGizmos()
    {
        Vector3 currentPos = grid.currentPos;
        Vector3 startPos = grid.startPos;

        startPos = transform.position;
        currentPos = grid.startPos;

        for (int i = 0; i < grid.height; i++)
        {
            for (int j = 0; j < grid.width; j++)
            {
                Gizmos.DrawCube(currentPos,new Vector3(1,0.2f,1));
                currentPos += Vector3.right *1.5f;
            }

            currentPos.x = startPos.x;
            currentPos += Vector3.forward *1.5f;

        }
    }
}
