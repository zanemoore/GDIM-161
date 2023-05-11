using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitMarker : MonoBehaviour
{
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private float flashTime = 2f;
    [SerializeField] private Health health;
    // Start is called before the first frame update
    void Start()
    {
        hitMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FlashHitMarker()
    {
        hitMarker.SetActive(true);
        Invoke("ResetHitMarker", flashTime);
    }

    private void ResetHitMarker()
    {
        hitMarker.SetActive(false);
    }
}
