using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class WeaponData : ScriptableObject
{
    public GameObject prefab;
    public float[] DamagePerLevel;
    public float[] projectilesPerLevel;
    public float[] timeBetweenProjectsPerLevel;
    public float[] piercePerLevel;
    public float[] spawnTimePerLevel;
    public float[] timeToLivePerLevel;

    [HideInInspector]
    [DoNotSerialize]
    public int currentLevel;

    public float GetDamage() { return DamagePerLevel[currentLevel]; }
    public float GetTimeToLive() { return timeToLivePerLevel[currentLevel]; }
    public float GetSpawnTime() { return spawnTimePerLevel[currentLevel]; }
    public float GetPierce() { return piercePerLevel[currentLevel]; }
    public float GetProjectilesCount() { return projectilesPerLevel[currentLevel]; }
    public float GetTimeBetweenSpawns(){ return timeBetweenProjectsPerLevel[currentLevel]; }
}
