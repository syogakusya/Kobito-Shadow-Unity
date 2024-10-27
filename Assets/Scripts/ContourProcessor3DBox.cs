using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class ContourProcessor3D : MonoBehaviour
{
    [SerializeField]
    private GameObject navSurfaceBox; // NavMeshのコリジョン用のBox
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private NavMeshSurface surface;
    public float zIndex = 10;

    // 現在のBoxCollidersを管理するリスト
    private List<BoxCollider> boxColliders = new List<BoxCollider>();

    private void Start()
    {
        var tcpServer = GetComponent<TCPServer>();
        if (tcpServer != null)
        {
            tcpServer.OnDataReceived += ProcessContourData;
        }
    }

    private void Update()
    {
        surface.BuildNavMesh(); // 毎フレームNavMeshを再構築
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
            UpdateBoxColliders(contourData);
        }
        catch (Exception e)
        {
            Debug.LogError($"JSON parsing error: {e.Message}\nStack trace: {e.StackTrace}");
        }
    }

    private void UpdateBoxColliders(ContourData contourData)
    {
        int requiredColliders = 0;

        // 各輪郭の頂点ペアに対応するBoxColliderを生成または再利用
        foreach (var contour in contourData.contours)
        {
            for (int i = 0; i < contour.vertices.Length - 1; i++)
            {
                Vector3 start = new Vector3(contour.vertices[i].x, contour.vertices[i].y, 0);
                Vector3 end = new Vector3(contour.vertices[i + 1].x, contour.vertices[i + 1].y, zIndex);

                CreateOrReuseBoxColliderBetweenPoints(start, end, requiredColliders++);
            }

            // 輪郭を閉じるために、最後の頂点と最初の頂点を接続
            Vector3 first = new Vector3(contour.vertices[0].x, contour.vertices[0].y, 0);
            Vector3 last = new Vector3(contour.vertices[^1].x, contour.vertices[^1].y, zIndex);

            CreateOrReuseBoxColliderBetweenPoints(last, first, requiredColliders++);
        }

        // 余分なBoxColliderを削除
        RemoveExtraColliders(requiredColliders);
    }

    private void CreateOrReuseBoxColliderBetweenPoints(Vector3 start, Vector3 end, int index)
    {
        BoxCollider boxCollider;

        // 既存のコライダーを再利用するか新規に生成
        if (index < boxColliders.Count)
        {
            boxCollider = boxColliders[index];
        }
        else
        {
            boxCollider = navSurfaceBox.AddComponent<BoxCollider>();
            boxColliders.Add(boxCollider);
        }

        // 中心と向きを計算
        Vector3 center = (start + end) / 2;
        boxCollider.center = center;

        // サイズを計算（長さは2点間の距離）
        float length = Vector3.Distance(start, end);
        Vector3 size = new Vector3(0.1f, 0.1f, length); // 高さと幅は0.1、長さは2点間の距離
        boxCollider.size = size;

        // コライダーの向きを設定
        boxCollider.transform.LookAt(end);

        Debug.Log($"Updated/Created BoxCollider at center: {center}, size: {size}");
    }

    private void RemoveExtraColliders(int requiredCount)
    {
        // 余分なコライダーを削除
        for (int i = requiredCount; i < boxColliders.Count; i++)
        {
            Destroy(boxColliders[i]);
        }

        // リストをトリム
        boxColliders.RemoveRange(requiredCount, boxColliders.Count - requiredCount);
    }
}