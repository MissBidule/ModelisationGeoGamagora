using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using System.Globalization;
using Unity.VisualScripting;

public class offReader : MonoBehaviour
{
    public string offFileName;

    public bool autoSaved = false;
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

        if (autoSaved) WriteFile(newShape);
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

                Vector3 A = newShape.vertices[pointA];
                Vector3 B = newShape.vertices[pointB];
                Vector3 C = newShape.vertices[pointC];

                Vector3 normal = Vector3.Cross(B - A, C - A);
                
                newShape.normals[pointA] += normal;
                normalsNb[pointA] += 1;
                newShape.normals[pointB] += normal;
                normalsNb[pointB] += 1;
                newShape.normals[pointC] += normal;
                normalsNb[pointC] += 1;
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
            
            //normalize normals
            for (int i = 0; i < newShape.verticesNb; i++)
            {
                if (normalsNb[i] != 0) newShape.normals[i] /= normalsNb[i];
                //uncomment if needed
                // Debug.DrawRay(newShape.vertices[i], newShape.normals[i], Color.white, Mathf.Infinity);
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
            return false;
        }

        return true;
    }
    
    public void WriteFile(Shape shapeToWrite)
    {
        string assetsPath = Application.dataPath;
        string offFileFullPath = assetsPath + "/ShapesOutput/" + ".off";

        //metadata
        string fileContent = "OFF\n";
        fileContent += shapeToWrite.verticesNb.ToString() + " ";
        fileContent += shapeToWrite.facesNb.ToString() + " ";
        fileContent += "0\n";
        //vertices
        for (int i = 0; i < shapeToWrite.verticesNb; i++)
        {
            fileContent += shapeToWrite.vertices[i].x.ToString(CultureInfo.InvariantCulture) + " ";
            fileContent += shapeToWrite.vertices[i].y.ToString(CultureInfo.InvariantCulture) + " ";
            fileContent += shapeToWrite.vertices[i].z.ToString(CultureInfo.InvariantCulture);
            fileContent += "\n";
        }
        //triangle faces
        for (int i = 0; i < shapeToWrite.facesNb; i++)
        {
            fileContent += "3 ";
            fileContent += shapeToWrite.triangles[i * 3 + 0].ToString() + " ";
            fileContent += shapeToWrite.triangles[i * 3 + 1].ToString() + " ";
            fileContent += shapeToWrite.triangles[i * 3 + 2].ToString();
            fileContent += "\n";
        }

        File.WriteAllText(offFileFullPath, fileContent);
    }
}