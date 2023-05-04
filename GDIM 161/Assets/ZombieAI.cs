using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private bool isAwareOfPlayer;
    [SerializeField]
    private float distanceThreshold;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }


    void Chase()
    {
        agent.SetDestination(target.transform.position);
    }


    void Update()
    {
        if (isAwareOfPlayer)
        {
            Chase();
        }
        else
        {
            float distance = Vector3.Distance(target.transform.position, this.transform.position);
            if (distance < distanceThreshold)
            {
                isAwareOfPlayer = true;
            }
        }
    }
}
