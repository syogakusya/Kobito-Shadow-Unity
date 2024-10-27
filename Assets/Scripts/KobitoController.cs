using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KobitoController : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if(target == null)
        {
            target = GameObject.FindGameObjectWithTag("Goal").transform;
        }
    }

    private void Update()
    {
        if(target == null)
        {
            Debug.LogError("Not set TargetObject");
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(target.position, out hit, 1.0f, NavMesh.AllAreas)) {
            agent.SetDestination(target.position);
        }
    }
}
