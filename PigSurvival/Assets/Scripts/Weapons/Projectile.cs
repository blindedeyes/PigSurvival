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
        //Move the object
        transform.position += (transform.right * speed * Time.deltaTime);
    }
}
