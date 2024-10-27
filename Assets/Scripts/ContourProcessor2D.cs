using System;
using System.Drawing;
using UnityEngine;

public class ContourProcessor2D : MonoBehaviour
{
    [SerializeField]
    private PolygonCollider2D polygonCollider2D;

    private void Start()
    {
        var tcpServer = GetComponent<TCPServer>();
        if (tcpServer != null)
        {
            tcpServer.OnDataReceived += ProcessContourData;
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

        polygonCollider2D.pathCount = 0;
        foreach(var contour in contourData.contours)
        {
            polygonCollider2D.pathCount++;
            polygonCollider2D.SetPath(polygonCollider2D.pathCount - 1, toVector2(contour.vertices));
        }
    }

    private Vector2[] toVector2(Vertex[] vertices)
    {
        var vectorList = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vectorList[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        return vectorList;
    }
}


