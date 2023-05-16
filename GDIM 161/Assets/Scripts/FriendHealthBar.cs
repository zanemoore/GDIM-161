using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendHealthBar : MonoBehaviour
{
    [SerializeField] private GameObject[] blockers;
    [SerializeField] private RawImage health;
    [SerializeField] private int totalHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        health.color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        if (totalHealth <= 25)
        {
            health.color = Color.red;
            if (totalHealth <= 0)
            {
                blockers[blockers.Length - 1].SetActive(true);
                blockers[0].SetActive(true);
                blockers[1].SetActive(true);
                blockers[2].SetActive(true);
            }
            else
            {
                blockers[blockers.Length - 1].SetActive(false);
                blockers[0].SetActive(true);
                blockers[1].SetActive(true);
                blockers[2].SetActive(true);
            }
        }
        else if (totalHealth <= 50)
        {
            health.color = Color.yellow;
            blockers[blockers.Length - 1].SetActive(false);
            blockers[0].SetActive(true);
            blockers[1].SetActive(true);
            blockers[2].SetActive(false);
        }
        else if (totalHealth <= 100)
        {
            health.color = Color.green;
            if (totalHealth <= 75)
            {
                blockers[blockers.Length - 1].SetActive(false);
                blockers[0].SetActive(true);
                blockers[1].SetActive(false);
                blockers[2].SetActive(false);
            }
            else
            {
                blockers[blockers.Length - 1].SetActive(false);
                blockers[0].SetActive(false);
                blockers[1].SetActive(false);
                blockers[2].SetActive(false);
            }
        }
    }
}
