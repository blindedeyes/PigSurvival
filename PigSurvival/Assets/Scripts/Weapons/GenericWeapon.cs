using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericWeapon : MonoBehaviour
{
    [HideInInspector]
    public WeaponData myData;


    protected Animator myAnimator;

    protected HashSet<GameObject> hitList= new HashSet<GameObject>();
    protected Timer TTLTimer;

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

        transform.right = PlayerController.Instance.moveDir;
        AudioMaster.Instance.PlaySound(myData.soundEffect);
        //transform.rotation = playerTrans.rotation;
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

        if (hitList.Count >= myData.GetPierce())
        {
            TTLTimer.Value = float.MaxValue;
            return;
        }

        if (!hitList.Contains(collision.gameObject))
        {
            hitList.Add(collision.gameObject);
            var enemy = collision.gameObject;
            
            var stats = enemy.transform.parent.GetComponent<EntityStats>();
            if(stats.IsActive) { stats.TakeDamage(myData.GetDamage()); }
        }
    }
}
