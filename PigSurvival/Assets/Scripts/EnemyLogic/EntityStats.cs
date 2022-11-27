using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStats : MonoBehaviour
{
    private Transform transf;
    public new Transform transform
    {
        get
        {
            if(transf == null) transf = GetComponent<Transform>();
            return transf;
        }
    }

    public float Health;
    public float Speed;
    public float TouchDamage;
    [HideInInspector]
    public float SpeedModifier;

    [HideInInspector]
    public int TransformArrayIndex = -1;

    private float currentHealth;
    
    public bool IsActive {  get {  return currentHealth > 0; } }

    public delegate void EntityStatEvent(EntityStats e);
    private EntityStatEvent deathEvent;

    public void Init()
    {
        currentHealth = Health;
        SpeedModifier = 0;
        deathEvent = null;
    }

    public void RegisterOnDeath(EntityStatEvent e)
    {
        deathEvent -= e;
        deathEvent += e;
    }

    public void UnregisterOnDeath(EntityStatEvent e)
    {
        deathEvent -= e;
    }

    public void TakeDamage(float Damage)
    {
        Health -= Damage;

        if (Health <= 0) deathEvent?.Invoke(this);
    }

    public void AddSpeedModifier(float mod)
    {
        SpeedModifier += mod;
    }

    public void ReduceSpeedModifier(float mod)
    {
        SpeedModifier -= mod;
    }
}
