using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
public class DeathEventHandler : MonoBehaviour
{
    public GameObject expPrefab;
    EntityStats stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<EntityStats>();

        stats.RegisterOnDeath(Death);
    }

    private void Death(EntityStats e)
    {
        GameObject go = ObjectPool.Instance.GetObject(expPrefab);

        go.transform.position = gameObject.transform.position;

        Debug.Log(gameObject.transform.position);
        go.GetComponent<ExpObject>().EnableExp();
    }
}
