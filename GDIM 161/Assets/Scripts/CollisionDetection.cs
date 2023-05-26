using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> players;

    [SerializeField]
    private Collider[] zombieColliders;

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        zombieColliders = GetComponentsInChildren<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            foreach (GameObject p in players)
            {
                foreach (Collider c in zombieColliders)
                {
                    Debug.Log("WORKING?>????");
                    Physics.IgnoreCollision(c.GetComponent<Collider>(), p.GetComponent<Collider>());
                }
            }
        }
    }
}
