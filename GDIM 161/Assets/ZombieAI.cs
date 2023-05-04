using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class ZombieAI : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private float viewRadius;
    [SerializeField]
    private float viewAngle;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float stopDistance;

    private GameObject target;
    private Vector3 playerPosition;
    private bool isAwareOfPlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        isAwareOfPlayer = false;
        playerPosition = Vector3.zero;
    }


    void Update()
    {
        // Checks if Zombie can see player
        ZombieView();

        if (isAwareOfPlayer)
        {
            Chase();  // If the zombie is aware of the player, it will chase them
        }
    }


    private void ZombieView()
    {
        Collider[] playerInView = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);
        for (int i = 0; i < playerInView.Length; i++)
        {
            Transform player = playerInView[i].transform;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, wallLayer))
                {
                    isAwareOfPlayer = true;
                    /*
                    if (Vector3.Distance(transform.position, player.position) <= stopDistance)
                    {
                        agent.isStopped = true;
                        agent.speed = 0;
                    }
                    */
                }
            }
            else
            {
                isAwareOfPlayer = false;
            }

            if (isAwareOfPlayer == true)
            {
                playerPosition = player.transform.position;
            }
        }
    }


    private void Chase()
    {
        agent.SetDestination(playerPosition);
        agent.transform.LookAt(playerPosition);
        Move(moveSpeed);
    }


    private void Move(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
    }

    /*
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, viewRadius);
        Vector3 viewAngle1 = DirectionFromAngle(transform.eulerAngles.y, -viewAngle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(transform.eulerAngles.y, viewAngle / 2);
        Handles.color = Color.yellow;
        Handles.DrawLine(transform.position, transform.position + viewAngle1 * viewRadius);
        Handles.DrawLine(transform.position, transform.position + viewAngle2 * viewRadius);
    }


    private Vector3 DirectionFromAngle(float eulery, float angle)
    {
        angle += eulery;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    */
}
