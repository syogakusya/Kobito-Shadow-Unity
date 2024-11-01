using System.Collections.Generic;
using UnityEngine;

public class KobitoPool : MonoBehaviour
{
    public GameObject kobitoPrefab; // 小人のプレハブ
    public int poolSize = 10; // プールのサイズ
    public Transform spawnPoint; // 生成地点
    public Transform targetObject;

    private Queue<GameObject> kobitoPool;

    private void Start()
    {
        // プールの初期化
        kobitoPool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject kobito = Instantiate(kobitoPrefab);
            kobito.SetActive(false); // 非アクティブ化してプールに追加
            var kobitoController = kobito.GetComponent<KobitoController>();
            kobitoController.target = targetObject;
            kobitoController.pool = this;
            kobitoPool.Enqueue(kobito);
        }
    }

    public GameObject GetKobito()
    {
        if (kobitoPool.Count > 0)
        {
            GameObject kobito = kobitoPool.Dequeue();
            kobito.SetActive(true);
            kobito.transform.position = spawnPoint.position; // スポーン地点に移動
            return kobito;
        }
        else
        {
            //Debug.LogWarning("Kobito pool is empty!");
            return null;
        }
    }

    public void ReturnKobito(GameObject kobito)
    {
        kobito.SetActive(false);
        kobitoPool.Enqueue(kobito); // プールに戻す
    }
}
