using System.Collections.Generic;
using UnityEngine;

public class enumspatiale : MonoBehaviour
{
    [SerializeField]
    int maxDepth = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CubeNode firstNode = new CubeNode();
        createSphereVoxel(firstNode, maxDepth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createSphereVoxel(CubeNode currentNode, int _maxDepth)
    {
        if (currentNode.depth >= _maxDepth) {
            currentNode.drawNode();
            return;
        }

        int outOfCircle = 0;
        for (int i = 0; i < 8; i++)
        {
            Vector3 cubeVertex = currentNode.cubeVertex(i);
            double distance = distanceFromCenter(cubeVertex);
            if (distance > 0.5f)
            {
                outOfCircle += 1;
            }
        }
        if (outOfCircle == 8)
        {
            double distance = distanceFromCenter(currentNode.center());
            if (distance <= 0.5f)
            {
                outOfCircle -= 1;
            }
        }
        if (outOfCircle == 0) { 
            currentNode.drawNode();
        }
        else if (outOfCircle < 8)
        {
            List<CubeNode> cubeNodes = currentNode.MakeChildNodes();
                foreach (CubeNode childNode in cubeNodes)
                {
                    createSphereVoxel(childNode, _maxDepth);
                }
        }
    }

    double distanceFromCenter(Vector3 vertex)
    {
        double distance = Mathf.Sqrt(
        Mathf.Pow(vertex.x, 2f) +
        Mathf.Pow(vertex.y, 2f) +
        Mathf.Pow(vertex.z, 2f));

        return distance;
    }
}
