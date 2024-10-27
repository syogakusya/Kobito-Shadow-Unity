using System;
using UnityEngine;
using Unity.AI.Navigation;

public class ContourProcessor3D : MonoBehaviour
{
    [SerializeField]
    private MeshCollider meshCollider;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private NavMeshSurface surface;
    private Mesh mesh;
    public float zIndex = 10;

    private void Start()
    {
        var tcpServer = GetComponent<TCPServer>();
        if (tcpServer != null)
        {
            tcpServer.OnDataReceived += ProcessContourData;
        }

        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    private void Update()
    {
        if (mesh.vertexCount > 3 && mesh.triangles.Length > 3)
        {
            surface.BuildNavMesh();
        }
    }

    private void ProcessContourData(string jsonData)
    {
        try
        {
            Debug.Log($"Received JSON data: {jsonData}");
            var contourData = JsonUtility.FromJson<ContourData>(jsonData);
            if (contourData == null || contourData.contours == null)
            {
                Debug.LogError("Deserialized contourData or contours is null");
                return;
            }

            Debug.Log($"Deserialized {contourData.contours.Length} contours");
            UpdateContours(contourData);
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON parsing error: {e.Message}\nStack trace: {e.StackTrace}");
        }
    }

    private void UpdateContours(ContourData contourData)
    {
        var vertices = new System.Collections.Generic.List<Vector3>();
        var triangles = new System.Collections.Generic.List<int>();
        int vertexOffset = 0;

        foreach (var contour in contourData.contours)
        {
            Vector3[] contourVertices = GetVerticesFromContour(contour);
            WeldVertices(ref contourVertices);
            int[] contourTriangles = GenerateTrianglesForContour(contourVertices.Length, vertexOffset);

            vertices.AddRange(contourVertices);
            triangles.AddRange(contourTriangles);

            vertexOffset += contourVertices.Length;
        }

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        // 頂点数と三角形の数をチェック
        Debug.Log($"Vertex count: {mesh.vertexCount}, Triangle count: {mesh.triangles.Length / 3}");

        if (mesh.vertexCount > 3 && mesh.triangles.Length > 3)
        {
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            mesh.Optimize();
            meshCollider.sharedMesh = mesh;
        }
    }

    private Vector3[] GetVerticesFromContour(Contour contour)
    {
        var vertexList = new System.Collections.Generic.List<Vector3>();

        // 前面と背面の頂点を追加
        foreach (var vertex in contour.vertices)
        {
            vertexList.Add(new Vector3(vertex.x, vertex.y, 0));      // 前面
        }

        foreach (var vertex in contour.vertices)
        {
            vertexList.Add(new Vector3(vertex.x, vertex.y, zIndex)); // 背面
        }

        //foreach(var vertex in contour.vertices){
        //    vertexList.Add(new Vector3(vertex.x, vertex.y, 0));
        //    vertexList.Add(new Vector3(vertex.x, vertex.y, zIndex));
        //}

        return vertexList.ToArray();
    }

    private int[] GenerateTrianglesForContour(int vertexCount, int vertexOffset)
    {
        var triangles = new System.Collections.Generic.List<int>();
        int frontOffset = vertexOffset;
        int backOffset = vertexOffset + vertexCount / 2;

        // 前面の三角形
        for (int i = 0; i < vertexCount / 2 - 2; i++)
        {
            triangles.Add(frontOffset);
            triangles.Add(frontOffset + i + 1);
            triangles.Add(frontOffset + i + 2);
        }

        // 背面の三角形（反時計回り）
        for (int i = 0; i < vertexCount / 2 - 2; i++)
        {
            triangles.Add(backOffset);
            triangles.Add(backOffset + i + 2);
            triangles.Add(backOffset + i + 1);
        }

        // 側面の三角形
        for (int i = 0; i < vertexCount / 2; i++)
        {
            int next = (i + 1) % (vertexCount / 2);

            // 一つ目の三角形
            triangles.Add(frontOffset + i);
            triangles.Add(frontOffset + next);
            triangles.Add(backOffset + i);

            // 二つ目の三角形
            triangles.Add(backOffset + i);
            triangles.Add(frontOffset + next);
            triangles.Add(backOffset + next);
        }

        return triangles.ToArray();
    }

    private void WeldVertices(ref Vector3[] vertices, float threshold = 0.01f)
    {
        System.Collections.Generic.List<Vector3> newVertices = new System.Collections.Generic.List<Vector3>();

        for (int i = 0; i < vertices.Length; i++)
        {
            bool isDuplicate = false;
            foreach (var v in newVertices)
            {
                if (Vector3.Distance(vertices[i], v) < threshold)
                {
                    isDuplicate = true;
                    break;
                }
            }
            if (!isDuplicate)
            {
                newVertices.Add(vertices[i]);
            }
        }
        vertices = newVertices.ToArray();
    }

}

[Serializable]
public class ContourData
{
    public Contour[] contours;
}

[Serializable]
public class Contour
{
    public Vertex[] vertices;
}

[Serializable]
public class Vertex
{
    public float x;
    public float y;
}
