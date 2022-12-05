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
    public float[] projectileTimeToLive;

    [HideInInspector]
    [DoNotSerialize]
    public int currentLevel;

    public float GetDamage() { return DamagePerLevel[GetClampedLevel()]; }
    public float GetTimeToLive() { return timeToLivePerLevel[GetClampedLevel()]; }
    public float GetSpawnTime() { return spawnTimePerLevel[GetClampedLevel()]; }
    public float GetPierce() { return piercePerLevel[GetClampedLevel()]; }
    public float GetProjectilesCount() { return projectilesPerLevel[GetClampedLevel()]; }
    public float GetTimeBetweenSpawns(){ return timeBetweenProjectsPerLevel[GetClampedLevel()]; }

    public float GetProjectileTimeToLive() { return projectileTimeToLive[GetClampedLevel()]; }

    private int GetClampedLevel()
    {
        return Mathf.Clamp(currentLevel, 0, projectileTimeToLive.Length);
    }
}
