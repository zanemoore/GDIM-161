using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public class ZombieAI : MonoBehaviourPunCallbacks
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float viewRadius;
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask spectatorLayer;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int attackDamage;
    [SerializeField] private float attackRate;
    [SerializeField] private float attackDistance;
    [SerializeField] private float attackAngle;

    [SerializeField] private List<GameObject> players;
    [SerializeField] private List<Transform> playerTransform;

    [SerializeField] Animator animator;
    [SerializeField] GameObject lookAt;
    [SerializeField] private List<GameObject> zombieColliders;

    private Vector3 playerPosition;
    private bool isAwareOfPlayer;
    private bool isAttacking;
    private float attackTime;
    private ZombieSFXScript sfx;
    private Vector3 _destination;
    private bool _isSetDestination;

    void Start()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        zombieColliders = new List<GameObject>(GameObject.FindGameObjectsWithTag("Zombie Arm"));
        agent = GetComponent<NavMeshAgent>();
        sfx = GetComponent<ZombieSFXScript>();
        isAwareOfPlayer = false;
        isAttacking = false;
        playerPosition = Vector3.zero;

        if (_isSetDestination == false)
        {
            _destination = Vector3.zero;
        }

        foreach (GameObject z in zombieColliders)
        {
            foreach (GameObject p in players)
            {
                Physics.IgnoreCollision(p.GetComponent<CharacterController>(), z.GetComponent<Collider>(), true);
            }
        }
    }


    void Update()
    {
        if (photonView.IsMine)
        {
            //Checks if Zombie can see player
            ZombieView();
        }

        playerTransform = GameObject.FindGameObjectsWithTag("Player").Select(go => go.transform).ToList();

        if (_isSetDestination == true)
        {
            agent.SetDestination(_destination);
            transform.LookAt(_destination);
            Move(moveSpeed);
            animator.SetBool("Walking", true);
        }
        else if ((isAwareOfPlayer == true) && (isAttacking == false))
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
            if (p.GetComponent<PlayerHealth>().health <= 0)
            {
                isAwareOfPlayer = false;
                isAttacking = false;
                agent.isStopped = true;
                agent.speed = 0;
                animator.SetBool("Idle", true);
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

        if (isAttacking == true)
        {
            animator.SetBool("Attacking", true);
        }
        else
        {
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
                    _isSetDestination = false;

                    if (distanceToPlayer <= attackDistance)
                    {
                        isAttacking = true;
                        Attack();
                    }
                    else
                    {
                        isAttacking = false;
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
        //Transform near = GetClosestPlayer(playerTransform);
        //Vector3 nearPlayer = new Vector3(near.transform.position.x, transform.position.y, near.transform.position.z);

        sfx.chase();
        agent.SetDestination(playerPosition);
        transform.LookAt(playerPosition);
        Move(moveSpeed);
    }

    private void Attack()
    {
        Vector3 currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        agent.SetDestination(currentPosition);
        transform.LookAt(playerPosition);
        agent.isStopped = true;
        agent.speed = 0;

        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }

        if (attackTime <= 0)
        {
            attackTime = 1 / attackRate;
            //DamagePlayer();
            Damage();
        }
    }

    /*
    void DamagePlayer()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, viewRadius))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            if (hit.collider.gameObject.GetComponent<PlayerHealth>() && hit.transform.gameObject.tag != "SpectatorCamera")
            {
                //sfx.attack();
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, attackDamage);
            }
        }
    }
    */

    private void Damage()
    {
        Collider[] playerInView = Physics.OverlapSphere(transform.position, attackDistance, playerLayer);

        for (int i = 0; i < playerInView.Length; i++)
        {
            Transform player = playerInView[i].transform;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToPlayer) < attackAngle / 2)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);

                if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, wallLayer))
                {
                    if (distanceToPlayer <= attackDistance)
                    {
                        player.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, attackDamage);
                    }
                }
            }
        }
    }

    private void Move(float speed)
    {
        agent.isStopped = false;
        agent.speed = speed;
    }


    public void SetDestination(Vector3 destination)
    {
        _isSetDestination = true;
        _destination = destination;
    }

    /*
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, attackDistance);
        Vector3 viewAngle1 = DirectionFromAngle(transform.eulerAngles.y, -attackAngle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(transform.eulerAngles.y, attackAngle / 2);
        Handles.color = Color.yellow;
        Handles.DrawLine(transform.position, transform.position + viewAngle1 * attackDistance);
        Handles.DrawLine(transform.position, transform.position + viewAngle2 * attackDistance);
    }


    private Vector3 DirectionFromAngle(float eulery, float angle)
    {
        angle += eulery;
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    */
}
