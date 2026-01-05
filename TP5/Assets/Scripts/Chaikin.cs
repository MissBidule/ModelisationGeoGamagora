using System.Collections.Generic;
using UnityEngine;

public class Chaikin : MonoBehaviour
{
    [SerializeField]
    public List<Vector3> ChaikinShape = new List<Vector3>();
    public int iteration = 1;
    
    private List<Vector3> ResultShape = new List<Vector3>();
    private List<Vector3> tempShape = new List<Vector3>();
    private void OnDrawGizmos()
    {
        if (ChaikinShape.Count < 3) return;

        // MainShape
        Gizmos.color = Color.yellow;

        // Draw main shape
        int vertexNb = ChaikinShape.Count;
        for (int i = 0; i < vertexNb; i++) {
            Gizmos.DrawLine(ChaikinShape[i%vertexNb], ChaikinShape[(i+1)%vertexNb]);
        }

        if (iteration < 1) return;

        ResultShape = new List<Vector3>(ChaikinShape);

        int resultVertexNb = ResultShape.Count;
        for (int i = 0; i < iteration; i++)
        {
            resultVertexNb = ResultShape.Count;
            for (int j = 0; j < resultVertexNb; j++) {
                tempShape.Add(0.75f * ResultShape[j%resultVertexNb] + 0.25f * ResultShape[(j+1)%resultVertexNb]);
                tempShape.Add(0.25f * ResultShape[j%resultVertexNb] + 0.75f * ResultShape[(j+1)%resultVertexNb]);
            }

            ResultShape = new List<Vector3>(tempShape);
            tempShape.Clear();
        }

        // MainShape
        Gizmos.color = Color.cyan;

        // Draw main shape
        resultVertexNb = ResultShape.Count;
        for (int i = 0; i < resultVertexNb; i++) {
            Gizmos.DrawLine(ResultShape[i%resultVertexNb], ResultShape[(i+1)%resultVertexNb]);
        }
    }
}
