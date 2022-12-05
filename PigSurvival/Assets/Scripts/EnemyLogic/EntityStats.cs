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

    [HideInInspector]
    public float currentHealth;
    
    public bool IsActive {  get {  return Mathf.FloorToInt(currentHealth) > 0; } }

    public delegate void EntityStatEvent(EntityStats e);
    public delegate void EntityDamagedStatEvent(EntityStats e,float damage);
    private EntityStatEvent deathEvent;
    private EntityDamagedStatEvent damageEvent;

    public void Init()
    {
        currentHealth = Health;
        SpeedModifier = 0;
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

    public void RegisterOnDamage(EntityDamagedStatEvent e)
    {
        damageEvent -= e;
        damageEvent += e;
    }

    public void UnregisterOnDamage(EntityDamagedStatEvent e)
    {
        damageEvent -= e;
    }

    public void TakeDamage(float Damage)
    {
        currentHealth -= Damage;
        damageEvent?.Invoke(this, Damage);
        if (Mathf.FloorToInt(currentHealth) <= 0) 
            deathEvent?.Invoke(this);
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
