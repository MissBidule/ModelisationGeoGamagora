using System;
using System.Collections.Generic;
using UnityEngine;

public class enumspatiale : MonoBehaviour
{
    public enum State {union, intersection};

    [SerializeField]
    public int maxDepth = 1;
    public List<Sphere> spheres = new List<Sphere> {
        new Sphere(new Vector3(-0.5f, 0, 0), 1),
        new Sphere(new Vector3(0.5f, 0, 0), 0.5f)};
    public int size = 1;
    public Vector3 position = Vector3.zero;
    public bool adaptative = true;
    public State state;
    public bool visibility = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CubeNode firstNode = new CubeNode();
        firstNode.setSize(size);
        firstNode.setPosition(position);
        createSphereVoxel(firstNode, maxDepth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createSphereVoxel(CubeNode currentNode, int _maxDepth)
    {
        if (currentNode.depth >= _maxDepth) {
            currentNode.drawNode(visibility);
            return;
        }

        int outOfCircle = 0;
        for (int i = 0; i < 8; i++)
        {
            int outOfCircleVertex = 0;
            for (int k = 0; k < spheres.Count; k++) {
                Vector3 cubeVertex = currentNode.cubeVertex(i);
                double distance = distanceFromCenter(cubeVertex, spheres[k].center);
                if (distance > spheres[k].size/2)
                {
                    outOfCircleVertex += 1;
                }
            }
            if (state == State.intersection) outOfCircleVertex = outOfCircleVertex % spheres.Count == 0 ? outOfCircleVertex : spheres.Count;
            outOfCircle += outOfCircleVertex;
        }
        if (outOfCircle == 8 * spheres.Count)
        {
            int inCircleVertex = 0;
            for (int k = 0; k < spheres.Count; k++) {
                double distanceNodeSphere = distanceFromCenter(currentNode.center(), spheres[k].center);
                double maxDistance = distanceFromCenter(currentNode.center(), currentNode.cubeVertex(0)) + spheres[k].size / 2;
                if (distanceNodeSphere <= maxDistance)
                {
                    inCircleVertex += 1;
                }
            }
            if ((state == State.union && inCircleVertex >= 1) || (state == State.intersection && inCircleVertex == spheres.Count))
            {
                outOfCircle = 7;
            }
        }
        if (adaptative && outOfCircle == 0) { 
            currentNode.drawNode(visibility);
        }
        else if (outOfCircle < 8 * spheres.Count)
        {
            List<CubeNode> cubeNodes = currentNode.MakeChildNodes();
                foreach (CubeNode childNode in cubeNodes)
                {
                    createSphereVoxel(childNode, _maxDepth);
                }
        }
    }

    double distanceFromCenter(Vector3 vertex, Vector3 center)
    {
        double distance = Mathf.Sqrt(
        Mathf.Pow(vertex.x - center.x, 2f) +
        Mathf.Pow(vertex.y - center.y, 2f) +
        Mathf.Pow(vertex.z - center.z, 2f));

        return distance;
    }
}

[Serializable]
public class Sphere
{
    public Vector3 center = new Vector3(0,0,0);
    public float size = 1;

    public Sphere(Vector3 vector3, float v)
    {
        center = vector3;
        size = v;
    }
}