using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacebleObject : MonoBehaviour
{

    public bool Placed { get; private set; }
    public Vector3Int Size { get; private set; }

    private Vector3[] Vertices;

    public void GetColliderVertexPositionsLocal()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        Vertices = new Vector3[4];
        float sizeX = collider.size.x;
        float sizeY = collider.size.y;
        float sizeZ = collider.size.z;
        Vertices[0] = collider.center + new Vector3(-sizeX, -sizeY, -sizeZ) * 0.5f;
        Vertices[1] = collider.center + new Vector3(sizeX, -sizeY, -sizeZ) * 0.5f;
        Vertices[2] = collider.center + new Vector3(sizeX, -sizeY, sizeZ) * 0.5f;
        Vertices[3] = collider.center + new Vector3(-sizeX, -sizeY, sizeZ) * 0.5f;
    }

    
    private void CalculateSizeInCells()
    {
        Vector3Int[] veritces = new Vector3Int[Vertices.Length];

        for (int i = 0; i < veritces.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(veritces[i]);
            //veritces[i] = BuildingSystem.Current.GridLayout.WorldToCell(worldPos);

        }

        Size = new Vector3Int(
            Mathf.Abs((veritces[0] - veritces[1]).x),
            Mathf.Abs((veritces[0] - veritces[3]).y),
            1);
    }

    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(Vertices[0]);
    }

    void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
    }


}
