using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
public class DeathEventHandler : MonoBehaviour
{
    public GameObject expPrefab;
    private Animator anim;
    EntityStats stats;
    // Start is called before the first frame update

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        stats = GetComponent<EntityStats>();

        stats.RegisterOnDeath(Death);
    }

    private void Death(EntityStats e)
    {
        GameObject go = ObjectPool.Instance.GetObject(expPrefab);

        go.transform.position = PlayerController.Instance.transform.position;
        //go.transform.position = gameObject.transform.position;

        if (anim)
        {
            anim.SetTrigger("Die");
        }


        go.GetComponent<ExpObject>().EnableExp();
    }
}
