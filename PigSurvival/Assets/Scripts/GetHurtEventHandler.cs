using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityStats))]
public class GetHurtEventHandler : MonoBehaviour
{
    Animator animator;
    EntityStats stats;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<EntityStats>();

        stats.RegisterOnDamage(DamageTaken);
    }

    private void DamageTaken(EntityStats e, float damage)
    {
        animator.SetTrigger("HIT");
    }
}
