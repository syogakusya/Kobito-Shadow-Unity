using System.Collections;
using UnityEngine;

public class KobitoSpawner : MonoBehaviour
{
    public KobitoPool pool; // プールの参照
    public float spawnInterval = 5f; // 生成間隔

    private void Start()
    {
        StartCoroutine(SpawnKobitoCoroutine());
    }

    private IEnumerator SpawnKobitoCoroutine()
    {
        while (true)
        {
            GameObject kobito = pool.GetKobito();
            if (kobito != null)
            {
                kobito.SetActive(true);
            }
            yield return new WaitForSeconds(spawnInterval); // 生成間隔を待機
        }
    }
}
