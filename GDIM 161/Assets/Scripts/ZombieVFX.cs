using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieVFX : MonoBehaviour
{
    [SerializeField] private Material damageMaterial;
    [SerializeField] private Material actualMaterial;
    [SerializeField] private SkinnedMeshRenderer skin;


    private void Awake()
    {
        actualMaterial = skin.material;
        this.GetComponent<ZombieHealth>().Damaged += IndicateDamage;
    }

    public void IndicateDamage(float damage)
    {
        skin.material = damageMaterial;
        Invoke("ResetSkin", 0.2f);
    }

    public void ResetSkin()
    {
        skin.material = actualMaterial;
    }
}
