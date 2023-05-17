using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Photon.Realtime;
using Photon.Pun;

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
    private int attackDamage;
    [SerializeField]
    private float attackRate;
    [SerializeField]
    private float attackDistance;

    [SerializeField]
    private List<GameObject> players;

    [SerializeField] 
    Animator animator;
    [SerializeField] 
    GameObject lookAt;

    private Vector3 playerPosition;
    private bool isAwareOfPlayer;
    private bool isAttacking;
    private float attackTime;
    private ZombieSFXScript sfx;

    void Start()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        agent = GetComponent<NavMeshAgent>();
        sfx = GetComponent<ZombieSFXScript>();
        isAwareOfPlayer = false;
        isAttacking = false;
        attackTime = 0;
        playerPosition = Vector3.zero;
    }


    void Update()
    {
        // Checks if Zombie can see player
        ZombieView();

        if ((isAwareOfPlayer == true) && (isAttacking == false))
        {
            Chase();  // If the zombie is aware of the player, it will chase them
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                players.Remove(players[i]);
                isAwareOfPlayer = false;
                agent.isStopped = true;
                agent.speed = 0;
                animator.SetBool("Idle", true);
                animator.SetBool("Attacking", false);
            }
        }

        if (animator.GetBool("Death") == true)
        {
            isAwareOfPlayer = false;
            isAttacking = false;
            transform.LookAt(lookAt.transform.position); // im lost...supposed to stop looking at player when in death animation
            agent.isStopped = true;
            agent.speed = 0;
            animator.SetBool("Attacking", false);
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

                    foreach (GameObject p in players)
                    {
                        if (Vector3.Distance(transform.position, p.transform.position) <= attackDistance)
                        {
                            isAttacking = true;
                            animator.SetBool("Attacking", true);
                            AttackPlayer();
                        }
                        else
                        {
                            isAttacking = false;
                            animator.SetBool("Attacking", false);
                        }
                    }
                }
            }
            else
            {
                isAwareOfPlayer = false;
                sfx.idle();
            }

            if (isAwareOfPlayer == true)
            {
                playerPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            }
        }
    }


    private void Chase()
    {
        sfx.chase();
        agent.SetDestination(playerPosition);
        transform.LookAt(playerPosition);
        Move(moveSpeed);
    }

    private void AttackPlayer()
    {
        if (isAttacking == true)
        {
            //sfx.attack();
            transform.LookAt(playerPosition);
            agent.isStopped = true;
            agent.speed = 0;
        }

        StartCoroutine("Delay");

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject p in players)
        {
            PlayerHealth healthscript = p.GetComponent<PlayerHealth>();

            if (Time.time - attackTime > attackRate)
            {
                if (healthscript != null)
                {
                    attackTime = Time.time;
                    healthscript.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, attackDamage);
                }
            }
        }
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
