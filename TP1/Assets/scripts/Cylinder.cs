using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.UIElements;

public class Cylinder : MonoBehaviour
{
    public int posX = 10;
    public int posY = 10;
    public int posZ = 10;
    public int radius = 3;
    public int height = 6;
    public int meridians = 3;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (meridians < 3) return;
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        //code here

        DrawCylinder();

        //up to there

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    void DrawCylinder()
    {

        vertices.Add(new Vector3(posX, posY, posZ));
        vertices.Add(new Vector3(posX, height + posY, posZ));
        for (int i = 0; i <= meridians; i++)
        {
            float theta = i * (2 * Mathf.PI) / meridians;
            float x = radius * Mathf.Sin(theta);
            float y1 = 0;
            float y2 = height;
            float z = -radius * Mathf.Cos(theta);

            vertices.Add(new Vector3(x + posX, y1 + posY, z + posZ));
            vertices.Add(new Vector3(x + posX, y2 + posY, z + posZ));
        }

        int point0 = 0;
        int point1 = 1;

        for (int i = 0; i < meridians; i++)
        {
            int point2 = (1 + i) * 2;
            int point3 = (1 + i) * 2 + 1;
            int point4 = (2 + i) * 2;
            int point5 = (2 + i) * 2 + 1;

            //bottom
            triangles.Add(point0);
            triangles.Add(point2);
            triangles.Add(point4);

            //sides
            triangles.Add(point2);
            triangles.Add(point3);
            triangles.Add(point4);

            triangles.Add(point3);
            triangles.Add(point5);
            triangles.Add(point4);

            //top
            triangles.Add(point1);
            triangles.Add(point5);
            triangles.Add(point3);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
