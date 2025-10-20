using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.UIElements;


public class Circle : MonoBehaviour
{
    public int posX = 10;
    public int posY = 10;
    public int posZ = 10;
    public int radius = 3;
    public int meridians = 3;
    public int parallels = 2;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (meridians < 3 || parallels < 2) return;
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        //code here

        DrawCircle();

        //up to there

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    void DrawCircle()
    {

        vertices.Add(new Vector3(posX, - radius + posY, posZ));
        vertices.Add(new Vector3(posX, radius + posY, posZ));
        for (int j = 0; j <= meridians; j++)
        {
            float theta = j * (2 * Mathf.PI) / meridians;

            for (int i = 1; i < parallels; i++)
            {
                float phi = Mathf.PI + (Mathf.PI / parallels * i);

                float x = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
                float y = radius * Mathf.Cos(phi);
                float z = -radius * Mathf.Sin(phi) * Mathf.Cos(theta);

                vertices.Add(new Vector3(x + posX, y + posY, z + posZ));
            }
        }

        int point0 = 0;
        int point1 = 1;

        for (int j = 0; j < meridians; j++)
        {
            //bottom
            triangles.Add(point0);
            triangles.Add(2 + j * (parallels - 1));
            triangles.Add(2 + (j + 1) * (parallels - 1));

            //top
            triangles.Add(point1);
            triangles.Add(parallels + (j + 1) * (parallels - 1));
            triangles.Add(parallels + j * (parallels - 1));

            for (int i = 1; i < parallels - 1; i++)
            {
                int point2 = 1 + i + j * (parallels - 1);
                int point3 = 2 + i + j * (parallels - 1);
                int point4 = 2 + i + (j + 1) * (parallels - 1);
                int point5 = 1 + i + (j + 1) * (parallels - 1);

                triangles.Add(point2);
                triangles.Add(point3);
                triangles.Add(point4);

                triangles.Add(point4);
                triangles.Add(point5);
                triangles.Add(point2);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
