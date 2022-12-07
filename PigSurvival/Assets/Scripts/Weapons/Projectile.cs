using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GenericWeapon
{
    public float speed;


    protected override void Restore()
    {
        base.Restore();
        TTLTimer.Time = myData.GetProjectileTimeToLive();
    }

    public override void Spawn()
    {
        Restore();
    }

    public override void Update()
    {
        if (TTLTimer.Tick(Time.deltaTime))
        {
            ObjectPool.Instance.FreeObject(this.gameObject);
        }
        if (hitList.Count > myData.GetPierce() ) { ObjectPool.Instance.FreeObject(this.gameObject); }
        //Move the object
        transform.position += (transform.right * speed * Time.deltaTime);
        
    }
}
