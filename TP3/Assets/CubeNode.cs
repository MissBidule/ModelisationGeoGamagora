using System.Collections.Generic;
using UnityEngine;

public class CubeNode
{
    private Vector3 position = new Vector3(0,0,0);
    private float size = 1;
    public int depth = 1;

    public void setSize(float _size)
    {
        size = _size;
    }

    public void setPosition(Vector3 _position)
    {
        position = _position;
    }

    public List<CubeNode> MakeChildNodes()
    {
        List<CubeNode> childNodes = new List<CubeNode>();
        for (int x = -1; x <= 1; x += 2)
        {
            for (int y = -1; y <= 1; y += 2)
            {
                for (int z = -1; z <= 1; z += 2)
                {
                    CubeNode newNode = new CubeNode();
                    newNode.size = size / 2;
                    newNode.depth = depth + 1;
                    Vector3 newPosition = position;
                    newPosition.x += x * size / 4;
                    newPosition.y += y * size / 4;
                    newPosition.z += z * size / 4;
                    newNode.position = newPosition;
                    childNodes.Add(newNode);
                }
            }
        }

        return childNodes;
    }

    public Vector3 cubeVertex(int id)
    {
        if (id < 0 || id >= 8) return position;
        int x = id % 2 == 0 ? -1 : 1;
        int y = id / 4 < 1  ? -1 : 1;
        int z = (id / 2) % 2 == 0  ? -1 : 1;
        Vector3 vertexPosition = position;
        vertexPosition.x += x * (size / 2.0f);
        vertexPosition.y += y * (size / 2.0f);
        vertexPosition.z += z * (size / 2.0f);

        return vertexPosition;
    }

    public void drawNode(bool visibility)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube); 
        cube.name = "cubeDepth" + depth;
        cube.GetComponent<Renderer>().material.color = Color.white / depth; 
        cube.GetComponent<Renderer>().enabled = visibility;
        cube.transform.position = position;
        cube.transform.localScale = new Vector3(size, size, size);
        cube.AddComponent<changeVisibility>();
        cube.GetComponent<changeVisibility>().initVisibility = visibility;
    }

    public Vector3 center()
    {
        return position;
    }

    public float getSize()
    {
        return size;
    }
}
