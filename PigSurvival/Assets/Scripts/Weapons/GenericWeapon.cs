using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWeapon : MonoBehaviour
{
    [HideInInspector]
    public WeaponData myData;


    Animator myAnimator;

    private HashSet<GameObject> hitList= new HashSet<GameObject>();
    private Timer TTLTimer;

    public void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    protected virtual void Restore()
    {
        hitList.Clear(); 
        TTLTimer.Value = 0;
        TTLTimer.Time = myData.GetTimeToLive();
    }

    public virtual void Spawn()
    {
        Restore();

        myAnimator.SetTrigger("Attack");

        var playerTrans = PlayerController.Instance.MyTransform;
        transform.position = playerTrans.position;
        transform.rotation = playerTrans.rotation;
    }

    public virtual void Update()
    {
        if(TTLTimer.Tick(Time.deltaTime))
        {
            ObjectPool.Instance.FreeObject(this.gameObject);
        }
        else
        {
            var playerTrans = PlayerController.Instance.MyTransform;
            transform.position = playerTrans.position;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(!hitList.Contains(collision.gameObject))
        {
            hitList.Add(collision.gameObject);
            var enemy = collision.gameObject;
            
            var stats = enemy.GetComponent<EntityStats>();
            if(stats.IsActive) { stats.TakeDamage(myData.GetDamage()); }
        }
    }
}
