using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rectangle : MonoBehaviour
{
    public int posX = 10;
    public int posY = 10;
    public int posZ = 10;
    public int width = 6;
    public int height = 3;
    public int lines = 1;
    public int columns = 1;

    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (columns == 0 || lines == 0) return;
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        //code here

        DrawRectangle();

        //up to there

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
    }

    void DrawRectangle()
    {
        for (int j = 0; j <= lines; j++)
        {
            for (int i = 0; i <= columns; i++)
            {
                vertices.Add(new Vector3(width / (float)columns * i + posX, -height / (float)lines * j + posY, posZ));
            }
        }

        for (int j = 0; j < lines; j++)
        {
            for (int i = 0; i < columns; i++)
            {
                int point0 = i + j * (columns + 1);
                int point1 = i + 1 + j * (columns + 1);
                int point2 = i + (j + 1) * (columns + 1);
                int point3 = i + 1 + (j + 1) * (columns + 1);
                triangles.Add(point0);
                triangles.Add(point1);
                triangles.Add(point2);

                triangles.Add(point2);
                triangles.Add(point1);
                triangles.Add(point3);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
