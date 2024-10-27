using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KobitoController : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isOnNavMesh;
    public KobitoPool pool;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Goal").transform;
        }
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.LogError("Not set TargetObject");
            return;
        }

        NavMeshHit hit;
        isOnNavMesh = NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas);

        if (isOnNavMesh)
        {
            // NavMesh上にいる場合はNavMeshAgentで移動を制御
            agent.enabled = true;
            rb.isKinematic = true;  // 物理挙動を停止

            // 目的地を設定
            agent.SetDestination(target.position);
        }
        else
        {
            // NavMeshから外れた場合は物理挙動に切り替え
            agent.enabled = false;
            rb.isKinematic = false;  // 重力で落下させる
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        // ゴールに到達したらプールに戻す
        if (other.CompareTag("Goal"))
        {
            pool.ReturnKobito(gameObject);
        }
        else if (other.CompareTag("Out"))
        {
            pool.ReturnKobito(gameObject);
        }
    }
}
