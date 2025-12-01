using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VertexClustering : MonoBehaviour
{
    [SerializeField]
    public float minCubeSize = 1;
    public GameObject objectToSimplify;

    private List<Vector3> newVertex = new List<Vector3>();
    private List<Vector3> oldVertex = new List<Vector3>();
    private List<int> weights = new List<int>();
    private List<int> vertexToReplace = new List<int>();
    private List<int> newTriangles;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (minCubeSize == 0) return;

        Mesh mesh = objectToSimplify.GetComponent<MeshFilter>().mesh;
        newTriangles = mesh.GetTriangles(0).ToList();
        newTriangles.AddRange(mesh.GetTriangles(1).ToList());
        int cptX = (int)Mathf.Ceil(objectToSimplify.GetComponent<MeshCollider>().bounds.extents.x * 2/minCubeSize);
        int cptY = (int)Mathf.Ceil(objectToSimplify.GetComponent<MeshCollider>().bounds.extents.y * 2/minCubeSize);
        int cptZ = (int)Mathf.Ceil(objectToSimplify.GetComponent<MeshCollider>().bounds.extents.z * 2/minCubeSize);
        for (int k = 0; k < cptZ * 2/minCubeSize; k++)
        {
            for (int j = 0; j < cptY * 2/minCubeSize; j++)
            {
                for (int i = 0; i < cptX; i++)
                {
                    newVertex.Add(Vector3.zero);
                    weights.Add(0);
                }
            }
        }
        mesh.GetVertices(oldVertex);

        foreach (Vector3 vertex in oldVertex)
        {
            Vector3 indexes = (vertex - objectToSimplify.GetComponent<MeshCollider>().bounds.min) / minCubeSize;
            int newIndex = (int)indexes.x + (int)indexes.y * cptX + (int)indexes.z * cptX * cptY;
            newVertex[newIndex] += vertex;
            weights[newIndex] += 1;
            vertexToReplace.Add(newIndex);
        }

        for (int i = 0; i < newTriangles.Count; i++)
        {
            newTriangles[i] = vertexToReplace[newTriangles[i]];
        }

        for (int i = 0; i < newVertex.Count; i++)
        {
            if (weights[i] == 0) continue;
            newVertex[i] = newVertex[i] / weights[i];
        }

        mesh.Clear();

        mesh.SetVertices(newVertex.ToArray());
        mesh.SetTriangles(newTriangles.ToArray(), 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
