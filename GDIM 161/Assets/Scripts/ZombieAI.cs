using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

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
    private List<Transform> playerTransform;

    [SerializeField] 
    private Animator animator;
    [SerializeField] 
    private GameObject lookAt;

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
        playerPosition = Vector3.zero;
    }


    void Update()
    {
        // Checks if Zombie can see player
        ZombieView();
        playerTransform = GameObject.FindGameObjectsWithTag("Player").Select(go => go.transform).ToList();

        if ((isAwareOfPlayer == true) && (isAttacking == false))
        {
            Chase();  // If the zombie is aware of the player, it will chase them
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        foreach (GameObject p in players)
        {
            if (p == null)
            {
                isAwareOfPlayer = false;
                agent.isStopped = true;
                agent.speed = 0;
                animator.SetBool("Idle", true);
                animator.SetBool("Attacking", false);
                players.Remove(p);
            }
        }

        if (animator.GetBool("Death") == true)
        {
            isAwareOfPlayer = false;
            isAttacking = false;
            ///transform.LookAt(lookAt.transform.position); // im lost...supposed to stop looking at player when in death animation
            agent.isStopped = true;
            agent.speed = 0;
            animator.SetBool("Attacking", false);
        }
    }


    void ZombieView()
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
                            playerPosition = new Vector3(p.transform.position.x, transform.position.y, p.transform.position.z);
                            transform.LookAt(playerPosition);
                            isAttacking = true;
                            animator.SetBool("Attacking", true);
                            Attack();
                        }
                        else
                        {
                            isAttacking = false;
                            animator.SetBool("Attacking", false);
                            Move(moveSpeed);
                        }
                    }
                }
            }
            else
            {
                isAwareOfPlayer = false;
                sfx.idle();
            }
        }
    }

    Transform GetClosestPlayer(List<Transform> players)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Transform potentialTarget in players)
        {
            Vector3 directionToTarget = potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }


    private void Chase()
    {
        Transform near = GetClosestPlayer(playerTransform);
        Vector3 nearPlayer = new Vector3(near.transform.position.x, transform.position.y, near.transform.position.z);

        sfx.chase();
        agent.SetDestination(nearPlayer);
        transform.LookAt(nearPlayer);
        Move(moveSpeed);
    }

    private void Attack()
    {
        Transform near = GetClosestPlayer(playerTransform);
        Vector3 nearPlayer = new Vector3(near.transform.position.x, transform.position.y, near.transform.position.z);
        transform.LookAt(nearPlayer);

        agent.isStopped = true;
        agent.speed = 0;

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }

        if (attackTime <= 0)
        {
            attackTime = 1 / attackRate;
            DamagePlayer();
        }
    }

    void DamagePlayer()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, viewRadius))
        {
            if (hit.transform.gameObject.GetComponent<PlayerHealth>())
            {
                //sfx.attack();
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, attackDamage);
            }
        }

        //foreach (GameObject p in players)
        //{
        //PlayerHealth healthscript = p.GetComponent<PlayerHealth>();

        //if (healthscript != null)
        //{
        //attackTime = Time.time;
        //healthscript.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, attackDamage);
        //}
        //}
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
