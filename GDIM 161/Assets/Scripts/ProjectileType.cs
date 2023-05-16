using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/ProjectileType")]
public class ProjectileType : ScriptableObject
{
    public GameObject prefab;
    public float forwardForce;
    public float verticalForce;
    public float timeBetweenLaunches;
}
