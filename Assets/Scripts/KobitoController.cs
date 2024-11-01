using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KobitoController : MonoBehaviour
{
    public Transform target;
    public KobitoPool pool;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private bool isOnNavMesh;
    private Animator anim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        agent.speed = Random.Range(0.8f, 3.5f);

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
            transform.rotation = Quaternion.Euler(0, 0, -1);
            anim.SetBool("isWalking", true);
            Vector3 direction = agent.desiredVelocity.normalized;
            if(direction.x < 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }else if(direction.x > 0)
            {
                transform.localScale = new Vector3(-1,1,1);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }

            if(agent.velocity.magnitude < 1)
            {
                anim.SetBool("isWalking", false);
            }

        }
        else
        {
            // NavMeshから外れた場合は物理挙動に切り替え
            agent.enabled = false;
            rb.isKinematic = false;  // 重力で落下させる
            anim.SetBool("isWalking", false);
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
