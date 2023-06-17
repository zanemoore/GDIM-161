using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollisionDetection : MonoBehaviour
{
    //[SerializeField]
    //private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Zombie")
        //{
            //Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);

            //agent.isStopped = true;
            //agent.speed = 0;
        //}
    }
}
