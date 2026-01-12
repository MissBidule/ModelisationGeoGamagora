using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using System.Globalization;
using Unity.VisualScripting;

public class offReader : MonoBehaviour
{
    [SerializeField]
    public int iteration = 1;
    public string offFileName;

    public class Shape
    {

        public int verticesNb { get; set; }
        public int facesNb { get; set; }

        public List<Vector3> vertices { get; set; }
        public List<int> triangles { get; set; }
        public List<Vector3> normals { get; set; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (iteration < 1) iteration = 1;
        Shape newShape = new Shape();
        newShape.vertices = new List<Vector3>();
        newShape.triangles = new List<int>();
        newShape.normals = new List<Vector3>();
        if (!ReadFile(newShape)) return;

        Mesh mesh = GetComponent<MeshFilter>().mesh;

        mesh.Clear();

        mesh.vertices = newShape.vertices.ToArray();
        mesh.triangles = newShape.triangles.ToArray();
        mesh.normals = newShape.normals.ToArray();
    }

    bool ReadFile(Shape newShape)
    {
        string assetsPath = Application.dataPath;
        string offFileFullPath = assetsPath + "/Shapes/" + offFileName + ".off";

        if (!File.Exists(offFileFullPath)) return false;

        try
        {
            var sr = new StreamReader(offFileFullPath);
            //OFF line
            string line = sr.ReadLine();
            //numbers data
            line = sr.ReadLine();
            string[] numbers = line.Split(' ');
            newShape.verticesNb = int.Parse(numbers[0]);
            newShape.facesNb = int.Parse(numbers[1]);
            //vertices
            Vector3 gravity = new Vector3();
            List<int> normalsNb = new List<int>();
            for (int i = 0; i < newShape.verticesNb; i++)
            {
                line = sr.ReadLine();
                numbers = line.Split(' ');
                Vector3 newVertice = new Vector3();
                
                newVertice.x = float.Parse(numbers[0], CultureInfo.InvariantCulture);
                newVertice.y = float.Parse(numbers[1], CultureInfo.InvariantCulture);
                newVertice.z = float.Parse(numbers[2], CultureInfo.InvariantCulture);

                newShape.vertices.Add(newVertice);
                gravity += newVertice;

                newShape.normals.Add(Vector3.zero);
                normalsNb.Add(0);
            }
            //triangle faces
            for (int i = 0; i < newShape.facesNb; i++)
            {
                line = sr.ReadLine();
                numbers = line.Split(' ');
                if (int.Parse(numbers[0]) != 3) continue;
                int pointA = int.Parse(numbers[1]);
                int pointB = int.Parse(numbers[2]);
                int pointC = int.Parse(numbers[3]);
                newShape.triangles.Add(pointA);
                newShape.triangles.Add(pointB);
                newShape.triangles.Add(pointC);

                // Vector3 A = newShape.vertices[pointA];
                // Vector3 B = newShape.vertices[pointB];
                // Vector3 C = newShape.vertices[pointC];

                // Vector3 normal = Vector3.Cross(B - A, C - A);
                
                // newShape.normals[pointA] += normal;
                // normalsNb[pointA] += 1;
                // newShape.normals[pointB] += normal;
                // normalsNb[pointB] += 1;
                // newShape.normals[pointC] += normal;
                // normalsNb[pointC] += 1;
            }

            //center shape
            gravity /= newShape.verticesNb;
            float max = 0;
            for (int i = 0; i < newShape.verticesNb; i++)
            {
                newShape.vertices[i] -= gravity;
                if (Mathf.Abs(newShape.vertices[i].x) > max) max = Mathf.Abs(newShape.vertices[i].x);
                if (Mathf.Abs(newShape.vertices[i].y) > max) max = Mathf.Abs(newShape.vertices[i].y);
                if (Mathf.Abs(newShape.vertices[i].z) > max) max = Mathf.Abs(newShape.vertices[i].z);
            }

            //normalize shape
            for (int i = 0; i < newShape.verticesNb; i++)
            {
                if (max != 0) newShape.vertices[i] /= max;
            }

            loopAlgo(newShape);
            
            //normalize normals
            // for (int i = 0; i < newShape.verticesNb; i++)
            // {
            //     if (normalsNb[i] != 0) newShape.normals[i] /= normalsNb[i];
            //     //uncomment if needed
            //     // Debug.DrawRay(newShape.vertices[i], newShape.normals[i], Color.white, Mathf.Infinity);
            // }
        }
        catch (System.Exception e)
        {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
            return false;
        }

        return true;
    }
    
    void loopAlgo(Shape newShape)
    {
        List<Vector3> currentVertices = newShape.vertices;
        List<int> currentTriangles = newShape.triangles;
        
        List<int> loopTriangles = new List<int>();
        List<Vector3> loopVertices = new List<Vector3>();

        for (int j = 0; j < iteration; j++) {
            loopTriangles.Clear();
            loopVertices = new List<Vector3>(currentVertices);
            for (int i = 0; i < currentTriangles.Count / 3; i++)
            {
                loopVertices.Add(splitVertex(currentVertices[currentTriangles[i*3+0]], currentVertices[currentTriangles[i*3+1]]));
                loopVertices.Add(splitVertex(currentVertices[currentTriangles[i*3+1]], currentVertices[currentTriangles[i*3+2]]));
                loopVertices.Add(splitVertex(currentVertices[currentTriangles[i*3+2]], currentVertices[currentTriangles[i*3+0]]));
            
                loopTriangles.Add(currentTriangles[i*3+0]);
                loopTriangles.Add(loopVertices.Count-3);
                loopTriangles.Add(loopVertices.Count-1);
                
                loopTriangles.Add(currentTriangles[i*3+1]);
                loopTriangles.Add(loopVertices.Count-2);
                loopTriangles.Add(loopVertices.Count-3);

                loopTriangles.Add(currentTriangles[i*3+2]);
                loopTriangles.Add(loopVertices.Count-1);
                loopTriangles.Add(loopVertices.Count-2);
                
                loopTriangles.Add(loopVertices.Count-3);
                loopTriangles.Add(loopVertices.Count-2);
                loopTriangles.Add(loopVertices.Count-1);
            }

            currentTriangles = new List<int>(loopTriangles);
            currentVertices = new List<Vector3>(loopVertices);
            
            loopVertex(currentTriangles, currentVertices, loopVertices);

            currentVertices.Clear();
            currentVertices = new List<Vector3>(loopVertices);
            currentTriangles.Clear();
            currentTriangles = new List<int>(loopTriangles);
        }

        List<Vector3> loopNormals = new List<Vector3>(loopVertices.Count);
        List<int> normalsNb = new List<int>(loopVertices.Count);

        for (int i = 0; i < loopVertices.Count; i++)
        {
            loopNormals.Add(new Vector3());
            normalsNb.Add(0);
        }

        for (int i = 0; i < loopTriangles.Count/3; i++) {
            int pointA = loopTriangles[i*3+0];
            int pointB = loopTriangles[i*3+1];
            int pointC = loopTriangles[i*3+2];

            Vector3 A = loopVertices[pointA];
            Vector3 B = loopVertices[pointB];
            Vector3 C = loopVertices[pointC];

            Vector3 normal = Vector3.Cross(B - A, C - A);

            loopNormals[pointA] += normal;
            normalsNb[pointA] += 1;
            loopNormals[pointB] += normal;
            normalsNb[pointB] += 1;
            loopNormals[pointC] += normal;
            normalsNb[pointC] += 1;
        }

        //normalize normals
        for (int i = 0; i < loopVertices.Count; i++)
        {
            if (normalsNb[i] != 0) loopNormals[i] /= normalsNb[i];
        }

        newShape.verticesNb = loopVertices.Count;
        newShape.facesNb = loopTriangles.Count/3;
        newShape.vertices = new List<Vector3>(loopVertices);
        newShape.normals = new List<Vector3>(loopNormals);
        newShape.triangles = new List<int>(loopTriangles);
    }

    Vector3 splitVertex(Vector3 v1, Vector3 v2)
    {
        return (v1 + v2)/2.0f;
    }

    void loopVertex(List<int> currentTriangles, List<Vector3> currentVertices, List<Vector3> loopVertices)
    {
        //fuse our vertex first
        List<int> replac = new List<int>();
        for (int i = 0; i < currentVertices.Count; i++) replac.Add(i);

        for (int i = 0; i < currentVertices.Count - 1; i ++)
        {
            for (int j = i + 1; j < currentVertices.Count; j++)
            {
                if (currentVertices[i] == currentVertices[j] && replac[j] == j)
                {
                    replac[j] = i;
                }
            }
        }

        List<HashSet<int>> weight = new List<HashSet<int>>();
        for (int i = 0; i < currentVertices.Count; i++) weight.Add(new HashSet<int>());
        for (int i = 0; i < currentTriangles.Count/3; i++)
        {
            //A
            weight[replac[currentTriangles[i * 3]]].Add(replac[currentTriangles[i * 3 + 1]]);
            weight[replac[currentTriangles[i * 3]]].Add(replac[currentTriangles[i * 3 + 2]]);

            //B
            weight[replac[currentTriangles[i * 3 + 1]]].Add(replac[currentTriangles[i * 3 + 0]]);
            weight[replac[currentTriangles[i * 3 + 1]]].Add(replac[currentTriangles[i * 3 + 2]]);

            //C
            weight[replac[currentTriangles[i * 3 + 2]]].Add(replac[currentTriangles[i * 3 + 1]]);
            weight[replac[currentTriangles[i * 3 + 2]]].Add(replac[currentTriangles[i * 3 + 0]]);
        }
        //new pos
        for (int i = 0; i < currentVertices.Count; i++)
        {
            float a = Mathf.Pow(3 + 2 * Mathf.Cos(2 * Mathf.PI/weight[replac[i]].Count), 2)/32.0f - (1.0f/4.0f);
            float b = (1 - a) / weight[replac[i]].Count;
            loopVertices[i] = a * currentVertices[i];
            foreach (int point in weight[replac[i]])
            {
                loopVertices[i] += b * currentVertices[point];
            }
        }
    }
}