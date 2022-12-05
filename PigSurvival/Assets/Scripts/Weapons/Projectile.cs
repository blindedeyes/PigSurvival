using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : GenericWeapon
{
    public float speed;

    public override void Spawn()
    {
        Restore();
    }

    public override void Update()
    {
        //Move the object
        transform.position += transform.right * speed;
    }
}
